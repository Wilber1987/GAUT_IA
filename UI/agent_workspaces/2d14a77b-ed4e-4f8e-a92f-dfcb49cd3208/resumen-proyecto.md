# Resumen del Proyecto — Claro Turnos

## Visión General
Sistema de gestión de filas por tickets numerados para puntos de atención de Claro, que reemplaza la fila física por un ticket impreso (o virtual) con número correlativo por tipo de servicio, visible en pantalla pública y gestionado por agentes desde un panel de atención.

## Módulos del Sistema
- **Kiosco:** Interfaz táctil para emisión de tickets e impresión térmica.
- **Panel de Recepción:** Gestión inicial de la cola y supervisión.
- **Panel de Agentes:** Login, llamar/repetir/transferir/finalizar turnos, marcar ausente.
- **Pantalla Pública:** Visualización en tiempo real del turno actual con animaciones y voz.
- **Administración:** CRUD de usuarios, servicios, prefijos y sucursales.
- **Reportes:** Atendidos por día, tiempos promedio, exportación Excel/PDF.

## Arquitectura Técnica
- **Estilo:** N-Tier + Clean Architecture en el backend.
- **Backend (ClaroTurnos.Api):** .NET Core con EF Core, SQL Server 2022, SignalR para WebSockets y JWT Auth.
- **Frontend:** React 18 + Bootstrap 5 + Axios + SignalR client.

## Alcance del MVP (~12.5 semanas)
Incluye la implementación completa de los módulos listados arriba e integración con impresoras térmicas vía TCP/IP (generación de buffer ESC/POS desde el backend). La plantilla del ticket incluye: logo centrado, número correlativo por servicio (ej. A-001), fecha/hora, personas antes en cola y sucursal.
**Fuera de alcance:** Multi-sucursal avanzado, app móvil para clientes, locutor físico y predicción de demanda por IA.