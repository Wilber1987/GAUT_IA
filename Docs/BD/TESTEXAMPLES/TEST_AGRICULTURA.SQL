USE [GAUT_IA];
GO

-- =================================================================
-- 1. ASEGURAR CATEGORÍA AGRICULTURA
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Categorias_Test] WHERE [descripcion] = N'AGRICULTURA')
BEGIN
    INSERT INTO [questionnaires].[Cat_Categorias_Test] ([descripcion], [estado], [created_at], [IsDeleted]) 
    VALUES (N'AGRICULTURA', N'ACTIVA', GETDATE(), 0);
END
GO

-- =================================================================
-- 2. CREAR TIPOS DE PREGUNTAS CATEGÓRICAS (Si no existen)
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Condición de Humedad del Suelo')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Condición de Humedad del Suelo', N'ACTIVA', N'Determina la viabilidad inmediata de la nascencia y riesgo de encostramiento', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Textura Predominante del Suelo')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Textura Predominante del Suelo', N'ACTIVA', N'Define la capacidad de retención de agua y la profundidad de siembra recomendada', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tecnología de Maquinaria de Siembra')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Tecnología de Maquinaria de Siembra', N'ACTIVA', N'Evalúa la precisión de la colocación de la semilla y el riesgo de fallas de distribución', 0);
END
GO

-- =================================================================
-- 3. CREAR VALORES PARA LOS TIPOS CATEGÓRICOS (Usando SELECT para evitar duplicados)
-- =================================================================
-- Valores para Humedad
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Óptima (Humedad a capacidad de campo)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0
FROM [questionnaires].[Cat_Tipo_Preguntas]
WHERE [descripcion] = N'Condición de Humedad del Suelo'
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Óptima (Humedad a capacidad de campo)' AND [id_tipo_pregunta] = [questionnaires].[Cat_Tipo_Preguntas].[id_tipo_pregunta]);

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Seca (Déficit hídrico superficial)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0
FROM [questionnaires].[Cat_Tipo_Preguntas]
WHERE [descripcion] = N'Condición de Humedad del Suelo'
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Seca (Déficit hídrico superficial)' AND [id_tipo_pregunta] = [questionnaires].[Cat_Tipo_Preguntas].[id_tipo_pregunta]);

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Encharcada (Exceso de agua / barro)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0
FROM [questionnaires].[Cat_Tipo_Preguntas]
WHERE [descripcion] = N'Condición de Humedad del Suelo'
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Encharcada (Exceso de agua / barro)' AND [id_tipo_pregunta] = [questionnaires].[Cat_Tipo_Preguntas].[id_tipo_pregunta]);

-- Valores para Textura
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Arcilloso (Alta retención, lento calentamiento)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0
FROM [questionnaires].[Cat_Tipo_Preguntas]
WHERE [descripcion] = N'Textura Predominante del Suelo'
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Arcilloso (Alta retención, lento calentamiento)' AND [id_tipo_pregunta] = [questionnaires].[Cat_Tipo_Preguntas].[id_tipo_pregunta]);

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Franco (Equilibrado, ideal para la mayoría de cultivos)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0
FROM [questionnaires].[Cat_Tipo_Preguntas]
WHERE [descripcion] = N'Textura Predominante del Suelo'
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Franco (Equilibrado, ideal para la mayoría de cultivos)' AND [id_tipo_pregunta] = [questionnaires].[Cat_Tipo_Preguntas].[id_tipo_pregunta]);

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Arenoso (Baja retención, drena rápido)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0
FROM [questionnaires].[Cat_Tipo_Preguntas]
WHERE [descripcion] = N'Textura Predominante del Suelo'
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Arenoso (Baja retención, drena rápido)' AND [id_tipo_pregunta] = [questionnaires].[Cat_Tipo_Preguntas].[id_tipo_pregunta]);

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Franco-Arcilloso', [id_tipo_pregunta], N'ACTIVO', 4, GETDATE(), 0
FROM [questionnaires].[Cat_Tipo_Preguntas]
WHERE [descripcion] = N'Textura Predominante del Suelo'
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Franco-Arcilloso' AND [id_tipo_pregunta] = [questionnaires].[Cat_Tipo_Preguntas].[id_tipo_pregunta]);

-- Valores para Maquinaria
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Sembradora de Precisión Neumática', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0
FROM [questionnaires].[Cat_Tipo_Preguntas]
WHERE [descripcion] = N'Tecnología de Maquinaria de Siembra'
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Sembradora de Precisión Neumática' AND [id_tipo_pregunta] = [questionnaires].[Cat_Tipo_Preguntas].[id_tipo_pregunta]);

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Sembradora de Precisión Mecánica', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0
FROM [questionnaires].[Cat_Tipo_Preguntas]
WHERE [descripcion] = N'Tecnología de Maquinaria de Siembra'
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Sembradora de Precisión Mecánica' AND [id_tipo_pregunta] = [questionnaires].[Cat_Tipo_Preguntas].[id_tipo_pregunta]);

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Siembra al Voleo (Sin surcos definidos)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0
FROM [questionnaires].[Cat_Tipo_Preguntas]
WHERE [descripcion] = N'Tecnología de Maquinaria de Siembra'
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Siembra al Voleo (Sin surcos definidos)' AND [id_tipo_pregunta] = [questionnaires].[Cat_Tipo_Preguntas].[id_tipo_pregunta]);

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Siembra Directa / No Till', [id_tipo_pregunta], N'ACTIVO', 4, GETDATE(), 0
FROM [questionnaires].[Cat_Tipo_Preguntas]
WHERE [descripcion] = N'Tecnología de Maquinaria de Siembra'
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Siembra Directa / No Till' AND [id_tipo_pregunta] = [questionnaires].[Cat_Tipo_Preguntas].[id_tipo_pregunta]);
GO

