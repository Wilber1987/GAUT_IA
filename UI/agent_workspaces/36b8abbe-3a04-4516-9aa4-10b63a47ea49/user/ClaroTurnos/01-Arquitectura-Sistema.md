# Sistema de Gestión de Turnos — CLARO
## Documento de Arquitectura Técnica

**Proyecto:** Claro Turnos (nombre interno del sistema)
**Versión:** 1.0 — MVP
**Fecha:** 20/08/2026

---

## 1. Visión general

Sistema de gestión de filas por tickets numerados para puntos de atención de Claro. Reemplaza la fila física por un ticket impreso (o virtual) con número correlativo por tipo de servicio, visible en pantalla pública y gestionado por agentes desde un panel de atención.

Módulos: Kiosco, Panel de Recepción, Panel de Agentes, Pantalla Pública, Administración y Reportes.

---

## 2. Arquitectura de la solución

### 2.1 Estilo arquitectónico

Arquitectura por capas (N-Tier) + Clean Architecture en el backend, con separación estricta de responsabilidades:

```
┌─────────────────────────────────────────────────────────┐
│                    CLIENTES (Frontend)                   │
│  Kiosco (touch) · Recepción · Agentes · Pantalla Pública │
│              · Panel Administrativo                      │
│         React 18 + Bootstrap 5 + Axios + SignalR client  │
└───────────────────────┬───────────────────────────────────┘
                         │ HTTPS / REST + WebSockets (SignalR)
┌───────────────────────▼───────────────────────────────────┐
│                  ClaroTurnos.Api                          │
│   Controllers · Middlewares · JWT Auth · Swagger · Hubs   │
├─────────────────────────────────────────────────────────┤
│                ClaroTurnos.Application                    │
│   Servicios de negocio · DTOs · Validadores (FluentVal.)  │
│   Interfaces de repositorios · Reglas de turnos           │
├─────────────────────────────────────────────────────────┤
│                  ClaroTurnos.Domain                        │
│   Entidades · Enums · Value Objects · Reglas de dominio    │
├─────────────────────────────────────────────────────────┤
│               ClaroTurnos.Infrastructure                   │
│   EF Core 8 · Repositorios · DbContext · Auditoría         │
│   Servicio de impresión térmica (ESC/POS) · Logs (Serilog) │
└───────────────────────┬───────────────────────────────────┘
                         │
                ┌────────▼────────┐
                │  SQL Server 2022 │
                └──────────────────┘
```

### 2.2 Justificación de decisiones clave

| Decisión | Justificación |
|---|---|
| ASP.NET Core 8 Web API | LTS, alto rendimiento, nativo para JWT/Swagger/EF Core |
| React sobre Blazor Server | Mejor soporte para kiosco offline-first, pantalla pública con animaciones/audio, y reactividad en tiempo real vía WebSockets sin round-trip al servidor por cada evento de UI |
| SignalR (WebSockets) | La Pantalla Pública y el Panel de Recepción necesitan reflejar en tiempo real cada "llamado" de turno sin polling constante |
| EF Core 8 + SQL Server 2022 | Estándar corporativo, soporta auditoría con `IAuditable`, particionamiento por sucursal a futuro |
| Arquitectura por capas | Aísla reglas de negocio (numeración, transferencias, SLA) de infraestructura, facilita pruebas unitarias |
| JWT + Roles | Login de agentes/administradores con expiración corta y refresh token |

### 2.3 Comunicación en tiempo real

- **REST API**: operaciones CRUD (crear ticket, login, reportes).
- **SignalR Hub `/hubs/turnos`**: eventos push:
  - `TicketCreado` → refresca contador de "clientes en espera" en Kiosco/Recepción.
  - `TicketLlamado` → dispara animación + voz en Pantalla Pública, refresca Panel de Agentes/Recepción.
  - `TicketFinalizado`, `TicketTransferido`, `TicketAusente` → actualizan estados en Recepción/Reportes.

---

## 3. Módulos funcionales

### 3.1 Kiosco de generación de turnos
Pantalla táctil full-screen. Flujo: bienvenida → selección de servicio (5 botones grandes con ícono) → generación de ticket → impresión térmica → confirmación 5s → vuelve a bienvenida.
Ticket impreso incluye: logo Claro, número (ej. `A-001`), tipo de atención, fecha/hora, clientes en espera delante del ticket generado, mensaje de bienvenida y código de sucursal.

### 3.2 Panel de Recepción
Vista de supervisor: cola por servicio, cantidad en espera, tiempo promedio de atención (rolling del día), y tabla de tickets con estado (Esperando, En atención, Atendido, Cancelado, Ausente). Filtros por sucursal y servicio.

### 3.3 Panel de Agentes
Login con JWT (usuario/contraseña). Selección de ventanilla asignada. Acciones: Llamar siguiente (respeta orden FIFO dentro del/los servicios asignados al agente), Repetir llamado (re-emite evento de voz/pantalla), Transferir (cambia el servicio del ticket y lo reinserta en cola conservando prioridad de espera), Finalizar atención (cierra el ticket, registra `Atencion` con duración), Marcar ausente (tras N llamados sin respuesta).

### 3.4 Pantalla Pública
Últimos 10 turnos llamados con ventanilla, reloj en vivo, animación de "nuevo llamado" (highlight + slide-in) y anuncio de voz sintetizada: *"Turno A-001, pasar a ventanilla 3"* mediante Web Speech API en el cliente, disparado por el evento `TicketLlamado` de SignalR (con opción de integrarse a un locutor físico vía altavoces IP en producción).

