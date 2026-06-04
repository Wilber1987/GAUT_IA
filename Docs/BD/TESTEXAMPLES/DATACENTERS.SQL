USE [GAUT_IA];
GO

-- =================================================================
-- 1. ASEGURAR CATEGORÍA "DATA CENTERS"
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Categorias_Test] WHERE [descripcion] = N'DATACENTERS')
BEGIN
    INSERT INTO [questionnaires].[Cat_Categorias_Test] ([descripcion], [estado], [created_at], [IsDeleted]) 
    VALUES (N'DATACENTERS', N'ACTIVA', GETDATE(), 0);
END
GO

-- =================================================================
-- 2. CREAR TIPOS DE PREGUNTAS CATEGÓRICAS ESPECÍFICOS PARA DC
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Nivel Tier Uptime Institute')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Nivel Tier Uptime Institute', N'ACTIVA', N'Determina la arquitectura de redundancia y el presupuesto base del proyecto', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Topología de Redundancia de Energía')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Topología de Redundancia de Energía', N'ACTIVA', N'Define la cantidad de UPS, PDUs y rutas de energía independientes a cotizar', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Climatización Preferido')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Sistema de Climatización Preferido', N'ACTIVA', N'Condiciona el tipo de equipos de precisión (CRAC/CRAH) y la eficiencia energética (PUE) a proponer', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Supresión de Incendios')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Sistema de Supresión de Incendios', N'ACTIVA', N'Determina el agente extintor a cotizar, crucial para la seguridad de los equipos electrónicos', 0);
END
GO

-- =================================================================
-- 3. CREAR VALORES PARA LOS TIPOS CATEGÓRICOS (Patrón seguro sin duplicados)
-- =================================================================
-- Valores para Nivel Tier
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Tier I (Básico, 99.671%)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Nivel Tier Uptime Institute' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Tier I (Básico, 99.671%)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Tier II (Redundancia de componentes, 99.741%)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Nivel Tier Uptime Institute' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Tier II (Redundancia de componentes, 99.741%)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Tier III (Mantenimiento concurrente, 99.982%)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Nivel Tier Uptime Institute' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Tier III (Mantenimiento concurrente, 99.982%)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Tier IV (Tolerancia a fallos, 99.995%)', [id_tipo_pregunta], N'ACTIVO', 4, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Nivel Tier Uptime Institute' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Tier IV (Tolerancia a fallos, 99.995%)');

-- Valores para Redundancia
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'N (Sin redundancia)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Topología de Redundancia de Energía' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'N (Sin redundancia)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'N+1 (Un módulo de respaldo)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Topología de Redundancia de Energía' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'N+1 (Un módulo de respaldo)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'2N (Duplicación completa de sistemas)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Topología de Redundancia de Energía' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'2N (Duplicación completa de sistemas)');

-- Valores para Climatización
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Precisión Tipo Sala (CRAC/CRAH)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Climatización Preferido' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Precisión Tipo Sala (CRAC/CRAH)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'In-Row (Refrigeración cercana al rack)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Climatización Preferido' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'In-Row (Refrigeración cercana al rack)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Contención de Pasillos (Frío/Caliente)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Climatización Preferido' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Contención de Pasillos (Frío/Caliente)');

-- Valores para Supresión de Incendios
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'FM-200 (Gas limpio)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Supresión de Incendios' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'FM-200 (Gas limpio)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Novec 1230 (Sostenible y seguro)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Supresión de Incendios' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Novec 1230 (Sostenible y seguro)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Agua Nebulizada (Water Mist)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Supresión de Incendios' AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Agua Nebulizada (Water Mist)');
GO

-- =================================================================
-- 4. CREAR EL FORMULARIO (TEST) CON PROMPT DE IA ESPECIALIZADO
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Tests] WHERE [titulo] = N'COTIZACIÓN DE PROYECTO DE DATACENTER')
BEGIN
    INSERT INTO [questionnaires].[Tests] (
        [titulo], [descripcion], [tipo_test], [estado], [id_categoria], 
        [fecha_publicacion], [created_at], [tiempo], [caducidad], [color], [IsDeleted]
    ) 
    SELECT 
        N'COTIZACIÓN DE PROYECTO DE DATACENTER', 
        N'Actúa como un Ingeniero de Ventas Pre-Sales Senior especializado en Infraestructura Crítica y Data Centers. 
        Tu tarea es redactar una "Propuesta Técnica y Económica Preliminar" profesional, persuasiva y detallada. 
        Analiza los datos recolectados y aplica las reglas de contexto de cada pregunta. 
        Estructura obligatoria del documento:
        1. Resumen Ejecutivo y Entendimiento del Requerimiento.
        2. Arquitectura Propuesta (Desglose por Energía, Climatización, Racks y Seguridad).
        3. Justificación Técnica (Explica POR QUÉ se eligieron estos equipos basándote en el Tier y la carga solicitada).
        4. Consideraciones de Implementación y Plazos.
        5. Próximos Pasos Recomendados (ej. visita técnica, levantamiento detallado).
        Usa un tono corporativo, técnico y orientado a la continuidad del negocio del cliente.', 
        N'GENERACION_IA', N'ACTIVO', [Id_categoria], 
        GETDATE(), GETDATE(), 25, 365, N'#1A237E', 0
    FROM [questionnaires].[Cat_Categorias_Test]
    WHERE [descripcion] = N'DATA CENTERS';
END
GO

