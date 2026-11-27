# Resumen del Proyecto Claro Turnos

## Visión General
Sistema de gestión de filas por tickets numerados para puntos de atención de Claro. Reemplaza la fila física por un ticket impreso (o virtual) con número correlativo por tipo de servicio, visible en pantalla pública y gestionado por agentes desde un panel de atención.

## Arquitectura Técnica
- **Estilo**: N-Tier + Clean Architecture en el backend.
- **Backend**: C# (.NET), SQL Server 2022. Separación estricta de responsabilidades (Domain, Application, Infrastructure).
- **Frontend**: React 18 con Bootstrap 5, Axios y SignalR client para comunicación real-time.

## Módulos del Sistema
1. Kiosco (touch)
2. Panel de Recepción
3. Panel de Agentes
4. Pantalla Pública
5. Panel Administrativo
6. Reportes

## Alcance del MVP (~12.5 semanas / 3 meses)
- **Incluye**: Generación y numeración correlativa por servicio, SignalR Hub para colas en vivo, impresión térmica vía TCP/IP (ESC/POS) a impresoras IP configuradas, CRUD de catálogos básicos, dashboard y exportación Excel/PDF.
- **Fuera de alcance del MVP**: Multi-sucursal avanzado con reportes comparativos, app móvil para clientes, integración con locutor físico por altavoces e IA para predicción de demanda.

## Modelo de Base de Datos (SQL Server 2022)
- Catálogos base: Roles, Sucursales, Servicios (identificados por prefijos A-E), EstadosTicket.
- Entidades principales: Tickets, Agentes/Personal, Historial de atención.

## Impresión Térmica
Durante el MVP se recomienda que el backend genere el buffer ESC/POS y lo envíe directamente por socket TCP al puerto 9100 de la impresora IP configurada en `ImpresorasTermicas`, evitando instalar drivers en cada kiosco.