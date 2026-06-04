USE [GAUT_IA];
GO

-- =================================================================
-- 1. CREAR TIPOS DE PREGUNTAS CATEGÓRICAS ESPECÍFICOS PARA LABORATORIOS
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Propósito Principal del Laboratorio')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Propósito Principal del Laboratorio', N'ACTIVA', N'Determina el perfil de hardware (CPU/GPU/RAM) y el software a preinstalar', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Gama de Hardware Requerida')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Gama de Hardware Requerida', N'ACTIVA', N'Ajusta el presupuesto y la vida útil esperada de los equipos (3, 5 o 7 años)', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Distribución Física')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Tipo de Distribución Física', N'ACTIVA', N'Define el mobiliario, el cableado y la dinámica de enseñanza (clase magistral vs trabajo colaborativo)', 0);
END

IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Gestión de Aula')
BEGIN
    INSERT INTO [questionnaires].[Cat_Tipo_Preguntas] ([tipo_pregunta], [descripcion], [estado], [descripcion_general], [IsDeleted]) 
    VALUES (N'CATEGORICA', N'Sistema de Gestión de Aula', N'ACTIVA', N'Condiciona el software de control, monitoreo y seguridad a cotizar', 0);
END
GO

-- =================================================================
-- 2. CREAR VALORES PARA LOS TIPOS CATEGÓRICOS DE LABORATORIOS
-- =================================================================
-- Valores para Propósito
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Uso General / Ofimática (Navegación, Office, Web)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Propósito Principal del Laboratorio' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Uso General / Ofimática (Navegación, Office, Web)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Programación y Desarrollo de Software (IDEs, VMs, Contenedores)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Propósito Principal del Laboratorio' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Programación y Desarrollo de Software (IDEs, VMs, Contenedores)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Diseño Gráfico y Edición de Video (Adobe CC, 3D ligero)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Propósito Principal del Laboratorio' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Diseño Gráfico y Edición de Video (Adobe CC, 3D ligero)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Ingeniería y CAD/BIM (AutoCAD, SolidWorks, Revit, Renderizado)', [id_tipo_pregunta], N'ACTIVO', 4, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Propósito Principal del Laboratorio' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Ingeniería y CAD/BIM (AutoCAD, SolidWorks, Revit, Renderizado)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Ciencia de Datos e IA (Machine Learning, Big Data, GPU Computing)', [id_tipo_pregunta], N'ACTIVO', 5, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Propósito Principal del Laboratorio' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Ciencia de Datos e IA (Machine Learning, Big Data, GPU Computing)');

-- Valores para Gama de Hardware
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Gama Básica (Costo optimizado, vida útil 3 años)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Gama de Hardware Requerida' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Gama Básica (Costo optimizado, vida útil 3 años)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Gama Media (Balance rendimiento/costo, vida útil 5 años)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Gama de Hardware Requerida' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Gama Media (Balance rendimiento/costo, vida útil 5 años)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Gama Alta / Workstation (Máximo rendimiento, vida útil 7 años)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Gama de Hardware Requerida' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Gama Alta / Workstation (Máximo rendimiento, vida útil 7 años)');

-- Valores para Distribución Física
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Filas Tradicionales (Enfocadas al instructor/pizarra)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Distribución Física' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Filas Tradicionales (Enfocadas al instructor/pizarra)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Islas / Racimos (Trabajo colaborativo en grupos de 4-6)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Distribución Física' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Islas / Racimos (Trabajo colaborativo en grupos de 4-6)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Perimetral / En U (Mixto: clase magistral + práctica individual)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Distribución Física' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Perimetral / En U (Mixto: clase magistral + práctica individual)');

