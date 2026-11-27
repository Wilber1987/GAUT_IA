# Plan de implementación MVP, estimación y mejoras futuras

## 1. Alcance del MVP

Incluye: Kiosco, Panel de Recepción, Panel de Agentes, Pantalla Pública, Administración básica (usuarios, servicios, prefijos, sucursal única) y Reportes esenciales (atendidos por día, tiempos promedio, exportación Excel/PDF). Impresión térmica vía TCP/IP a una impresora por kiosco.

Fuera de alcance del MVP (ver sección de mejoras futuras): multi-sucursal avanzado con reportes comparativos, app móvil para clientes, integración con locutor físico por altavoces, IA para predicción de demanda.

## 2. Plan de implementación por fases

| Fase | Contenido | Duración estimada |
|---|---|---|
| 0. Preparación | Repos, CI/CD básico, entorno SQL Server, definición de ambientes (dev/staging) | 1 semana |
| 1. Backend base | Dominio, EF Core + migraciones, autenticación JWT, CRUD de catálogos (Sucursales, Servicios, Usuarios, Ventanillas) | 2 semanas |
| 2. Motor de turnos | Generación de tickets, numeración correlativa por servicio, endpoints de cola, SignalR Hub | 1.5 semanas |
| 3. Frontend Kiosco + Pantalla Pública | UI táctil, impresión, pantalla pública con animaciones y voz | 2 semanas |
| 4. Frontend Agentes + Recepción | Login, llamar/repetir/transferir/finalizar/ausente, panel de colas en vivo | 1.5 semanas |
| 5. Administración y Reportes | CRUD administrativo, dashboard, exportación Excel/PDF | 1.5 semanas |
| 6. Impresión térmica real | Integración con impresora física, manejo de errores, pruebas en sucursal piloto | 1 semana |
| 7. QA, auditoría, hardening | Pruebas end-to-end, auditoría, logs, ajuste de SLA/roles | 1 semana |
| 8. Piloto y estabilización | Despliegue en 1 sucursal piloto, ajustes con usuarios reales | 1 semana |

**Total estimado: ~12.5 semanas (≈ 3 meses) para el MVP completo con 1 sucursal piloto.**

## 3. Estimación de recursos

| Rol | Cantidad | Dedicación |
|---|---|---|
| Arquitecto / Tech Lead | 1 | Parcial (supervisión + decisiones clave) |
| Desarrollador Backend (.NET) | 1–2 | Tiempo completo |
| Desarrollador Frontend (React) | 1–2 | Tiempo completo |
| QA / Tester | 1 | Desde fase 3 |
| Analista de Negocio / PO | 1 | Parcial |
| DevOps | 1 | Parcial (fases 0, 6–8) |

**Equipo sugerido para cumplir el cronograma: 4–5 personas.**

## 4. Riesgos principales

- Disponibilidad e integración real de impresoras térmicas (variabilidad de modelos/drivers).
- Conectividad de red en sucursales para SignalR en tiempo real (mitigar con reconexión automática y polling de respaldo).
- Adopción por parte de agentes acostumbrados a procesos manuales (plan de capacitación en fase 8).

## 5. Mejoras futuras (post-MVP)

1. **Multi-sucursal a escala**: selector de sucursal en todos los paneles, reportes comparativos entre sucursales, franquicias.
2. **App móvil para clientes**: reservar turno virtual desde el celular antes de llegar a la sucursal, con notificación push cuando falten N turnos.
3. **Integración con WhatsApp Business**: notificación del turno y tiempo estimado de espera.
4. **Predicción de demanda con IA**: modelos de series de tiempo para sugerir dotación de agentes por franja horaria.
5. **Encuesta de satisfacción post-atención** (NPS) vinculada al ticket.
6. **Locutor físico / altavoces IP** integrados al evento `TicketLlamado` además de la síntesis de voz en pantalla.
7. **Modo offline del kiosco** con sincronización diferida ante cortes de red.
8. **Panel de KPIs en tiempo real** (SLA cumplido/incumplido) con alertas proactivas a supervisores.
9. **Autenticación biométrica o tarjeta de empleado** para agentes en lugar de usuario/contraseña.
10. **Internacionalización** (multi-idioma) para sucursales fronterizas o turísticas.
