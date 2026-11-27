# Resumen del Proyecto Claro Turnos

## Visión General
**Claro Turnos** es un sistema de gestión de filas y atención al cliente diseñado para los puntos de servicio de la empresa **Claro**. Su objetivo principal es reemplazar las largas filas físicas por tickets digitales (impresos o virtuales) numerados correlativamente, optimizando el flujo de clientes y mejorando la experiencia del usuario.

## Módulos Principales
El sistema se divide en cinco módulos clave:
1. **Kiosco**: Permite a los clientes solicitar un ticket físico mediante impresión térmica (ESC/POS).
2. **Panel de Recepción**: Gestión inicial de tickets y atención al cliente que llega sin turno.
3. **Panel de Agentes**: Interfaz para el personal de Claro, donde pueden llamar turnos, transferirlos o marcar su estado como ausente.
4. **Pantalla Pública**: Muestra en tiempo real los números de turno llamados y los pendientes por servicio (Ventas, Soporte, etc.).
5. **Administración y Reportes**: Gestión de usuarios, sucursales, servicios y generación de reportes estadísticos (tiempos promedio, tickets atendidos).

## Arquitectura Técnica
- **Backend (.NET Core / C#)**: Sigue una arquitectura limpia por capas (Domain, Application, Infrastructure) con Entity Framework Core para la persistencia en SQL Server.
- **Frontend**: Desarrollado en React 18 + Bootstrap 5. Utiliza SignalR para las actualizaciones de estado en tiempo real entre el kiosco y la pantalla pública.
- **Base de Datos (SQL Server)**: Almacena catálogos de servicios, sucursales, usuarios/agentes y el historial de tickets con sus estados (pendiente, atendido, finalizado).

## Funcionalidades Clave
- **Numeración Correlativa**: Cada tipo de servicio tiene su propia secuencia numérica independiente.
- **Impresión Térmica**: Integración directa del backend con impresoras térmicas vía TCP/IP para generar tickets físicos sin necesidad de drivers locales en el kiosco.
- **Gestión de SLA**: Definición de tiempos máximos de atención por servicio (SLA) y alertas si se exceden.
- **Multi-servicio**: Soporte para diferentes tipos de atención dentro de una misma sucursal (Ventas, Servicio al Cliente, Reclamos, etc.).

## Estado Actual del Proyecto
Se encuentra en la fase de definición del MVP (Producto Mínimo Viable), con un estimado de 12.5 semanas para su implementación completa.