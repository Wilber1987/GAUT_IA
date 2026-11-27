# Resumen del Proyecto — Claro Turnos

## Visión General
Sistema de gestión de filas por tickets numerados para puntos de atención de Claro, que reemplaza la fila física por un ticket impreso (o virtual) con número correlativo por tipo de servicio. El sistema es visible en pantalla pública y gestionado por agentes desde paneles especializados.

## Módulos del Sistema
- **Kiosco**: Interfaz táctil para generación de tickets físicos/virtuales e impresión térmica.
- **Panel de Recepción**: Gestión inicial de turnos, atención prioritaria y administración básica.
- **Panel de Agentes**: Login seguro, llamada/repetir/transferir/finalizar/ausente, panel de colas en vivo.
- **Pantalla Pública**: Visualización animada con voz del ticket actual por servicio.
- **Administración**: CRUD de usuarios, servicios, prefijos y sucursales.
- **Reportes**: Atendidos por día, tiempos promedio, exportación Excel/PDF.

## Arquitectura Técnica
Arquitectura N-Tier + Clean Architecture en el backend:
- **Frontend**: React 18 + Bootstrap 5 + Axios + SignalR client (Kiosco, Recepción, Agentes, Pantalla Pública).
- **Backend (.NET)**: ClaroTurnos.Api (Controllers, Middlewares, JWT Auth, Swagger), ClaroTurnos.Application (Servicios de negocio, DTOs, Validadores FluentValidation, Reglas de turnos), ClaroTurnos.Domain (Entidades, Enums, Value Objects, Reglas de dominio).
- **Base de Datos**: SQL Server 2022 con modelo relacional que incluye tablas para Roles, Sucursales, Servicios, EstadosTicket, Tickets, Agentes, etc.

## Impresión Térmica
Implementación vía ESC/POS por socket TCP al puerto 9100 de impresoras IP (Epson TM-T20/T88, Bixolon SRP-350). El backend genera el buffer y lo envía directamente a la impresora configurada en `ImpresorasTermicas`, evitando drivers locales.

## Plan MVP (~12.5 semanas)
| Fase | Contenido |
|---|---|
| 0 | Preparación (Repos, CI/CD, SQL Server) — 1 semana |
| 1 | Backend base (Dominio, EF Core, JWT Auth, CRUD catálogos) — 2 semanas |
| 2 | Motor de turnos (Tickets, numeración correlativa, SignalR Hub) — 1.5 semanas |
| 3 | Frontend Kiosco + Pantalla Pública — 2 semanas |
| 4 | Frontend Agentes + Recepción — 1.5 semanas |
| 5 | Administración y Reportes — 1.5 semanas |
| 6 | Impresión térmica real (integración, pruebas) — 1 semana |
| 7 | QA, auditoría, hardening — 1 semana |
| 8 | Piloto y estabilización — 1 semana |

## Mejoras Futuras
- Multi-sucursal avanzado con reportes comparativos.
- App móvil para clientes.
- Integración con locutor físico por altavoces.
- IA para predicción de demanda.