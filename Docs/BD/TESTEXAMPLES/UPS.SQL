USE [GAUT_IA];
GO

-- =================================================================
-- 1. ASEGURAR CATEGORÍA "DATA CENTERS" (Por si se ejecuta en una BD nueva)
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Categorias_Test] WHERE [descripcion] = N'DATA CENTERS')
BEGIN
    INSERT INTO [questionnaires].[Cat_Categorias_Test] ([descripcion], [estado], [created_at], [IsDeleted]) 
    VALUES (N'DATA CENTERS', N'ACTIVA', GETDATE(), 0);
END
GO

-- =================================================================
-- 2. CREAR TIPOS DE PREGUNTAS CATEGÓRICAS ESPECÍFICOS PARA ENERGÍA/UPS
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Topología del Sistema UPS')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Topología del Sistema UPS', N'ACTIVA', N'Determina la calidad de la energía de salida y la protección contra microcortes', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tecnología de Baterías Preferida')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Tecnología de Baterías Preferida', N'ACTIVA', N'Impacta directamente en el peso, espacio, vida útil (ciclos) y costo total de propiedad (TCO)', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Configuración de Fases')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Configuración de Fases', N'ACTIVA', N'Define si la entrada y salida del UPS son monofásicas o trifásicas, crucial para el dimensionamiento del cableado', 0);
END
GO

-- =================================================================
-- 3. CREAR VALORES PARA LOS TIPOS CATEGÓRICOS DE ENERGÍA
-- =================================================================
-- Valores para Topología UPS
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Online Doble Conversión (Protección total)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Topología del Sistema UPS' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Online Doble Conversión (Protección total)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Line-Interactive (AVR, para cargas no críticas)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Topología del Sistema UPS' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Line-Interactive (AVR, para cargas no críticas)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Módular Escalable (Hot-Swap)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Topología del Sistema UPS' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Módular Escalable (Hot-Swap)');

-- Valores para Tecnología de Baterías
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'VRLA (Plomo-Ácido Sellado, estándar)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tecnología de Baterías Preferida' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'VRLA (Plomo-Ácido Sellado, estándar)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Litio-Ion (Li-Ion, mayor vida útil y menor peso)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tecnología de Baterías Preferida' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Litio-Ion (Li-Ion, mayor vida útil y menor peso)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Ni-Cd (Níquel-Cadmio, para temperaturas extremas)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tecnología de Baterías Preferida' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Ni-Cd (Níquel-Cadmio, para temperaturas extremas)');

-- Valores para Configuración de Fases
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Monofásico / Monofásico (1/1)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Configuración de Fases' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Monofásico / Monofásico (1/1)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Trifásico / Monofásico (3/1)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Configuración de Fases' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Trifásico / Monofásico (3/1)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Trifásico / Trifásico (3/3)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Configuración de Fases' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Trifásico / Trifásico (3/3)');
GO

-- =================================================================
-- 4. CREAR EL FORMULARIO (TEST) CON PROMPT DE IA PARA ENERGÍA/UPS
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Tests] WHERE [titulo] = N'COTIZACIÓN E IMPLEMENTACIÓN DE UPS Y RESPALDO ELÉCTRICO')
BEGIN
    INSERT INTO [questionnaires].[Tests] (
        [titulo], [descripcion], [tipo_test], [estado], [id_categoria], 
        [fecha_publicacion], [created_at], [tiempo], [caducidad], [color], [IsDeleted]
    ) 
    SELECT 
        N'COTIZACIÓN E IMPLEMENTACIÓN DE UPS Y RESPALDO ELÉCTRICO', 
        N'Actúa como un Ingeniero Eléctrico Senior especializado en Continuidad de Negocio y Sistemas de Energía Ininterrumpida (UPS) para entornos TI. 
        Tu tarea es redactar una "Propuesta Técnica y Económica para Respaldo de Energía" detallada y profesional. 
        Analiza los datos recolectados y aplica las reglas de contexto de cada pregunta. 
        Estructura obligatoria del documento:
        1. Resumen Ejecutivo y Análisis de la Carga Crítica.
        2. Arquitectura de Energía Propuesta (Dimensionamiento del UPS, Topología y Configuración de Fases).
        3. Cálculo de Autonomía y Justificación de la Tecnología de Baterías (comparativa breve de TCO si aplica).
        4. Integración con Infraestructura Existente (Bypass de mantenimiento, coordinación con Planta Eléctrica/Generador).
        5. Monitoreo, Gestión Remota y Próximos Pasos para la implementación.
        Usa un tono técnico, enfocado en la mitigación de riesgos, la eficiencia energética y la protección de la inversión del cliente.', 
        N'GENERACION_IA', N'ACTIVO', [Id_categoria], 
        GETDATE(), GETDATE(), 20, 365, N'#FF8F00', 0
    FROM [questionnaires].[Cat_Categorias_Test]
    WHERE [descripcion] = N'DATA CENTERS';
END
GO

-- =================================================================
-- 5. CREAR LAS PREGUNTAS DEL FORMULARIO (Autocontenidas)
-- =================================================================
DECLARE @TestID_UPS INT = (SELECT [id_test] FROM [questionnaires].[Tests] WHERE [titulo] = N'COTIZACIÓN E IMPLEMENTACIÓN DE UPS Y RESPALDO ELÉCTRICO');