-- Valores para Gestión de Aula
INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Sin gestión centralizada (Independiente)', [id_tipo_pregunta], N'ACTIVO', 1, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Gestión de Aula' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Sin gestión centralizada (Independiente)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Software de Control de Aula (LanSchool, NetSupport, Veyon)', [id_tipo_pregunta], N'ACTIVO', 2, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Gestión de Aula' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Software de Control de Aula (LanSchool, NetSupport, Veyon)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Virtualización de Escritorios (VDI - VMware Horizon, Citrix)', [id_tipo_pregunta], N'ACTIVO', 3, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Gestión de Aula' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Virtualización de Escritorios (VDI - VMware Horizon, Citrix)');

INSERT INTO [questionnaires].[Cat_Valor_Preguntas] ([descripcion], [id_tipo_pregunta], [estado], [valor], [created_at], [IsDeleted])
SELECT N'Sistema de Imagen/Clonación (Deep Freeze, FOG Project, SCCM)', [id_tipo_pregunta], N'ACTIVO', 4, GETDATE(), 0 
FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Gestión de Aula' 
AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Cat_Valor_Preguntas] WHERE [descripcion] = N'Sistema de Imagen/Clonación (Deep Freeze, FOG Project, SCCM)');
GO

-- =================================================================
-- 3. CREAR EL FORMULARIO (TEST) CON PROMPT DE IA ESPECIALIZADO
-- =================================================================
IF NOT EXISTS (SELECT 1 FROM [questionnaires].[Tests] WHERE [titulo] = N'DISEÑO DE LABORATORIO DE CÓMPUTO')
BEGIN
    INSERT INTO [questionnaires].[Tests] (
        [titulo], [descripcion], [tipo_test], [estado], [id_categoria], 
        [fecha_publicacion], [created_at], [tiempo], [caducidad], [color], [IsDeleted]
    ) 
    SELECT 
        N'DISEÑO DE LABORATORIO DE CÓMPUTO', 
        N'Actúa como un Arquitecto de Soluciones TI Senior especializado en el diseño de Laboratorios de Cómputo para entornos educativos, corporativos y de entrenamiento. 
        Tu tarea es redactar una "Propuesta Técnica para el Diseño e Implementación de un Laboratorio de Cómputo" profesional, detallada y orientada a la experiencia de aprendizaje. 
        Analiza los datos recolectados y aplica las reglas de contexto de cada pregunta para dimensionar correctamente el hardware, la red y el mobiliario. 
        Estructura obligatoria del documento:
        1. Resumen Ejecutivo y Entendimiento del Propósito Pedagógico/Operativo.
        2. Especificaciones Técnicas del Hardware (CPU, RAM, GPU, Almacenamiento) justificadas según el software a ejecutar.
        3. Diseño de Infraestructura de Red (Switches, Cableado, VLANs, Ancho de Banda).
        4. Propuesta de Mobiliario y Ergonomía (Mesas, sillas, distribución según el tipo de dinámica de clase).
        5. Infraestructura Eléctrica y Climatización (Tomacorrientes, UPS, BTU/h necesarios).
        6. Software de Gestión, Seguridad y Licenciamiento.
        7. Presupuesto Estimado por Categorías y Cronograma de Implementación.
        Usa un tono técnico pero accesible, enfatizando la durabilidad, la gestionabilidad centralizada y el retorno de la inversión educativa.', 
        N'GENERACION_IA', N'ACTIVO', [Id_categoria], 
        GETDATE(), GETDATE(), 25, 365, N'#0277BD', 0
    FROM [questionnaires].[Cat_Categorias_Test]
    WHERE [descripcion] = N'TECNOLOGIA';
END
GO

-- =================================================================
-- 4. CREAR LAS PREGUNTAS DEL FORMULARIO (Autocontenidas)
-- =================================================================
DECLARE @TestID_Lab INT = (SELECT [id_test] FROM [questionnaires].[Tests] WHERE [titulo] = N'DISEÑO DE LABORATORIO DE CÓMPUTO');