### 3.5 Módulo Administrativo
- Gestión de usuarios (agentes, supervisores, administradores) y roles.
- Gestión de servicios (alta/baja, orden de prioridad, SLA objetivo).
- Configuración de prefijos por servicio (A=Ventas, B=Servicio al Cliente, C=Soporte Técnico, D=Facturación, E=Reclamos), con reinicio de numeración diario configurable.
- Configuración de impresoras térmicas por sucursal/kiosco (IP o USB, ancho de papel, plantilla).
- Configuración de sucursales (dirección, horario, ventanillas disponibles).

### 3.6 Reportes
- Clientes atendidos por día / servicio / sucursal.
- Tiempo promedio de espera y de atención (con desviación).
- Rendimiento por agente (atendidos, tiempo promedio, ausentes, transferencias).
- Exportación a Excel (ClosedXML) y PDF (QuestPDF).
- Dashboard con gráficas (Chart.js en frontend, agregaciones vía endpoints `/api/reportes/*`).

---

## 4. Casos de uso principales

| ID | Caso de uso | Actor | Descripción |
|---|---|---|---|
| CU-01 | Generar turno | Cliente (kiosco) | Selecciona servicio, el sistema genera número correlativo y lo imprime |
| CU-02 | Iniciar sesión de agente | Agente | Autenticación JWT y asignación de ventanilla |
| CU-03 | Llamar siguiente turno | Agente | Sistema selecciona el ticket más antiguo en estado Esperando del/los servicios del agente y lo marca En atención |
| CU-04 | Repetir llamado | Agente | Reemite el evento de llamado (voz + pantalla) sin cambiar estado |
| CU-05 | Transferir ticket | Agente | Cambia el servicio de un ticket En atención y lo regresa a Esperando en la nueva cola |
| CU-06 | Finalizar atención | Agente | Cambia estado a Atendido y registra duración en tabla Atenciones |
| CU-07 | Marcar cliente ausente | Agente | Cambia estado a Ausente tras no presentarse al llamado |
| CU-08 | Monitorear colas | Supervisor (Recepción) | Visualiza en tiempo real colas, tiempos y estados |
| CU-09 | Administrar catálogos | Administrador | CRUD de usuarios, servicios, prefijos, impresoras, sucursales |
| CU-10 | Generar reportes | Administrador/Supervisor | Consulta y exporta métricas de operación |
| CU-11 | Anunciar turno en pantalla pública | Sistema | Al recibir `TicketLlamado`, muestra y narra el llamado |

---

## 5. Flujo de atención (resumen)

`Kiosco genera ticket (Esperando)` → `Agente llama siguiente (En atención)` → `Finaliza (Atendido)` **o** `Ausente tras reintentos` **o** `Transferido → vuelve a Esperando en otro servicio`.
Ver diagrama detallado en `04-Diagrama-Flujo-Atencion.mermaid`.

---

## 6. Modelo entidad-relación (resumen)

Entidades principales: `Sucursales 1—N Ventanillas`, `Sucursales 1—N Kioscos/Impresoras`, `Servicios 1—N Tickets`, `Usuarios N—N Roles`, `Tickets 1—N Atenciones` (historial de cada llamado/transferencia), `EstadosTicket` como catálogo, `Configuraciones` clave/valor por sucursal.
Ver diagrama completo en `03-Diagrama-ER.mermaid` y script en `02-Modelo-Base-Datos.sql`.

---

## 7. Estructura de carpetas del proyecto

### Backend (`/backend`)

```
ClaroTurnos.Api/
├── ClaroTurnos.Domain/
│   ├── Entities/           (Ticket, Servicio, Usuario, Sucursal, Ventanilla, Atencion, ...)
│   ├── Enums/              (EstadoTicket, RolUsuario)
│   └── Common/             (BaseEntity, IAuditable)
├── ClaroTurnos.Application/
│   ├── DTOs/
│   ├── Interfaces/         (ITicketRepository, ITicketService, IPrinterService...)
│   ├── Services/           (TicketService, ReporteService, AuthService)
│   └── Validators/         (FluentValidation)
├── ClaroTurnos.Infrastructure/
│   ├── Data/               (ClaroTurnosDbContext, Migrations)
│   ├── Repositories/
│   ├── Printing/           (EscPosPrinterService)
│   └── Logging/            (Serilog config)
├── ClaroTurnos.Api/
│   ├── Controllers/        (TicketsController, AuthController, AgentesController,
│   │                        AdminController, ReportesController)
│   ├── Hubs/               (TurnosHub — SignalR)
│   ├── Middlewares/        (ExceptionHandling, AuditoriaMiddleware)
│   ├── Program.cs
│   └── appsettings.json
└── ClaroTurnos.Tests/       (xUnit + Moq)
```

### Frontend (`/frontend`)

```
claro-turnos-frontend/
├── public/
├── src/
│   ├── components/          (TicketCard, ColaServicio, VentanillaBadge, VozAnuncio)
│   ├── pages/               (Kiosco.jsx, Recepcion.jsx, Agente.jsx,
│   │                         PantallaPublica.jsx, Admin.jsx, Reportes.jsx)
│   ├── services/            (api.js, signalr.js, auth.js)
│   ├── context/             (AuthContext.jsx)
│   ├── App.jsx
│   └── main.jsx
├── package.json
└── vite.config.js
```

---

## 8. Seguridad y auditoría

- **JWT**: access token (15 min) + refresh token (8 h), claims con `rol` y `sucursalId`.
- **Roles**: `Administrador`, `Supervisor`, `Agente`.
- **Auditoría**: middleware que registra en `Auditoria` cada acción sensible (login, cambios de estado de ticket, cambios de configuración) con usuario, IP, timestamp y payload.
- **Logs de eventos**: Serilog → archivo rotativo + tabla `LogsEventos` para eventos de negocio (impresión fallida, desconexión de impresora, SLA excedido).
- **Swagger**: documentación OpenAPI con esquema de seguridad Bearer, agrupado por módulo.