IF @TestID_UPS IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Pregunta_Tests] WHERE [id_test] = @TestID_UPS AND [seccion] = N'DATOS DEL ENTORNO')
BEGIN
    -- SECCIÓN 1: DATOS DEL ENTORNO Y CARGA
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Nombre de la empresa y ubicación de la instalación', @TestID_UPS, 1, GETDATE(), GETDATE(), N'DATOS DEL ENTORNO', N'La IA usará esto para personalizar la propuesta y considerar factores ambientales locales si se mencionan.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Tipo de entorno a proteger (ej. Sala de servidores, Rack distribuido, Área industrial con TI)', @TestID_UPS, 1, GETDATE(), GETDATE(), N'DATOS DEL ENTORNO', N'La IA ajustará las recomendaciones de robustez y protección (IP) según si es un entorno limpio o industrial.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Carga total actual a respaldar (especificar en kW o kVA)', @TestID_UPS, 1, GETDATE(), GETDATE(), N'DATOS DEL ENTORNO', N'Dato fundamental. La IA aplicará un factor de seguridad (ej. 20-30% de margen) sobre este valor para recomendar la capacidad nominal del UPS.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Porcentaje estimado de crecimiento de la carga a 3-5 años (%)', @TestID_UPS, 2, GETDATE(), GETDATE(), N'DATOS DEL ENTORNO', N'Si es >20%, la IA recomendará fuertemente una topología UPS Modular Escalable para evitar reemplazos futuros.', 0;

    -- SECCIÓN 2: DIMENSIONAMIENTO Y TOPOLOGÍA
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Configuración de fases de la instalación eléctrica', @TestID_UPS, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Configuración de Fases'), 
           GETDATE(), GETDATE(), N'DIMENSIONAMIENTO Y TOPOLOGIA', N'Determina si se debe cotizar un equipo monofásico (hasta ~10kVA) o trifásico (mayor potencia y balanceo de cargas).', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Topología del sistema UPS requerida', @TestID_UPS, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Topología del Sistema UPS'), 
           GETDATE(), GETDATE(), N'DIMENSIONAMIENTO Y TOPOLOGIA', N'Si eligen "Online Doble Conversión", la IA destacará la protección total contra armónicos y microcortes en la justificación técnica.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Se requiere redundancia en el sistema de energía (ej. N+1, 2N)?', @TestID_UPS, 3, GETDATE(), GETDATE(), N'DIMENSIONAMIENTO Y TOPOLOGIA', N'Si es SÍ, la IA duplicará o modularizará la propuesta y explicará el impacto en la disponibilidad (ej. 99.999%).', 0;

    -- SECCIÓN 3: AUTONOMÍA Y BATERÍAS
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Tiempo de autonomía requerido sin energía de red (en minutos)', @TestID_UPS, 2, GETDATE(), GETDATE(), N'AUTONOMIA Y BATERIAS', N'La IA calculará la necesidad de gabinetes de baterías extendidas. Si es >15 min, enfatizará la necesidad de un generador diésel complementario.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Tecnología de baterías preferida o a evaluar', @TestID_UPS, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tecnología de Baterías Preferida'), 
           GETDATE(), GETDATE(), N'AUTONOMIA Y BATERIAS', N'Si es Litio-Ion, la IA justificará el mayor CAPEX inicial con el menor OPEX, mayor vida útil (10-15 años) y ahorro de espacio/peso.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Temperatura promedio de la sala donde se ubicarán las baterías (°C)', @TestID_UPS, 2, GETDATE(), GETDATE(), N'AUTONOMIA Y BATERIAS', N'Crítico: Si es >25°C, la IA advertirá que la vida útil de las baterías VRLA se reduce a la mitad y recomendará Ni-Cd o Litio, o mejorar la climatización.', 0;

    -- SECCIÓN 4: INTEGRACIÓN Y SEGURIDAD
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Existe planta eléctrica (generador) en el sitio?', @TestID_UPS, 3, GETDATE(), GETDATE(), N'INTEGRACION Y SEGURIDAD', N'Si es SÍ, la IA incluirá en la propuesta la coordinación de arranque del generador y la función de "retorno a red" del UPS.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Se requiere Bypass de Mantenimiento (Manual o Estático)?', @TestID_UPS, 3, GETDATE(), GETDATE(), N'INTEGRACION Y SEGURIDAD', N'Elemento de seguridad crítica. La IA lo incluirá como estándar o opcional, explicando que permite el mantenimiento del UPS sin apagar la carga.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Requerimientos de monitoreo (ej. Tarjeta de red SNMP, Software de apagado graceful, App móvil)', @TestID_UPS, 1, GETDATE(), GETDATE(), N'INTEGRACION Y SEGURIDAD', N'La IA detallará las opciones de gestión remota, vitales para entornos no atendidos (unmanned sites).', 0;

    -- SECCIÓN 5: OBSERVACIONES
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Observaciones adicionales, restricciones de espacio/peso o plazos de ejecución', @TestID_UPS, 1, GETDATE(), GETDATE(), N'OBSERVACIONES', N'La IA usará esto para afinar la propuesta (ej. si hay restricción de peso, reforzará la recomendación de baterías de Litio).', 0;

    PRINT '✅ Formulario de COTIZACIÓN DE UPS Y RESPALDO ELÉCTRICO creado exitosamente.';
END
ELSE
BEGIN
    PRINT 'ℹ️ El formulario de COTIZACIÓN DE UPS Y RESPALDO ELÉCTRICO ya existe. No se realizaron cambios.';
END
GO