IF @TestID_Lab IS NOT NULL AND NOT EXISTS (SELECT 1 FROM [questionnaires].[Pregunta_Tests] WHERE [id_test] = @TestID_Lab AND [seccion] = N'DATOS GENERALES DEL PROYECTO')
BEGIN
    -- SECCIÓN 1: DATOS GENERALES
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Nombre de la institución/empresa y persona de contacto', @TestID_Lab, 1, GETDATE(), GETDATE(), N'DATOS GENERALES DEL PROYECTO', N'La IA usará esto para personalizar el encabezado y adaptar el tono (educativo vs corporativo).', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Cantidad total de estaciones de trabajo (PCs) a instalar', @TestID_Lab, 2, GETDATE(), GETDATE(), N'DATOS GENERALES DEL PROYECTO', N'Dato fundamental. La IA lo usará para dimensionar switches, licencias, mobiliario y carga eléctrica total.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Dimensiones del aula disponible (largo x ancho en metros)', @TestID_Lab, 1, GETDATE(), GETDATE(), N'DATOS GENERALES DEL PROYECTO', N'La IA calculará si la densidad de puestos es adecuada y recomendará la distribución óptima según los m2 disponibles.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Se incluye también una estación de instructor con proyector/pantalla?', @TestID_Lab, 3, GETDATE(), GETDATE(), N'DATOS GENERALES DEL PROYECTO', N'Si es SÍ, la IA añadirá en la propuesta un PC de mayores prestaciones, sistema de proyección y audio para el docente.', 0;

    -- SECCIÓN 2: PROPÓSITO Y HARDWARE
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Propósito principal del laboratorio', @TestID_Lab, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Propósito Principal del Laboratorio'), 
           GETDATE(), GETDATE(), N'PROPOSITO Y HARDWARE', N'La IA definirá el perfil de hardware: Uso General (i3/8GB), Programación (i5/16GB), Diseño (i7/32GB+GPU), CAD (Workstation), IA (GPU dedicada de alto rendimiento).', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Gama de hardware requerida', @TestID_Lab, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Gama de Hardware Requerida'), 
           GETDATE(), GETDATE(), N'PROPOSITO Y HARDWARE', N'Ajusta el presupuesto. Si el propósito es "CAD" pero la gama es "Básica", la IA debe alertar sobre la incompatibilidad y recomendar upgrade.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Lista de software especializado que se ejecutará (ej. AutoCAD 2026, Photoshop, Visual Studio, SPSS)', @TestID_Lab, 1, GETDATE(), GETDATE(), N'PROPOSITO Y HARDWARE', N'La IA cruzará esta lista con los requisitos oficiales de cada software para validar o ajustar las especificaciones de CPU/RAM/GPU propuestas.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Se requiere almacenamiento en estado sólido (SSD) o disco duro tradicional (HDD)?', @TestID_Lab, 3, GETDATE(), GETDATE(), N'PROPOSITO Y HARDWARE', N'Si es SSD, la IA destacará la mejora en tiempos de arranque y carga de aplicaciones. Si es HDD, alertará sobre cuellos de botella en software moderno.', 0;

    -- SECCIÓN 3: RED Y CONECTIVIDAD
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Velocidad de red requerida por puesto (100Mbps, 1Gbps, 10Gbps)', @TestID_Lab, 1, GETDATE(), GETDATE(), N'RED Y CONECTIVIDAD', N'La IA dimensionará los switches. Para laboratorios de >20 PCs con VDI o transferencia de archivos grandes, recomendará 1Gbps mínimo y uplinks de 10Gbps.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Se requiere cableado estructurado nuevo (Categoría 6/6A) o se reutiliza el existente?', @TestID_Lab, 3, GETDATE(), GETDATE(), N'RED Y CONECTIVIDAD', N'Si es nuevo, la IA incluirá en el presupuesto la partida de cableado, patch panels, canaletas y certificación de puntos de red.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Se necesitan VLANs separadas para estudiantes, invitados y administración?', @TestID_Lab, 3, GETDATE(), GETDATE(), N'RED Y CONECTIVIDAD', N'Si es SÍ, la IA recomendará switches gestionables L2/L3 y detallará la segmentación de red para seguridad y control de ancho de banda.', 0;

    -- SECCIÓN 4: MOBILIARIO Y DISTRIBUCIÓN
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Tipo de distribución física preferida', @TestID_Lab, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Tipo de Distribución Física'), 
           GETDATE(), GETDATE(), N'MOBILIARIO Y DISTRIBUCION', N'La IA recomendará el mobiliario: Filas (mesas lineales con canaleta porta-cables), Islas (mesas trapezoidales/modulares), En U (mesas curvas perimetrales).', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Se requiere mobiliario ergonómico especializado (sillas con soporte lumbar, mesas ajustables en altura)?', @TestID_Lab, 3, GETDATE(), GETDATE(), N'MOBILIARIO Y DISTRIBUCION', N'Si es SÍ, la IA incluirá sillas de gama alta y justificará la inversión en salud ocupacional y productividad para sesiones largas.', 0;

    -- SECCIÓN 5: INFRAESTRUCTURA ELÉCTRICA Y CLIMATIZACIÓN
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Cantidad de tomacorrientes por puesto (mínimo 2: PC + monitor, o 3 si hay docking)', @TestID_Lab, 2, GETDATE(), GETDATE(), N'INFRAESTRUCTURA ELECTRICA', N'La IA calculará la carga eléctrica total y recomendará la instalación de circuitos dedicados y UPS centralizado o por fila.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿El aula cuenta con aire acondicionado existente o se debe cotizar uno nuevo?', @TestID_Lab, 3, GETDATE(), GETDATE(), N'INFRAESTRUCTURA ELECTRICA', N'Si no tiene, la IA calculará los BTU/h necesarios basándose en la cantidad de PCs (cada PC genera ~300-800 BTU/h) + ocupación humana + iluminación.', 0;

    -- SECCIÓN 6: GESTIÓN Y SEGURIDAD
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Sistema de gestión de aula requerido', @TestID_Lab, 
           (SELECT [id_tipo_pregunta] FROM [questionnaires].[Cat_Tipo_Preguntas] WHERE [descripcion] = N'Sistema de Gestión de Aula'), 
           GETDATE(), GETDATE(), N'GESTION Y SEGURIDAD', N'La IA detallará las funcionalidades: Control de pantalla, bloqueo de USB, distribución de exámenes, monitoreo remoto, o virtualización completa de escritorios.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'¿Se requiere bloqueo físico de puertos USB y seguridad antirrobo para los equipos?', @TestID_Lab, 3, GETDATE(), GETDATE(), N'GESTION Y SEGURIDAD', N'Si es SÍ, la IA incluirá candones de seguridad Kensington, tapones físicos para USB y software de control de dispositivos extraíbles.', 0;

    -- SECCIÓN 7: PRESUPUESTO Y PLAZOS
    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Presupuesto estimado total del proyecto (en moneda local)', @TestID_Lab, 2, GETDATE(), GETDATE(), N'PRESUPUESTO Y PLAZOS', N'La IA ajustará las recomendaciones para no exceder este monto, priorizando lo crítico (PCs, red) y ofreciendo alternativas para lo accesorio.', 0;

    INSERT INTO [questionnaires].[Pregunta_Tests] ([estado], [descripcion_pregunta], [id_test], [id_tipo_pregunta], [fecha], [created_at], [seccion], [descripcion_general], [IsDeleted]) 
    SELECT N'ACTIVO', N'Fecha límite deseada para la puesta en marcha y observaciones adicionales', @TestID_Lab, 1, GETDATE(), GETDATE(), N'PRESUPUESTO Y PLAZOS', N'La IA evaluará la viabilidad del cronograma. Si el plazo es <30 días para >20 PCs, alertará sobre posibles retrasos en cadena de suministro.', 0;

    PRINT '✅ Formulario de DISEÑO DE LABORATORIO DE CÓMPUTO creado exitosamente con todas sus preguntas y valores.';
END
ELSE
BEGIN
    PRINT 'ℹ️ El formulario de DISEÑO DE LABORATORIO DE CÓMPUTO ya existe o no se pudo encontrar. No se realizaron cambios.';
END
GO