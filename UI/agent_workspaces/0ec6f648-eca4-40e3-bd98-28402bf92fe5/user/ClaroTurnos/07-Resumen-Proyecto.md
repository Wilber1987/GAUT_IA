# Resumen del Proyecto — Claro Turnos

## Visión General
Sistema de gestión de filas por tickets numerados para puntos de atención de Claro, que reemplaza la fila física por un ticket impreso (o virtual) con número correlativo por tipo de servicio. El sistema es visible en pantalla pública y gestionado por agentes desde un panel de atención.

## Módulos del Sistema
- **Kiosco**: Pantalla táctil para emisión de tickets físicos/temáticos.
- **Panel de Recepción**: Gestión inicial de colas, impresión térmica vía TCP/IP a impresoras ESC/POS (58mm/80mm).
- **Panel de Agentes**: Login, llamar/repetir/transferir/finalizar/ausente, panel de colas en vivo.
- **Pantalla Pública**: Animaciones y voz para mostrar turnos actuales.
- **Administración**: CRUD de usuarios, servicios, prefijos, sucursales (MVP: única).
- **Reportes**: Atendidos por día, tiempos promedio, exportación Excel/PDF.

## Arquitectura Técnica
**Estilo arquitectónico:** N-Tier + Clean Architecture en el backend.

### Backend (.NET 8)
- **ClaroTurnos.Api**: Controllers, Middlewares, JWT Auth, Swagger, Hubs SignalR.
- **ClaroTurnos.Application**: Servicios de negocio, DTOs, Validadores (FluentValidation), Interfaces de repositorios, Reglas de turnos.
- **ClaroTurnos.Domain**: Entidades, Enums, Value Objects, Reglas de dominio.

### Frontend (React 18)
- Kiosco táctil con Bootstrap 5 + Axios + SignalR client.
- Pantalla pública con animaciones y voz.
- Panel de agentes con login y gestión en tiempo real.

## Base de Datos (SQL Server 2022)
**Catálogos base:**
- **Roles**: Administrador, Supervisor, Agente.
- **Sucursales**: Código único, nombre, dirección, horario apertura/cierre.
- **Servicios**: Ventas, Servicio al Cliente, Soporte Técnico, Facturación, Reclamos (prefijo A-E).
- **EstadosTicket**: Pendiente, En espera, Atendido, Cancelado.

**Entidades principales:**
- **Tickets**: Número correlativo por servicio, fecha/hora de emisión, estado actual.
- **Agentes**: Usuario, nombre, servicios asignados (ej: María Fernández → Ventas/Facturación).
- **ColaTurnos**: Orden de atención en tiempo real.

## Impresión Térmica
**Protocolo:** ESC/POS vía TCP/IP al puerto 9100 de la impresora IP configurada.
**Hardware compatible:** Epson TM-T20/T88, Bixolon SRP-350 o genéricas 58mm/80mm.
**Flujo recomendado (MVP):** El backend genera el buffer ESC/POS y lo envía directamente a la impresora, evitando instalar drivers en cada kiosco.

## Plan de Implementación MVP (~12.5 semanas)
| Fase | Contenido | Duración |
|---|---|---|
| 0. Preparación | Repos, CI/CD básico, entorno SQL Server, definición de ambientes (dev/staging) | 1 semana |
| 1. Backend base | Dominio, EF Core + migraciones, autenticación JWT, CRUD de catálogos | 2 semanas |
| 2. Motor de turnos | Generación de tickets, numeración correlativa por servicio, endpoints de cola, SignalR Hub | 1.5 semanas |
| 3. Frontend Kiosco + Pantalla Pública | UI táctil, impresión, pantalla pública con animaciones y voz | 2 semanas |
| 4. Frontend Agentes + Recepción | Login, llamar/repetir/transferir/finalizar/ausente, panel de colas en vivo | 1.5 semanas |
| 5. Administración y Reportes | CRUD administrativo, dashboard, exportación Excel/PDF | 1.5 semanas |
| 6. Impresión térmica real | Integración con impresora física, manejo de errores, pruebas en sucursal piloto | 1 semana |
| 7. QA, auditoría, hardening | Pruebas end-to-end, auditoría, logs, ajuste de SLA/roles | 1 semana |
| 8. Piloto y estabilización | Despliegue en 1 sucursal piloto, ajustes con usuarios reales | 1 semana |

## Mejoras Futuras (Fuera del MVP)
- Multi-sucursal avanzado con reportes comparativos.
- App móvil para clientes.
- Integración con locutor físico por altavoces.
- IA para predicción de demanda.