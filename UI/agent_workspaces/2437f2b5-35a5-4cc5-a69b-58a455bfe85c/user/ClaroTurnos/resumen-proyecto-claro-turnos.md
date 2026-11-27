# Resumen del Proyecto: Claro Turnos

## Visión General
Sistema de gestión de filas por tickets numerados para puntos de atención de Claro, diseñado para reemplazar la fila física mediante un ticket impreso (o virtual) con número correlativo por tipo de servicio. El sistema es visible en una pantalla pública y gestionado por agentes desde paneles especializados.

## Módulos Principales
- **Kiosco:** Interfaz táctil donde los clientes solicitan su turno e imprimen el ticket térmico.
- **Panel de Recepción:** Gestión inicial de la cola y asignación de ventanillas.
- **Panel de Agentes:** Permite llamar, repetir, transferir o finalizar turnos; gestionar ausencias en tiempo real.
- **Pantalla Pública:** Muestra el turno actual atendido y las personas antes de usted con animaciones y voz (en futuras mejoras).
- **Administración:** CRUD para usuarios, servicios, prefijos y sucursales.
- **Reportes:** Atendidos por día, tiempos promedio y exportación a Excel/PDF.

## Arquitectura Técnica
- **Estilo Arquitectónico:** N-Tier + Clean Architecture en el backend.
- **Backend (C#):**
  - `ClaroTurnos.Api`: Controladores, Middlewares, JWT Auth, Swagger, Hubs SignalR.
  - `ClaroTurnos.Application`: Servicios de negocio, DTOs, Validadores (FluentValidation), interfaces de repositorios y reglas de turnos.
  - `ClaroTurnos.Domain`: Entidades, Enums, Value Objects y Reglas de Negocio.
- **Frontend:** React 18 + Bootstrap 5 + Axios + SignalR client.

## Modelo de Datos (SQL Server)
El sistema utiliza un modelo relacional con tablas clave como:
- `Roles` (Administrador, Supervisor, Agente).
- `Sucursales` y `Servicios` (con prefijos únicos por sucursal: A-Ventas, B-Servicio al Cliente, C-Soporte Técnico, D-Facturación, E-Reclamos).
- `EstadosTicket`, `Usuarios`, `Ventanillas`, etc.

## Impresión de Tickets Térmicos (MVP)
Para el MVP, la impresión se centraliza en el backend:
1. El cliente solicita un turno vía API (`POST /api/tickets`).
2. El backend genera el buffer ESC/POS y lo envía por socket TCP al puerto 9100 de la impresora IP configurada.
3. **Plantilla del Ticket:** Incluye logo Claro, código de servicio (ej: A-001), fecha/hora, personas antes de usted y sucursal.

## Plan MVP y Estimación (~12.5 semanas)
| Fase | Contenido |
|---|---|
| 0. Preparación | Repositorio, CI/CD básico, entorno SQL Server (1 semana) |
| 1. Backend base | Dominio, EF Core + migraciones, autenticación JWT, CRUD de catálogos (2 semanas) |
| 2. Motor de turnos | Generación de tickets, numeración correlativa, endpoints de cola y SignalR Hub (1.5 semanas) |
| 3. Frontend Kiosco + Pantalla Pública | UI táctil e impresión (2 semanas) |
| 4. Frontend Agentes + Recepción | Login, llamar/repetir/transferir/finalizar/ausente y panel de colas en vivo (1.5 semanas) |
| 5. Administración y Reportes | CRUD administrativo, dashboard y exportación Excel/PDF (1.5 semanas) |
| 6. Impresión térmica real | Integración con impresora física y manejo de errores (1 semana) |
| 7. QA, auditoría, hardening | Pruebas end-to-end, logs y ajuste de SLA/roles (1 semana) |
| 8. Piloto y estabilización | Despliegue en sucursal piloto (1 semana) |

## Mejoras Futuras (Fuera del alcance MVP)
- Multi-sucursal avanzado con reportes comparativos.
- App móvil para clientes.
- Integración con locutor físico por altavoces.
- IA para predicción de demanda.