-- =================================================================
-- 4. CREAR EL FORMULARIO (TEST) CON PROMPT BASE DINÁMICO
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Tests] WHERE [titulo] = N'INFORME TÉCNICO DE ASISTENCIA A SIEMBRA')
BEGIN
    INSERT INTO [questionnaires].[Tests] (
        [titulo], [descripcion], [tipo_test], [estado], [id_categoria], 
        [fecha_publicacion], [created_at], [tiempo], [caducidad], [color], [IsDeleted]
    ) 
    SELECT 
        N'INFORME TÉCNICO DE ASISTENCIA A SIEMBRA', 
        N'Actúa como un Ingeniero Agrónomo Senior. Redacta un "Informe Técnico de Asistencia a Siembra" profesional basado en los datos recolectados. 
        Aplica la regla de contexto de cada pregunta para evaluar riesgos. 
        Estructura obligatoria: 
        1. Resumen Ejecutivo del Lote. 
        2. Análisis de Condiciones (Semilla, Suelo y Clima). 
        3. Evaluación de la Operación (Maquinaria y parámetros). 
        4. Matriz de Riesgos Identificados (Alerta si hay condiciones críticas). 
        5. Recomendaciones Agronómicas Accionables.', 
        N'GENERACION_IA', N'ACTIVO', [Id_categoria], 
        GETDATE(), GETDATE(), 20, 365, N'#2E7D32', 0
    FROM [questionnaires].[Cat_Categorias_Test]
    WHERE [descripcion] = N'AGRICULTURA';
END
GO

-- =================================================================
-- 5. CREAR LAS PREGUNTAS DEL FORMULARIO (Autocontenidas, sin variables)
-- =================================================================
DECLARE @TestID INT = (SELECT [id_test] FROM [questionnaires].[Tests] WHERE [titulo] = N'INFORME TÉCNICO DE ASISTENCIA A SIEMBRA');

-- Solo ejecutamos la inserción de preguntas si el test existe y no tiene preguntas aún
IF @TestID IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Pregunta_Tests] WHERE [id_test] = @TestID AND [seccion] = N'DATOS GENERALES')
BEGIN
    -- SECCIÓN: DATOS GENERALES
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Identificación interna del lote o parcela', @TestID, 1, GETDATE(), GETDATE(), N'DATOS GENERALES', N'La IA usará este dato para personalizar el encabezado del informe.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Superficie total a sembrar (en hectáreas)', @TestID, 2, GETDATE(), GETDATE(), N'DATOS GENERALES', N'Dato numérico crítico. La IA lo cruzará con la cantidad de semilla para validar la densidad.', 0;

    -- SECCIÓN: SEMILLA (Reutilizamos DICOTOMICA ID 3)
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Variedad específica de la semilla (ej. Maíz DK-700)', @TestID, 1, GETDATE(), GETDATE(), N'SEMILLA', N'La IA aplicará conocimientos específicos sobre los requerimientos de esta variedad.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿La semilla cuenta con tratamiento fungicida/insecticida industrial?', @TestID, 3, GETDATE(), GETDATE(), N'SEMILLA', N'Si es NO, la IA debe generar una alerta en "Matriz de Riesgos" sobre ataques tempranos de plagas.', 0;

    -- SECCIÓN: SUELO Y CLIMA (Subconsultas para obtener IDs dinámicamente)
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Seleccione la condición de humedad del suelo al momento de la siembra:', @TestID, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Condición de Humedad del Suelo'), 
           GETDATE(), GETDATE(), N'SUELO Y CLIMA', N'Si es "Encharcada" o "Seca", la IA debe destacar esto como factor limitante crítico en el Resumen Ejecutivo.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Seleccione la textura predominante del suelo:', @TestID, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Textura Predominante del Suelo'), 
           GETDATE(), GETDATE(), N'SUELO Y CLIMA', N'La IA ajustará las recomendaciones de profundidad de siembra según la textura seleccionada.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Pronóstico climático para las próximas 72 horas (describir brevemente)', @TestID, 1, GETDATE(), GETDATE(), N'SUELO Y CLIMA', N'La IA evaluará la ventana de siembra. Lluvias intensas = riesgo de encostramiento; Sequía = riesgo de fallas de nascencia.', 0;

    -- SECCIÓN: OPERACIÓN
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Seleccione el tipo de maquinaria utilizada:', @TestID, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tecnología de Maquinaria de Siembra'), 
           GETDATE(), GETDATE(), N'OPERACIÓN', N'Si es "Al Voleo", la IA ajustará las recomendaciones de control de malezas por falta de surcos.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Profundidad de siembra establecida (en cm)', @TestID, 2, GETDATE(), GETDATE(), N'OPERACIÓN', N'La IA validará si esta profundidad es adecuada para la semilla y textura de suelo informadas.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Velocidad de avance de la maquinaria (en km/h)', @TestID, 2, GETDATE(), GETDATE(), N'OPERACIÓN', N'Si supera 7-8 km/h, la IA debe advertir sobre riesgo de mala distribución y rebote de discos.', 0;

    -- SECCIÓN: OBSERVACIONES
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Observaciones adicionales o incidencias detectadas en campo', @TestID, 1, GETDATE(), GETDATE(), N'OBSERVACIONES', N'Campo comodín para que la IA capture matices humanos o problemas logísticos no cubiertos por las preguntas estructuradas.', 0;
    
    PRINT '✅ Formulario de SIEMBRA creado exitosamente con todas sus preguntas y valores.';
END
ELSE
BEGIN
    PRINT 'ℹ️ El formulario de SIEMBRA ya existe o no se pudo encontrar. No se realizaron cambios.';
END
GO