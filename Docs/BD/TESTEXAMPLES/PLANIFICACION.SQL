USE [GAUT_IA];
GO

-- =================================================================
-- 1. CREAR CATEGORÍA "PLANIFICACION"
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Categorias_Test] WHERE [descripcion] = N'PLANIFICACION')
BEGIN
    INSERT INTO [questionnaires].[Cat_Categorias_Test] ([descripcion], [estado], [created_at], [IsDeleted]) 
    VALUES (N'PLANIFICACION', N'ACTIVA', GETDATE(), 0);
END
GO

-- =================================================================
-- 2. CREAR TIPOS DE PREGUNTAS CATEGÓRICAS PARA PLANIFICACIÓN
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Actividad Institucional')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Tipo de Actividad Institucional', N'ACTIVA', N'Determina la naturaleza, complejidad y recursos típicos requeridos', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Alcance de la Actividad')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Alcance de la Actividad', N'ACTIVA', N'Define el nivel de coordinación, comunicación y stakeholders involucrados', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Nivel de Prioridad')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Nivel de Prioridad', N'ACTIVA', N'Condiciona la asignación de recursos y la urgencia en el cronograma', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Recursos Principales')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Tipo de Recursos Principales', N'ACTIVA', N'Orienta la sección de presupuesto y gestión de recursos', 0);
END
GO

-- =================================================================
-- 3. CREAR VALORES PARA LOS TIPOS CATEGÓRICOS
-- =================================================================
-- Valores para Tipo de Actividad
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Académica (Cursos, seminarios, congresos)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Actividad Institucional' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Académica (Cursos, seminarios, congresos)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Administrativa (Procesos internos, auditorías, certificaciones)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Actividad Institucional' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Administrativa (Procesos internos, auditorías, certificaciones)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Cultural (Eventos artísticos, celebraciones, exposiciones)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Actividad Institucional' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Cultural (Eventos artísticos, celebraciones, exposiciones)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Deportiva (Torneos, olimpiadas, actividades recreativas)', [id_tipo_pregunta], N'ACTIVO', 4, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Actividad Institucional' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Deportiva (Torneos, olimpiadas, actividades recreativas)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Investigación (Proyectos, publicaciones, convenios)', [id_tipo_pregunta], N'ACTIVO', 5, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Actividad Institucional' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Investigación (Proyectos, publicaciones, convenios)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Extensión y Vinculación (Servicio comunitario, prácticas, pasantías)', [id_tipo_pregunta], N'ACTIVO', 6, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Actividad Institucional' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Extensión y Vinculación (Servicio comunitario, prácticas, pasantías)');

-- Valores para Alcance
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Departamental (Solo un departamento o área específica)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Alcance de la Actividad' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Departamental (Solo un departamento o área específica)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Facultativo o Divisional (Varios departamentos de una facultad)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Alcance de la Actividad' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Facultativo o Divisional (Varios departamentos de una facultad)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Institucional (Toda la organización)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Alcance de la Actividad' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Institucional (Toda la organización)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Interinstitucional (Con otras organizaciones o entidades externas)', [id_tipo_pregunta], N'ACTIVO', 4, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Alcance de la Actividad' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Interinstitucional (Con otras organizaciones o entidades externas)');

-- Valores para Prioridad
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Alta (Crítica, no negociable en fecha)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Nivel de Prioridad' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Alta (Crítica, no negociable en fecha)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Media (Importante, con cierto margen de flexibilidad)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Nivel de Prioridad' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Media (Importante, con cierto margen de flexibilidad)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Baja (Deseable, ajustable según disponibilidad)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Nivel de Prioridad' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Baja (Deseable, ajustable según disponibilidad)');

