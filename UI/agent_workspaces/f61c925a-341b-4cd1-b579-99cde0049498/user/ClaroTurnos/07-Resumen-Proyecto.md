# Resumen del Proyecto: Claro Turnos

## ¿De qué trata este proyecto?

**Claro Turnos** es un sistema de gestión de filas por tickets numerados para puntos de atención al cliente de Claro. Reemplaza la fila física tradicional por un ticket impreso (o virtual) con número correlativo por tipo de servicio, visible en pantalla pública y gestionado por agentes desde paneles especializados.

## Módulos del Sistema
- **Kiosco**: Pantalla táctil donde el cliente selecciona su servicio y obtiene un ticket impreso térmico.
- **Panel de Recepción**: Gestión inicial de tickets (repetir, cancelar).
- **Panel de Agentes**: Llamada/repetición/transferencia/finalización de turnos por agente.
- **Pantalla Pública**: Visualización en tiempo real del turno actual y colas pendientes.
- **Administración**: CRUD de usuarios, servicios, prefijos y sucursales.
- **Reportes**: Atendidos por día, tiempos promedio, exportación Excel/PDF.

## Arquitectura Técnica
Arquitectura N-Tier + Clean Architecture:
1. **Frontend**: React 18 + Bootstrap 5 + Axios + SignalR client (Kiosco táctil).
2. **Backend API (.NET Core/C#)**: Controllers, Middlewares, JWT Auth, Swagger.
3. **Application Layer**: Servicios de negocio, DTOs, validadores FluentValidation, reglas de turnos.
4. **Domain Layer**: Entidades, Enums, Value Objects, Reglas de dominio (ej. numeración correlativa).
5. **Infrastructure Layer**: Persistencia SQL Server 2022, impresión térmica ESC/POS vía TCP/IP.

## Base de Datos (SQL Server)
- Catálogos: Roles, Sucursales, Servicios (con prefijos A-E), EstadosTicket.
- Tickets: Generación correlativa por servicio y sucursal, con reinicio diario configurable.
- Agentes: Asignados a servicios específicos, estado en tiempo real (Activo/Ausente).

## Impresión Térmica de Tickets
- Protocolo ESC/POS vía socket TCP al puerto 9100 de impresoras compatibles (Epson TM-T20/T88, Bixolon SRP-350).
- Plantilla del ticket: Código servicio + número correlativo, fecha/hora, personas antes en cola, sucursal.

## Alcance MVP (~12.5 semanas)
Fases:
1. Preparación (Repos/CI/CD/SQL Server) - 1 semana
2. Backend base (Dominio/EF Core/JWT/CRUD catálogos) - 2 semanas
3. Motor de turnos (Generación tickets, SignalR Hub) - 1.5 semanas
4. Frontend Kiosco + Pantalla Pública - 2 semanas
5. Frontend Agentes + Recepción - 1.5 semanas
6. Administración y Reportes - 1.5 semanas
7. Impresión térmica real (integración con impresora física) - 1 semana
8. QA, auditoría, hardening - 1 semana
9. Piloto en sucursal piloto - 1 semana

## Mejoras Futuras (Fuera del MVP)
- Multi-sucursal avanzado con reportes comparativos.
- App móvil para clientes.
- Integración con locutor físico por altavoces.
- IA para predicción de demanda.