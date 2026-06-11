using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.ApiChat.AutomaticIA.TrainingModule;
using BusinessLogic.IAModule.Model;

namespace BusinessLogic.SinteticTRainingData
{
    public class Student
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string Career { get; set; }
        public double Average { get; set; }
    }

    public class StudentsSimulator
    {
        private readonly Random rnd = new();

        private readonly string[] nombres1 =
        {
            "Juan","Pedro","Carlos","José","Luis",
            "Daniel","Miguel","Javier","Andrés","Mario",
            "Ana","María","Sofía","Gabriela","Marta",
            "Lucía","Valeria","Paola","Andrea","Carla"
        };

        private readonly string[] nombres2 =
        {
            "Antonio","Enrique","Alberto","Manuel","David",
            "Francisco","Alejandro","Ricardo","Roberto","Fernando",
            "Isabel","Patricia","Fernanda","Cristina","Elena",
            "Verónica","Rosa","Tatiana","Yadira","Karla"
        };

        private readonly string[] apellido1 =
        {
            "Pérez","López","Ramírez","Martínez","González",
            "Hernández","Castillo","Morales","Ruiz","Torres",
            "Flores","Mendoza","Vargas","Rojas","Silva",
            "García","Navarro","Ortega","Rivera","Blandón"
        };

        private readonly string[] apellido2 =
        {
            "Sequeira","Espinoza","Duarte","Cruz","Mairena",
            "Téllez","Alemán","Jarquín","Orozco","Centeno",
            "Pineda","Mejía","Salinas","Acevedo","Lacayo",
            "Argüello","Obando","Mayorga","Bermúdez","Narváez"
        };

        private readonly string[] departamentos =
        {
            "Managua",
            "León",
            "Chinandega",
            "Masaya",
            "Granada",
            "Carazo",
            "Rivas",
            "Boaco",
            "Matagalpa",
            "Jinotega",
            "Estelí",
            "Madriz",
            "Nueva Segovia",
            "Chontales",
            "Río San Juan",
            "RAAN",
            "RAAS"
        };

        private readonly string[] carreras =
        {
            "Ingeniería en Sistemas",
            "Ingeniería Industrial",
            "Ingeniería Civil",
            "Arquitectura",
            "Administración de Empresas",
            "Contabilidad Pública",
            "Derecho",
            "Medicina",
            "Psicología",
            "Mercadotecnia"
        };

        private readonly string[] materiasGenerales =
        {
            "Matemática I",
            "Matemática II",
            "Estadística I",
            "Estadística II",
            "Lengua y Comunicación",
            "Metodología de la Investigación",
            "Inglés I",
            "Inglés II",
            "Ética Profesional",
            "Realidad Nacional"
        };

        private readonly Dictionary<string, string[]> materiasEspecialidad =
            new()
            {
                ["Ingeniería en Sistemas"] = new[]
                {
                    "Programación I",
                    "Programación II",
                    "Base de Datos",
                    "Redes",
                    "Ingeniería de Software",
                    "Sistemas Operativos",
                    "Inteligencia Artificial",
                    "Desarrollo Web",
                    "Arquitectura de Software",
                    "Seguridad Informática"
                },

                ["Ingeniería Industrial"] = new[]
                {
                    "Procesos Industriales",
                    "Control de Calidad",
                    "Investigación de Operaciones",
                    "Logística",
                    "Producción Industrial",
                    "Gestión de Inventarios",
                    "Planeación Industrial",
                    "Diseño Industrial",
                    "Automatización",
                    "Gestión de Riesgos"
                },

                ["Ingeniería Civil"] = new[]
                {
                    "Topografía",
                    "Resistencia de Materiales",
                    "Mecánica de Suelos",
                    "Hidráulica",
                    "Diseño Estructural",
                    "Construcción I",
                    "Construcción II",
                    "Puentes",
                    "Carreteras",
                    "Urbanismo"
                },

                ["Arquitectura"] = new[]
                {
                    "Diseño Arquitectónico I",
                    "Diseño Arquitectónico II",
                    "Urbanismo",
                    "Paisajismo",
                    "Representación Digital",
                    "Construcción Arquitectónica",
                    "Historia de la Arquitectura",
                    "Diseño Interior",
                    "Maquetas",
                    "Restauración"
                },

                ["Administración de Empresas"] = new[]
                {
                    "Administración I",
                    "Administración II",
                    "Finanzas",
                    "Recursos Humanos",
                    "Gestión Empresarial",
                    "Marketing",
                    "Planeación Estratégica",
                    "Emprendimiento",
                    "Comercio Internacional",
                    "Auditoría"
                },

                ["Contabilidad Pública"] = new[]
                {
                    "Contabilidad I",
                    "Contabilidad II",
                    "Auditoría",
                    "Tributación",
                    "Costos",
                    "Finanzas",
                    "Normas NIIF",
                    "Contabilidad Gerencial",
                    "Presupuestos",
                    "Control Interno"
                },

                ["Derecho"] = new[]
                {
                    "Derecho Civil",
                    "Derecho Penal",
                    "Derecho Laboral",
                    "Derecho Mercantil",
                    "Derecho Constitucional",
                    "Derecho Administrativo",
                    "Derecho Internacional",
                    "Criminología",
                    "Derecho Procesal",
                    "Mediación"
                },

                ["Medicina"] = new[]
                {
                    "Anatomía",
                    "Fisiología",
                    "Patología",
                    "Farmacología",
                    "Cirugía General",
                    "Pediatría",
                    "Ginecología",
                    "Neurología",
                    "Medicina Interna",
                    "Epidemiología"
                },

                ["Psicología"] = new[]
                {
                    "Psicología General",
                    "Psicología Clínica",
                    "Psicología Infantil",
                    "Psicoterapia",
                    "Neuropsicología",
                    "Psicología Social",
                    "Psicometría",
                    "Orientación Vocacional",
                    "Salud Mental",
                    "Evaluación Psicológica"
                },

                ["Mercadotecnia"] = new[]
                {
                    "Marketing I",
                    "Marketing II",
                    "Publicidad",
                    "Investigación de Mercados",
                    "Branding",
                    "Marketing Digital",
                    "Ventas",
                    "Comportamiento del Consumidor",
                    "CRM",
                    "Analítica Comercial"
                }
            };

        public async Task Execute()
        {
            List<PreRagElement> ragData = new();

            int studentNumber = 600;

            foreach (var carrera in carreras)
            {
                for (int i = 0; i < 10; i++)
                {
                    string studentId = $"EST-{studentNumber:0000}";

                    string nombreCompleto =
                        $"{nombres1[rnd.Next(nombres1.Length)]} " +
                        $"{nombres2[rnd.Next(nombres2.Length)]} " +
                        $"{apellido1[rnd.Next(apellido1.Length)]} " +
                        $"{apellido2[rnd.Next(apellido2.Length)]}";

                    string departamento =
                        departamentos[rnd.Next(departamentos.Length)];

                    double promedioGeneral = 0;
                    int materiasAprobadas = 0;

                    var todasMaterias =
                        materiasGenerales
                        .Concat(materiasEspecialidad[carrera])
                        .ToList();

                    List<int> notas = new();

                    var student = new Student
                    {
                        Id = studentId,
                        FullName = nombreCompleto,
                        Department = departamento,
                        Career = carrera
                    };

                    ragData.Add(new PreRagElement
                    {
                        Id_Servicio = 1,
                        Category = "Universidad",
                        Query = $"Información académica de {student.FullName}",
                        BodyResponse =
                            $"Alumno ID {student.Id}. " +
                            $"Nombre {student.FullName}. " +
                            $"Departamento {student.Department}. " +
                            $"Carrera {student.Career}."
                    });

                    for (int year = 1; year <= 5; year++)
                    {
                        var materiasAnuales =
                            todasMaterias
                            .Skip((year - 1) * 4)
                            .Take(4)
                            .ToList();

                        List<int> notasAnuales = new();

                        foreach (var materia in materiasAnuales)
                        {
                            int nota = rnd.Next(50, 101);

                            notas.Add(nota);
                            notasAnuales.Add(nota);

                            bool aprobado = nota >= 70;

                            if (aprobado)
                                materiasAprobadas++;

                            ragData.Add(new PreRagElement
                            {
                                Id_Servicio = 1,
                                Category = "Universidad",
                                Query =
                                    $"Resultado de {materia} para {student.FullName}",

                                BodyResponse =
                                    $"Alumno ID {student.Id}. " +
                                    $"Nombre {student.FullName}. " +
                                    $"Carrera {student.Career}. " +
                                    $"Año académico {year}. " +
                                    $"Materia {materia}. " +
                                    $"Nota final {nota}. " +
                                    $"Estado {(aprobado ? "Aprobado" : "Reprobado")}."
                            });
                        }

                        double promedioAnual =
                            Math.Round(notasAnuales.Average(), 2);

                        ragData.Add(new PreRagElement
                        {
                            Id_Servicio = 1,
                            Category = "Universidad",
                            Query =
                                $"Resumen académico año {year} de {student.FullName}",

                            BodyResponse =
                                $"Alumno ID {student.Id}. " +
                                $"Carrera {student.Career}. " +
                                $"Año académico {year}. " +
                                $"Promedio anual {promedioAnual}."
                        });
                    }

                    promedioGeneral =
                        Math.Round(notas.Average(), 2);

                    ragData.Add(new PreRagElement
                    {
                        Id_Servicio = 1,
                        Category = "Universidad",
                        Query =
                            $"Promedio general de {student.FullName}",

                        BodyResponse =
                            $"Alumno ID {student.Id}. " +
                            $"Nombre {student.FullName}. " +
                            $"Carrera {student.Career}. " +
                            $"Promedio general {promedioGeneral}. " +
                            $"Materias aprobadas {materiasAprobadas} de 20. " +
                            $"Estado Graduado."
                    });

                    studentNumber++;
                }
            }

            Console.WriteLine($"Documentos generados: {ragData.Count}");

            VectorizationWorker.ProcessTraining(ragData);
        }
    }
}