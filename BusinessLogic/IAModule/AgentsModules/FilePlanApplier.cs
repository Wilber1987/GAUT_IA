using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APPCORE;
using BusinessLogic.IA.Agent;
using BusinessLogic.IAModule.AgentsModules.Models;
using CAPA_NEGOCIO.MAPEO;
using CommandLine;

namespace BusinessLogic.IAModule.AgentsModules
{
    // Este modelo SOLO representa las decisiones del planificador.
    public class FileInstruction
    {
        public string Path { get; set; } = "";

        public string Action { get; set; } = "create_file";
    }

    public class FilePlan
    {
        public string Action { get; set; } = "final";

        public List<FileInstruction> Files { get; set; } = new();

        public string? ReadPath { get; set; }

        public string? Answer { get; set; }

        public const string PlanSchema = """
        {
          "type": "object",
          "properties": {
            "action": {
              "type": "string",
              "enum": ["create_files", "read_file", "final"]
            },
            "files": {
              "type": "array",
              "items": {
                "type": "object",
                "properties": {
                  "path": { "type": "string" },
                  "action": {
                    "type": "string",
                    "enum": [
                      "create_file",
                      "overwrite_file",
                      "edit_file"
                    ]
                  }
                },
                "required": ["path", "action"],
                "additionalProperties": false
              }
            },
            "readPath": {
              "type": ["string", "null"]
            },
            "answer": {
              "type": ["string", "null"]
            }
          },
          "required": [
            "action",
            "files",
            "readPath",
            "answer"
          ],
          "additionalProperties": false
        }
        """;
        public static FilePlan Parse(string json)
        {
            // Limpiar posibles bloques Markdown
            json = CleanJsonResponse(json);

            if (string.IsNullOrWhiteSpace(json))
                throw new AgentValidationException(
                    "El modelo devolvió una respuesta vacía.");

            FilePlan? plan;

            try
            {
                plan = JsonSerializer.Deserialize<FilePlan>(
                    json,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
            }
            catch (JsonException ex)
            {
                throw new AgentValidationException(
                    $"El modelo devolvió un JSON inválido: {ex.Message}");
            }

            if (plan == null)
                throw new AgentValidationException(
                    "El modelo devolvió un plan vacío.");

            if (!Validate(plan, out var error))
                throw new AgentValidationException(error);

            return plan;
        }
        private static string CleanJsonResponse(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return string.Empty;

            content = content.Trim();

            // Eliminar apertura de bloque Markdown: ```json o ```
            if (content.StartsWith("```"))
            {
                int firstNewLine = content.IndexOf('\n');

                if (firstNewLine >= 0)
                    content = content[(firstNewLine + 1)..];

                // Eliminar cierre de bloque Markdown
                int lastFence = content.LastIndexOf("```");

                if (lastFence >= 0)
                    content = content[..lastFence];
            }

            return content.Trim();
        }
        private static bool Validate(
            FilePlan plan,
            out string? error)
        {
            error = null;

            if (plan.Action is not (
                "create_files" or
                "read_file" or
                "final"))
            {
                error = $"Acción no permitida: {plan.Action}";
                return false;
            }

            switch (plan.Action)
            {
                case "final":
                    if (plan.Files.Count > 0)
                    {
                        error = "Una acción final no debe contener archivos.";
                        return false;
                    }

                    return true;

                case "read_file":
                    if (string.IsNullOrWhiteSpace(plan.ReadPath))
                    {
                        error = "read_file requiere readPath.";
                        return false;
                    }

                    if (plan.Files.Count > 0)
                    {
                        error = "read_file no debe contener archivos.";
                        return false;
                    }

                    return true;

                case "create_files":

                    if (plan.Files == null || plan.Files.Count == 0)
                    {
                        error = "create_files requiere archivos.";
                        return false;
                    }

                    var paths = new HashSet<string>(
                        StringComparer.OrdinalIgnoreCase);

                    foreach (var file in plan.Files)
                    {
                        if (string.IsNullOrWhiteSpace(file.Path))
                        {
                            error = "Un archivo no tiene ruta.";
                            return false;
                        }

                        if (string.IsNullOrWhiteSpace(file.Action))
                        {
                            error = $"{file.Path}: falta action.";
                            return false;
                        }

                        var normalized = file.Path.Replace('\\', '/');

                        if (System.IO.Path.IsPathRooted(file.Path) ||
                            normalized.Split('/').Any(x => x == "..") ||
                            normalized.Contains(':'))
                        {
                            error = $"Ruta no permitida: {file.Path}";
                            return false;
                        }

                        if (!paths.Add(normalized))
                        {
                            error = $"Ruta duplicada: {file.Path}";
                            return false;
                        }

                        if (file.Action is not (
                            "create_file" or
                            "overwrite_file" or
                            "edit_file"))
                        {
                            error = $"Acción no permitida para {file.Path}: {file.Action}";
                            return false;
                        }
                    }

                    return true;
            }

            return false;
        }
    }



