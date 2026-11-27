# Resumen del Proyecto: Claro Turnos

## Visión General
Sistema de gestión de filas por tickets numerados para puntos de atención de Claro. Reemplaza la fila física por un ticket impreso (o virtual) con número correlativo por tipo de servicio, visible en pantalla pública y gestionado por agentes desde un panel de atención.

## Módulos Principales
- **Kiosco:** Interfaz táctil para que los clientes generen tickets.
- **Panel de Recepción:** Gestión inicial de la cola.
- **Panel de Agentes:** Llamada, repetición y finalización de turnos.
- **Pantalla Pública:** Visualización en tiempo real del estado de las colas.
- **Administración y Reportes:** CRUD administrativo, dashboard y exportación Excel/PDF.

## Arquitectura Técnica
- **Estilo:** N-Tier + Clean Architecture (Backend).
- **Frontend:** React 18 + Bootstrap 5 + Axios + SignalR client.
- **Backend:** C# con EF Core, migraciones, autenticación JWT y hubs de SignalR para comunicación en tiempo real.

## Impresión Térmica
- Integración directa del backend (C#) a la impresora térmica vía TCP/IP (puerto 9100).
- Evita instalar drivers locales; el backend genera el buffer ESC/POS y lo envía directamente.
- Ticket incluye: Logo Claro, código de servicio (ej. A - 001), fecha/hora, personas antes del cliente y sucursal.

## Base de Datos
- **Motor:** SQL Server 2022.
- **Entidades clave:** Roles, Sucursales, Servicios (con prefijos correlativos: A, B, C...), EstadosTicket, Tickets, Agentes.

## Plan MVP y Estimación (~12.5 semanas)
| Fase | Contenido |
|---|---|
| 0. Preparación | Repositorio, CI/CD básico, entorno SQL Server (1 semana) |
| 1. Backend base | Dominio, EF Core + migraciones, autenticación JWT, CRUD de catálogos (2 semanas) |
| 2. Motor de turnos | Generación de tickets, numeración correlativa, endpoints de cola, SignalR Hub (1.5 semanas) |
| 3. Frontend Kiosco + Pantalla Pública | UI táctil, impresión, pantalla pública con animaciones y voz (2 semanas) |
| 4. Frontend Agentes + Recepción | Login, llamar/repetir/transferir/finalizar/ausente, panel de colas en vivo (1.5 semanas) |
| 5. Administración y Reportes | CRUD administrativo, dashboard, exportación Excel/PDF (1.5 semanas) |
| 6. Impresión térmica real | Integración con impresora física, manejo de errores, pruebas piloto (1 semana) |
| 7. QA, auditoría, hardening | Pruebas end-to-end, logs, ajuste SLA/roles (1 semana) |
| 8. Piloto y estabilización | Despliegue en sucursal piloto, ajustes con usuarios reales (1 semana) |

## Mejoras Futuras (Fuera del alcance MVP)
- Multi-sucursal avanzado con reportes comparativos.
- App móvil para clientes.
- Integración con locutor físico por altavoces.
- IA para predicción de demanda.