-- Valores para Recursos
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Principalmente Humanos (Personal, ponentes, voluntarios)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Recursos Principales' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Principalmente Humanos (Personal, ponentes, voluntarios)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Principalmente Financieros (Presupuesto, patrocinios, pagos)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Recursos Principales' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Principalmente Financieros (Presupuesto, patrocinios, pagos)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Principalmente Tecnológicos (Software, equipos, plataformas)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Recursos Principales' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Principalmente Tecnológicos (Software, equipos, plataformas)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Principalmente Físicos (Espacios, infraestructura, materiales)', [id_tipo_pregunta], N'ACTIVO', 4, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Recursos Principales' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Principalmente Físicos (Espacios, infraestructura, materiales)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Mixto (Combinación de varios tipos de recursos)', [id_tipo_pregunta], N'ACTIVO', 5, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Recursos Principales' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Mixto (Combinación de varios tipos de recursos)');
GO

-- =================================================================
-- 4. CREAR EL FORMULARIO (TEST) CON PROMPT DE IA ESPECIALIZADO
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Tests] WHERE [titulo] = N'PLANIFICACIÓN DE ACTIVIDADES INSTITUCIONALES')
BEGIN
    INSERT INTO [questionnaires].[Tests] (
        [titulo], [descripcion], [tipo_test], [estado], [id_categoria], 
        [fecha_publicacion], [created_at], [tiempo], [caducidad], [color], [IsDeleted]
    ) 
    SELECT 
        N'PLANIFICACIÓN DE ACTIVIDADES INSTITUCIONALES', 
        N'Actúa como un Planificador Estratégico y Gestor de Proyectos Institucionales Senior. 
        Tu tarea es redactar un "Plan de Actividades Institucional" profesional, detallado y ejecutable. 
        Analiza los datos recolectados y aplica las reglas de contexto de cada pregunta para crear un plan alineado con los objetivos institucionales. 
        Estructura obligatoria del documento:
        1. Resumen Ejecutivo y Alineación Estratégica (cómo esta actividad contribuye a la misión institucional).
        2. Objetivos SMART (Específicos, Medibles, Alcanzables, Relevantes, con Tiempo definido).
        3. Descripción Detallada de la Actividad (alcance, público objetivo, metodología).
        4. Cronograma de Implementación (fases, hitos, fechas clave, diagrama de Gantt textual).
        5. Asignación de Recursos (humanos, financieros, tecnológicos, físicos) con estimación de costos.
        6. Matriz de Riesgos y Plan de Contingencia (riesgos identificados, probabilidad, impacto, mitigación).
        7. Indicadores de Éxito (KPIs) y Mecanismos de Evaluación.
        8. Plan de Comunicación y Difusión (stakeholders, canales, mensajes clave).
        Usa un tono profesional, estructurado y orientado a resultados, enfatizando la viabilidad y el impacto institucional.', 
        N'GENERACION_IA', N'ACTIVO', [Id_categoria], 
        GETDATE(), GETDATE(), 30, 365, N'#6A1B9A', 0
    FROM [questionnaires].[Cat_Categorias_Test]
    WHERE [descripcion] = N'PLANIFICACION';
END
GO

-- =================================================================
-- 5. CREAR LAS PREGUNTAS DEL FORMULARIO
-- =================================================================
DECLARE @TestID_Plan INT = (SELECT [id_test] FROM [questionnaires].[Tests] WHERE [titulo] = N'PLANIFICACIÓN DE ACTIVIDADES INSTITUCIONALES');