    public static class FilePlanApplier
    {
        public static FilePlanResult ApplyGenerated(
            FilePlan plan,
            IReadOnlyList<GeneratedFile> generatedFiles,
            AgentWorkspaceService ws,
            SkillDefinition skill)
        {
            var result = new FilePlanResult();

            if (plan.Action != "create_files")
                throw new AgentValidationException(
                    "El plan no contiene una operación create_files.");

            if (generatedFiles == null || generatedFiles.Count == 0)
                throw new AgentValidationException(
                    "No hay archivos generados para aplicar.");

            if (generatedFiles.Count != plan.Files.Count)
                throw new AgentValidationException(
                    "La cantidad de archivos generados no coincide con el plan.");

            if (generatedFiles.Count > skill.MaxFilesPerPlan)
                throw new AgentValidationException(
                    $"El plan excede {skill.MaxFilesPerPlan} archivos.");

            // Comprobar que cada archivo generado corresponde
            // exactamente con una instrucción del plan.
            var planByPath = plan.Files.ToDictionary(
                x => NormalizePath(x.Path),
                StringComparer.OrdinalIgnoreCase);

            var generatedPaths = new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

            long totalBytes = 0;

            foreach (var generated in generatedFiles)
            {
                var path = NormalizePath(generated.Path);

                if (!generatedPaths.Add(path))
                    throw new AgentValidationException(
                        $"Archivo generado duplicado: {generated.Path}");

                if (!planByPath.TryGetValue(path, out var instruction))
                    throw new AgentValidationException(
                        $"El archivo {generated.Path} no pertenece al plan.");

                if (instruction.Action != generated.Action)
                    throw new AgentValidationException(
                        $"La acción de {generated.Path} no coincide con el plan.");

                if (string.IsNullOrWhiteSpace(generated.Content))
                    throw new AgentValidationException(
                        $"El archivo {generated.Path} tiene contenido vacío.");

                totalBytes += Encoding.UTF8.GetByteCount(generated.Content);
            }

            if (totalBytes > skill.MaxTotalContentBytes)
                throw new AgentValidationException(
                    $"El contenido excede el límite de " +
                    $"{skill.MaxTotalContentBytes / 1024} KB.");

            var applied = new List<string>();
            var failed = new List<string>();

            foreach (var generated in generatedFiles)
            {
                try
                {
                    var rel = NormalizePath(generated.Path);

                    // El workspace valida la ubicación final.
                    var full = ws.ResolveSafePath(rel);

                    switch (generated.Action)
                    {
                        case "create_file":

                            if (File.Exists(full))
                                throw new AgentValidationException(
                                    $"{rel} ya existe. Usa overwrite_file o edit_file.");

                            WriteAtomic(full, generated.Content);

                            ws.RegisterFile(
                                rel,
                                "AGENT",
                                Encoding.UTF8.GetByteCount(generated.Content));

                            applied.Add($"+ {rel}");
                            break;

                        case "overwrite_file":

                            if (File.Exists(full))
                            {
                                var current = File.ReadAllText(full);

                                if (current == generated.Content)
                                {
                                    applied.Add(
                                        $"= {rel} (sin cambios, contenido idéntico)");

                                    break;
                                }
                            }

                            WriteAtomic(full, generated.Content);

                            ws.RegisterFile(
                                rel,
                                "AGENT",
                                Encoding.UTF8.GetByteCount(generated.Content));

                            applied.Add($"~ {rel}");
                            break;

                        case "edit_file":

                            if (!File.Exists(full))
                                throw new AgentValidationException(
                                    $"{rel} no existe. No se puede modificar.");

                            // El generador entrega el archivo completo actualizado.
                            WriteAtomic(full, generated.Content);

                            ws.RegisterFile(
                                rel,
                                "AGENT",
                                Encoding.UTF8.GetByteCount(generated.Content));

                            applied.Add($"✎ {rel}");
                            break;

                        default:
                            throw new AgentValidationException(
                                $"Acción no reconocida: {generated.Action}");
                    }
                }
                catch (Exception ex)
                {
                    failed.Add($"✗ {generated.Path}: {ex.Message}");

                    result.Trace.Add(new
                    {
                        path = generated.Path,
                        action = generated.Action,
                        error = ex.Message
                    });
                }
            }

            result.Status =
                failed.Count == 0
                    ? "OK"
                    : applied.Count > 0
                        ? "PARTIAL"
                        : "ERROR";

            result.Summary = BuildSummary(
                applied,
                failed,
                plan.Answer);

            result.Trace.AddRange(
                applied.Select(x => new { path = x }));

            return result;
        }

        private static string NormalizePath(string path)
        {
            return path.Replace('\\', '/').Trim();
        }

        private static void WriteAtomic(string full, string content)
        {
            var directory = Path.GetDirectoryName(full)
                ?? throw new InvalidOperationException(
                    "Directorio inválido.");

            Directory.CreateDirectory(directory);

            var tmp = Path.Combine(
                directory,
                $".{Path.GetFileName(full)}.{Guid.NewGuid():N}.tmp");

            try
            {
                File.WriteAllText(
                    tmp,
                    content,
                    new UTF8Encoding(false));

                File.Move(tmp, full, overwrite: true);
            }
            finally
            {
                if (File.Exists(tmp))
                    File.Delete(tmp);
            }
        }

        private static string BuildSummary(
            List<string> applied,
            List<string> failed,
            string? answer)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(answer))
                sb.AppendLine(answer);

            if (applied.Count > 0)
            {
                sb.AppendLine("\nArchivos creados/modificados:");

                foreach (var item in applied)
                    sb.AppendLine($"- {item}");
            }

            if (failed.Count > 0)
            {
                sb.AppendLine("\nErrores:");

                foreach (var item in failed)
                    sb.AppendLine($"- {item}");
            }

            return sb.ToString().TrimEnd();
        }
    }
    public class FilePlanResult
    {
        public string Status { get; set; } = "OK";
        public List<object> Trace { get; set; } = new();
        public string Summary { get; set; } = "";
    }
}