-- =================================================================
-- 5. CREAR LAS PREGUNTAS DEL FORMULARIO (Autocontenidas)
-- =================================================================
DECLARE @TestID_DC INT = (SELECT [id_test] FROM [questionnaires].[Tests] WHERE [titulo] = N'COTIZACIÓN DE PROYECTO DE DATACENTER');

IF @TestID_DC IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Pregunta_Tests] WHERE [id_test] = @TestID_DC AND [seccion] = N'DATOS DEL PROYECTO')
BEGIN
    -- SECCIÓN 1: DATOS DEL PROYECTO
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Nombre de la empresa cliente y persona de contacto', @TestID_DC, 1, GETDATE(), GETDATE(), N'DATOS DEL PROYECTO', N'La IA usará esto para personalizar el encabezado y el tono de la propuesta.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Superficie total disponible para la sala de servidores (en metros cuadrados)', @TestID_DC, 2, GETDATE(), GETDATE(), N'DATOS DEL PROYECTO', N'La IA evaluará si el espacio es suficiente para la cantidad de racks solicitada y el flujo de aire requerido.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Cantidad estimada de Racks a instalar', @TestID_DC, 2, GETDATE(), GETDATE(), N'DATOS DEL PROYECTO', N'Dato fundamental para dimensionar la distribución de energía (PDUs) y la capacidad de refrigeración total.', 0;

    -- SECCIÓN 2: DISPONIBILIDAD Y ENERGÍA
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Cuál es el Nivel Tier (Uptime Institute) requerido o esperado?', @TestID_DC, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Nivel Tier Uptime Institute'), 
           GETDATE(), GETDATE(), N'DISPONIBILIDAD Y ENERGIA', N'Si es Tier III o IV, la IA debe incluir en la propuesta sistemas de conmutación estática (STS) y justificar el mayor costo por la alta disponibilidad.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Topología de redundancia de energía deseada', @TestID_DC, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Topología de Redundancia de Energía'), 
           GETDATE(), GETDATE(), N'DISPONIBILIDAD Y ENERGIA', N'Define si se cotiza un solo camino de energía (N) o caminos duales completos (2N) hasta el rack.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Carga IT total estimada (en kW)', @TestID_DC, 2, GETDATE(), GETDATE(), N'DISPONIBILIDAD Y ENERGIA', N'El dato más crítico. La IA lo usará para calcular el tamaño del UPS, el generador diésel y la capacidad de los CRACs.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Autonomía requerida del UPS sin planta eléctrica (en minutos)', @TestID_DC, 2, GETDATE(), GETDATE(), N'DISPONIBILIDAD Y ENERGIA', N'Si es >15 minutos, la IA recomendará bancos de baterías de larga duración o enfatizará la necesidad urgente de un generador diésel.', 0;

    -- SECCIÓN 3: CLIMATIZACIÓN Y AMBIENTE
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Densidad de potencia promedio por Rack (en kW/rack)', @TestID_DC, 2, GETDATE(), GETDATE(), N'CLIMATIZACION Y AMBIENTE', N'Si es >5kW/rack, la IA debe recomendar obligatoriamente soluciones In-Row o Liquid Cooling en lugar de CRACs tradicionales de sala.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Sistema de climatización preferido o actual', @TestID_DC, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Climatización Preferido'), 
           GETDATE(), GETDATE(), N'CLIMATIZACION Y AMBIENTE', N'La IA ajustará la propuesta técnica para alinearse con esta preferencia, destacando sus beneficios de eficiencia (PUE).', 0;

    -- SECCIÓN 4: SEGURIDAD FÍSICA Y PROTECCIÓN
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Sistema de supresión de incendios requerido', @TestID_DC, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Supresión de Incendios'), 
           GETDATE(), GETDATE(), N'SEGURIDAD FISICA Y PROTECCION', N'La IA detallará las ventajas del agente seleccionado (ej. Novec 1230 por su bajo impacto ambiental) en la sección de Justificación Técnica.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Se requiere control de acceso biométrico y sistema de CCTV independiente?', @TestID_DC, 3, GETDATE(), GETDATE(), N'SEGURIDAD FISICA Y PROTECCION', N'Si es SÍ, la IA añadirá una partida presupuestaria estimada para control de acceso de alta seguridad y videovigilancia con retención de 90 días.', 0;

    -- SECCIÓN 5: CONECTIVIDAD Y PLAZOS
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Se requiere neutralidad de operadores (múltiples entradas de fibra)?', @TestID_DC, 3, GETDATE(), GETDATE(), N'CONECTIVIDAD Y PLAZOS', N'Si es SÍ, la IA incluirá recomendaciones sobre Meet-Me Rooms (MMR) y gestión de cableado estructurado de alta densidad.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Fecha estimada de puesta en marcha (Go-Live) y observaciones adicionales', @TestID_DC, 1, GETDATE(), GETDATE(), N'CONECTIVIDAD Y PLAZOS', N'La IA usará la fecha para evaluar la viabilidad del cronograma y añadirá un sentido de urgencia o planificación de fases si el plazo es ajustado.', 0;

    PRINT '✅ Formulario de COTIZACIÓN DE DATACENTER creado exitosamente con todas sus preguntas y valores.';
END
ELSE
BEGIN
    PRINT 'ℹ️ El formulario de COTIZACIÓN DE DATACENTER ya existe o no se pudo encontrar. No se realizaron cambios.';
END
GO