IF @TestID_Plan IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Pregunta_Tests] WHERE [id_test] = @TestID_Plan AND [seccion] = N'DATOS GENERALES')
BEGIN
    -- SECCIÓN 1: DATOS GENERALES
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Nombre de la actividad o proyecto', @TestID_Plan, 1, GETDATE(), GETDATE(), N'DATOS GENERALES', N'La IA usará esto como título principal del plan y lo referenciará en todo el documento.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Departamento, área o unidad responsable de la ejecución', @TestID_Plan, 1, GETDATE(), GETDATE(), N'DATOS GENERALES', N'La IA identificará al responsable principal y lo incluirá en la sección de asignación de recursos humanos.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Nombre y cargo del responsable o coordinador general', @TestID_Plan, 1, GETDATE(), GETDATE(), N'DATOS GENERALES', N'La IA usará esto para personalizar el plan y establecer el punto de contacto para aprobaciones.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Tipo de actividad institucional', @TestID_Plan, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Actividad Institucional'), 
           GETDATE(), GETDATE(), N'DATOS GENERALES', N'La IA ajustará el lenguaje, los recursos típicos y los indicadores de éxito según el tipo de actividad seleccionada.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Alcance de la actividad', @TestID_Plan, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Alcance de la Actividad'), 
           GETDATE(), GETDATE(), N'DATOS GENERALES', N'Si es "Interinstitucional", la IA incluirá una sección específica sobre coordinación con socios externos y acuerdos de colaboración.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Nivel de prioridad de la actividad', @TestID_Plan, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Nivel de Prioridad'), 
           GETDATE(), GETDATE(), N'DATOS GENERALES', N'Si es "Alta", la IA enfatizará la urgencia en la asignación de recursos y propondrá un cronograma más ajustado con hitos de control frecuentes.', 0;

    -- SECCIÓN 2: OBJETIVOS Y JUSTIFICACIÓN
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Objetivo general de la actividad (qué se busca lograr)', @TestID_Plan, 1, GETDATE(), GETDATE(), N'OBJETIVOS Y JUSTIFICACION', N'La IA transformará esto en un objetivo SMART y lo usará como base para derivar los objetivos específicos.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Justificación y relevancia institucional (por qué es importante)', @TestID_Plan, 1, GETDATE(), GETDATE(), N'OBJETIVOS Y JUSTIFICACION', N'La IA usará esto para redactar el "Resumen Ejecutivo" y alinear la actividad con la misión y visión institucional.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Público objetivo o beneficiarios (estudiantes, personal, comunidad, etc.)', @TestID_Plan, 1, GETDATE(), GETDATE(), N'OBJETIVOS Y JUSTIFICACION', N'La IA definirá el perfil de los beneficiarios y lo usará para diseñar el plan de comunicación y los indicadores de alcance.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Número estimado de participantes o beneficiarios directos', @TestID_Plan, 2, GETDATE(), GETDATE(), N'OBJETIVOS Y JUSTIFICACION', N'La IA usará este dato para calcular recursos logísticos (espacios, materiales, catering) y establecer KPIs de asistencia.', 0;

    -- SECCIÓN 3: CRONOGRAMA Y FASES
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Fecha de inicio planificada', @TestID_Plan, 1, GETDATE(), GETDATE(), N'CRONOGRAMA Y FASES', N'La IA usará esto como punto de partida para construir el cronograma y calcular la duración total del proyecto.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Fecha de finalización planificada', @TestID_Plan, 1, GETDATE(), GETDATE(), N'CRONOGRAMA Y FASES', N'La IA calculará la duración total y distribuirá las fases proporcionalmente en el cronograma.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Principales fases o etapas de la actividad (describir brevemente)', @TestID_Plan, 1, GETDATE(), GETDATE(), N'CRONOGRAMA Y FASES', N'La IA estructurará el cronograma en base a estas fases, asignando hitos y entregables a cada una.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Existen fechas críticas o hitos inamovibles (ej. fecha de evento, deadline de entrega)?', @TestID_Plan, 1, GETDATE(), GETDATE(), N'CRONOGRAMA Y FASES', N'Si existen, la IA las marcará como "Milestones" en el cronograma y ajustará las tareas previas para garantizar su cumplimiento.', 0;

    -- SECCIÓN 4: RECURSOS Y PRESUPUESTO
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Tipo de recursos principales requeridos', @TestID_Plan, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Recursos Principales'), 
           GETDATE(), GETDATE(), N'RECURSOS Y PRESUPUESTO', N'La IA detallará la gestión específica para cada tipo de recurso en la sección de asignación.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Presupuesto estimado total (en moneda local)', @TestID_Plan, 2, GETDATE(), GETDATE(), N'RECURSOS Y PRESUPUESTO', N'La IA distribuirá este presupuesto por categorías (personal, materiales, logística, etc.) y propondrá un desglose detallado.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Fuentes de financiamiento (presupuesto institucional, patrocinios, fondos externos)', @TestID_Plan, 1, GETDATE(), GETDATE(), N'RECURSOS Y PRESUPUESTO', N'La IA incluirá una sección sobre gestión de fondos y requisitos de reportabilidad según la fuente.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Espacios físicos o instalaciones requeridas (aulas, auditorios, laboratorios, exteriores)', @TestID_Plan, 1, GETDATE(), GETDATE(), N'RECURSOS Y PRESUPUESTO', N'La IA incluirá en el cronograma la reserva de espacios y verificará la disponibilidad según las fechas planificadas.', 0;

    -- SECCIÓN 5: RIESGOS Y CONTINGENCIAS
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Principales riesgos identificados (clima, cancelaciones, problemas técnicos, baja asistencia)', @TestID_Plan, 1, GETDATE(), GETDATE(), N'RIESGOS Y CONTINGENCIAS', N'La IA construirá una matriz de riesgos con probabilidad, impacto y estrategias de mitigación para cada uno.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Se cuenta con planes de contingencia o alternativas ya definidas?', @TestID_Plan, 3, GETDATE(), GETDATE(), N'RIESGOS Y CONTINGENCIAS', N'Si es SÍ, la IA las incorporará en el plan. Si es NO, propondrá planes B para los riesgos más críticos.', 0;

    -- SECCIÓN 6: EVALUACIÓN Y SEGUIMIENTO
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Indicadores de éxito o resultados esperados (KPIs)', @TestID_Plan, 1, GETDATE(), GETDATE(), N'EVALUACION Y SEGUIMIENTO', N'La IA transformará estos indicadores en métricas medibles y definirá mecanismos de recolección de datos.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Mecanismos de evaluación (encuestas, reportes, métricas de asistencia, análisis de impacto)', @TestID_Plan, 1, GETDATE(), GETDATE(), N'EVALUACION Y SEGUIMIENTO', N'La IA detallará cuándo y cómo se aplicarán estos mecanismos, y quién será responsable de cada uno.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Frecuencia de reportes de avance requeridos (semanal, quincenal, mensual)', @TestID_Plan, 1, GETDATE(), GETDATE(), N'EVALUACION Y SEGUIMIENTO', N'La IA establecerá un calendario de reuniones de seguimiento y el formato de los reportes de avance.', 0;

    -- SECCIÓN 7: COMUNICACIÓN
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Estrategia de comunicación y difusión (redes sociales, correo, intranet, medios tradicionales)', @TestID_Plan, 1, GETDATE(), GETDATE(), N'COMUNICACION', N'La IA diseñará un plan de comunicación con cronograma de publicaciones, mensajes clave y responsables.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Stakeholders clave que deben ser informados o involucrados', @TestID_Plan, 1, GETDATE(), GETDATE(), N'COMUNICACION', N'La IA creará una matriz de stakeholders con nivel de influencia, interés y estrategia de comunicación para cada grupo.', 0;

    -- SECCIÓN 8: OBSERVACIONES ADICIONALES
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Observaciones adicionales, restricciones o consideraciones especiales', @TestID_Plan, 1, GETDATE(), GETDATE(), N'OBSERVACIONES ADICIONALES', N'La IA incorporará estas consideraciones en las secciones relevantes del plan (ej. restricciones de horario, requisitos de accesibilidad, normativas específicas).', 0;

    PRINT '✅ Formulario de PLANIFICACIÓN DE ACTIVIDADES INSTITUCIONALES creado exitosamente.';
END
ELSE
BEGIN
    PRINT 'ℹ️ El formulario de PLANIFICACIÓN DE ACTIVIDADES INSTITUCIONALES ya existe o no se pudo encontrar.';
END
GO