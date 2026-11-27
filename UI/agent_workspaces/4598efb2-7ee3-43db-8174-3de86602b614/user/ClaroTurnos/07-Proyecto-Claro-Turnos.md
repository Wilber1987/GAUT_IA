# Proyecto: Sistema de Gestión de Turnos — CLARO

## Descripción General
El **Sistema de Gestión de Turnos Claro** es una solución tecnológica diseñada para modernizar y optimizar la atención al cliente en los puntos de venta y servicio de Claro. Su objetivo principal es reemplazar las filas físicas tradicionales por un sistema digital basado en tickets numerados, mejorando la experiencia del usuario y la eficiencia operativa.

## ¿De qué trata?
El sistema permite a los clientes obtener un ticket virtual o impreso al llegar a una sucursal, indicando el tipo de servicio que necesitan (Ventas, Servicio al Cliente, Soporte Técnico, Facturación, Reclamos). El ticket muestra su número correlativo y la cantidad de personas antes de ellos en la cola.

Una vez atendidos, los agentes pueden gestionar las colas desde un panel dedicado: llamar el siguiente turno, repetir tickets, transferirlos o marcar ausencias. La información se actualiza en tiempo real tanto para los clientes (en pantallas públicas) como para el personal interno.

## Módulos del Sistema
1. **Kiosco:** Interfaz táctil donde el cliente selecciona su servicio y obtiene un ticket impreso térmico.
2. **Panel de Recepción:** Permite a la recepción gestionar tickets, imprimir duplicados o cancelar turnos.
3. **Panel de Agentes:** Herramienta para que los agentes atiendan, repitan o transfieran tickets en tiempo real.
4. **Pantalla Pública:** Muestra el ticket actual y las colas pendientes por servicio con animaciones visuales.
5. **Administración:** Gestión de usuarios, sucursales, servicios, prefijos de turnos e impresoras térmicas.
6. **Reportes:** Estadísticas sobre tiempos promedio de atención, tickets atendidos por día y exportación a Excel/PDF.

## Arquitectura Técnica (MVP)
- **Frontend:** React 18 + Bootstrap 5 + Axios + SignalR Client para la interfaz táctil y pantallas públicas.
- **Backend (.NET Core):**
    - `ClaroTurnos.Api`: Controladores REST, autenticación JWT y Hubs de SignalR.
    - `ClaroTurnos.Application`: Servicios de negocio, DTOs y validaciones (FluentValidation).
    - `ClaroTurnos.Domain`: Entidades, enums, Value Objects y reglas de dominio.
- **Base de Datos:** SQL Server 2022 con Entity Framework Core para la persistencia de datos.
- **Impresión Térmica:** Integración directa del backend (C#) a impresoras ESC/POS vía socket TCP/IP al puerto 9100, evitando drivers locales en los kioscos.

## Beneficios Clave
- Reducción de tiempos de espera y quejas por filas desordenadas.
- Visibilidad en tiempo real para el cliente sobre su posición en la cola.
- Control centralizado de turnos y métricas operativas para la gerencia.
- Escalabilidad futura hacia multi-sucursal, app móvil y predicción de demanda con IA.