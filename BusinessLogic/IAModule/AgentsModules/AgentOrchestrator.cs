using APPCORE;
using BusinessLogic.ApiChat.AutomaticIA.TrainingModule;
using BusinessLogic.ApiChat.AutomaticIAOllama.Model;
using BusinessLogic.IA.Providers;
using BusinessLogic.IAModule.AgentsModules;
using BusinessLogic.IAModule.AgentsModules.Formatters;
using CAPA_NEGOCIO;
using CAPA_NEGOCIO.MAPEO;
using System.Text;
using System.Text.Json;

namespace BusinessLogic.IA.Agent
{
    public class AgentOrchestrator
    {
        private readonly IIAProvider _provider;
        private readonly SkillRegistry _skills;

        private readonly PlanFormatter _planFormatter = new();
        private readonly ContentFormatter _contentFormatter = new();

        public AgentOrchestrator() : this(IAProviderFactory.Create(), SkillRegistry.Default) { }
        public AgentOrchestrator(IIAProvider provider, SkillRegistry skills)
        {
            _provider = provider;
            _skills = skills;
        }




        public async Task<UserMessage> Run(UserMessage request)
        {
            var start = DateTime.Now;
            string status = "OK";
            string? error = null;
            bool runRecorded = false;

            try
            {
                var ws = AgentWorkspaceService.ForToken(
                    request.Token,
                    request.UserId);

                var skill = _skills.Resolve(request.Skill);

                var history = AgentHistory.Load(
                    request.Token,
                    ws);

                var rag = skill.UseRag
                    ? await BuildRagAsync(request.Text)
                    : null;

                var messages = AgentContextBuilder.Build(
                    skill,
                    ws,
                    history,
                    rag,
                    request.Text);

                // ============================================================
                // FASE 1: PLANIFICACIÓN Y LECTURA DE ARCHIVOS
                // ============================================================

                const int maxPlanningSteps = 5;

                AgentTokenUsageTracker traker = new AgentTokenUsageTracker();
                
                FilePlan? plan = null;
                bool planningCompleted = false;

                for (int step = 0; step < maxPlanningSteps; step++)
                {
                    plan = await GeneratePlanAsync(
                        request.Text,
                        messages,
                        traker,
                        request.MessageIAResponse);

                    if (plan == null)
                    {
                        status = "INVALID_PLAN";

                        request.MessageIA =
                            "No pude obtener un plan válido del modelo. " +
                            "La respuesta pudo estar vacía, ser inválida o estar truncada.";

                        return request;
                    }

                    // El modelo ha terminado.
                    if (plan.Action == "final")
                    {
                        request.MessageIA =
                            string.IsNullOrWhiteSpace(plan.Answer)
                                ? "Listo."
                                : plan.Answer;

                        return request;
                    }

                    // El modelo necesita leer un archivo.
                    if (plan.Action == "read_file")
                    {
                        var content = TryRead(
                            ws,
                            plan.ReadPath!);

                        messages.Add(new ChatMessage
                        {
                            Role = "user",
                            Content = content == null
                                ? $"""
                           RESULTADO DE LECTURA:

                           El archivo '{plan.ReadPath}' no existe
                           o no se pudo leer.

                           Continúa con la información disponible.
                           Puedes solicitar otra lectura o emitir
                           el plan final.
                           """
                                : $"""
                           CONTENIDO DEL ARCHIVO: {plan.ReadPath}

                           {AgentContextBuilder.Truncate(content, 12000)}

                           Utiliza este contenido para decidir
                           las siguientes operaciones.
                           """
                        });
                        request.MessageIAResponse.Add(new CAPA_NEGOCIO.IAInteractions
                        {
                            Type = CAPA_NEGOCIO.IAInteractionsTypes.ACTION,
                            Action = "read_file",
                            Path = plan.ReadPath,
                            Status = "completed",
                            Response = $"Leyendo archivo: {plan.ReadPath}"
                        });

                        continue;
                    }

                    if (plan.Action == "create_files")
                    {
                        planningCompleted = true;
                        break;
                    }

                    status = "INVALID_PLAN";
                    request.MessageIA = $"Acción no reconocida: {plan.Action}";
                    return request;
                }

                if (!planningCompleted || plan == null)
                {
                    status = "MAX_PLANNING_STEPS";

                    request.MessageIA =
                        "El modelo alcanzó el máximo de pasos de planificación. " +
                        "Divide la solicitud o simplifica las lecturas necesarias.";

                    return request;
                }

                // ============================================================
                // FASE 2: VALIDAR EL PLAN
                // ============================================================

                if (plan.Files.Count == 0)
                {
                    status = "INVALID_PLAN";

                    request.MessageIA =
                        "El modelo no especificó ningún archivo para generar.";

                    return request;
                }

                if (plan.Files.Count > skill.MaxFilesPerPlan)
                {
                    status = "INVALID_PLAN";

                    request.MessageIA =
                        $"El plan excede el límite de {skill.MaxFilesPerPlan} archivos.";

                    return request;
                }

                // ============================================================
                // FASE 3: GENERACIÓN INDIVIDUAL Y ESCRITURA
                // ============================================================

                var applied = new List<string>();
                var failed = new List<string>();

                foreach (var file in plan.Files)
                {
                    try
                    {
                        // Validar la ruta antes de llamar al modelo.
                        var rel = file.Path.Replace('\\', '/').Trim();

                        var full = ws.ResolveSafePath(rel);

                        var exists = File.Exists(full);

                        if (file.Action == "create_file" && exists)
                        {
                            throw new AgentValidationException(
                                $"{rel} ya existe. Utiliza overwrite_file o edit_file.");
                        }

                        if (file.Action == "edit_file" && !exists)
                        {
                            throw new AgentValidationException(
                                $"{rel} no existe. No se puede modificar.");
                        }

                        if (file.Action is not (
                            "create_file" or
                            "overwrite_file" or
                            "edit_file"))
                        {
                            throw new AgentValidationException(
                                $"Acción no permitida: {file.Action}");
                        }

                        // Leer el archivo actual solamente cuando es una edición.
                        string? existingContent = null;

                        if (file.Action == "edit_file")
                        {
                            existingContent = File.ReadAllText(full);
                        }
                        request.MessageIAResponse?.Add(new CAPA_NEGOCIO.IAInteractions
                        {
                            Type = CAPA_NEGOCIO.IAInteractionsTypes.ACTION,
                            Action = file.Action,
                            Path = file.Path,
                            Status = "completed",
                            Response = $"Contenido generado: {file.Path}"
                        });

                        // Generar un archivo individual.
                        var generated = await GenerateFileContentAsync(
                            userRequest: request.Text,
                            instruction: file,
                            contextMessages: messages,
                            existingContent: existingContent,
                            maxTokens: skill.MaxTokens,
                            tokenTracker: traker,
                            interactions: request.MessageIAResponse
                        );

                        request.MessageIAResponse.Add(new CAPA_NEGOCIO.IAInteractions
                        {
                            Type = CAPA_NEGOCIO.IAInteractionsTypes.ACTION,
                            Action = "write_file",
                            Path = generated?.Path,
                            Status = "completed",
                            Response = $"Archivo guardado: {generated?.Path}"
                        });


                        if (generated == null)
                        {
                            throw new AgentValidationException(
                                $"No se pudo generar el archivo {rel}.");
                        }

                        // Aplicar exclusivamente mediante C#.
                        var singleFilePlan = new FilePlan
                        {
                            Action = "create_files",
                            Answer = null,
                            Files = new List<FileInstruction>
                            {
                                new FileInstruction
                                {
                                    Path = rel,
                                    Action = file.Action
                                }
                            }
                        };

                        var result = FilePlanApplier.ApplyGenerated(
                            singleFilePlan,
                            new List<GeneratedFile> { generated },
                            ws,
                            skill
                        );

                        if (result.Status != "OK")
                        {
                            throw new AgentValidationException(
                                result.Summary);
                        }

                        applied.Add(rel);

                        // Informar al contexto de las siguientes generaciones.
                        messages.Add(new ChatMessage
                        {
                            Role = "user",
                            Content = $"""
                        ARCHIVO GENERADO Y APLICADO:

                        {rel}

                        El archivo ya está disponible en el workspace.
                        Tenlo en cuenta al generar los siguientes archivos.
                        """
                        });
                    }
                    catch (Exception ex)
                    {
                        failed.Add($"{file.Path}: {ex.Message}");

                        LoggerServices.AddMessageError(
                            $"AGENT-FILE-FAILED: {file.Path}",
                            ex);
                    }
                }

                // ============================================================
                // FASE 4: RESUMEN
                // ============================================================

                if (failed.Count == 0)
                {
                    status = "OK";
                }
                else if (applied.Count > 0)
                {
                    status = "PARTIAL";
                }
                else
                {
                    status = "ERROR";
                }

                var summary = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(plan.Answer))
                    summary.AppendLine(plan.Answer);

                if (applied.Count > 0)
                {
                    summary.AppendLine();
                    summary.AppendLine("Archivos creados/modificados:");

                    foreach (var path in applied)
                        summary.AppendLine($"- {path}");
                }

                if (failed.Count > 0)
                {
                    summary.AppendLine();
                    summary.AppendLine("Archivos que no pudieron procesarse:");

                    foreach (var item in failed)
                        summary.AppendLine($"- {item}");
                }

                request.MessageIA = summary.ToString().Trim();

                request.IsWithIaResponse = applied.Count > 0;

                // ============================================================
                // FASE 5: REGISTRO DE EJECUCIÓN
                // ============================================================

                new Tbl_Agent_Run
                {
                    Token = request.Token,
                    Skill = skill.Name,
                    Status = status,
                    Steps = plan.Files.Count,
                    Trace = JsonSerializer.Serialize(new
                    {
                        Applied = applied,
                        Failed = failed
                    }),
                    StartedAt = start,
                    EndedAt = DateTime.Now
                }.Save();

                runRecorded = true;

                
                request.TokenUsage = traker.GetSummary();
                return request;
            }
            catch (OperationCanceledException)
            {
                status = "TIMEOUT";
                error = "Timeout del agente.";

                request.MessageIA =
                    "El agente tardó demasiado. Reduce la petición o divídela.";

                return request;
            }
            catch (Exception ex)
            {
                status = "ERROR";
                error = ex.Message;

                LoggerServices.AddMessageError(
                    "AGENT-RUN-ERROR",
                    ex);

                request.MessageIA = $"Error del agente: {ex.Message}";

                return request;
            }
            finally
            {
                if (!runRecorded)
                {
                    try
                    {
                        new Tbl_Agent_Run
                        {
                            Token = request.Token,
                            Status = status,
                            Error = error,
                            StartedAt = start,
                            EndedAt = DateTime.Now
                        }.Save();
                    }
                    catch
                    {
                        // No sustituir el resultado del agente
                        // por un error secundario de registro.
                    }
                }
            }
        }

        private async Task<GeneratedFile?> GenerateFileContentAsync(
            string userRequest,
            FileInstruction instruction,
            List<ChatMessage> contextMessages,
            AgentTokenUsageTracker tokenTracker,
            List<CAPA_NEGOCIO.IAInteractions> interactions,
            string? existingContent = null,
            string? additionalContext = null,
            int? maxTokens = null)
        {
            try
            {
                var systemPrompt = _contentFormatter.GetSystemPrompt();

                var userPrompt = _contentFormatter.BuildUserPrompt(
                    originalRequest: userRequest,
                    filePath: instruction.Path,
                    fileAction: instruction.Action,
                    existingContent: existingContent,
                    additionalContext: additionalContext
                );

                // Contexto independiente para generar este archivo.
                var generationMessages = new List<ChatMessage>();

                foreach (var message in contextMessages)
                {
                    if (!string.Equals(
                        message.Role,
                        "system",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        generationMessages.Add(new ChatMessage
                        {
                            Role = message.Role,
                            Content = message.Content
                        });
                    }
                }

                generationMessages.Add(new ChatMessage
                {
                    Role = "user",
                    Content = userPrompt
                });

                var completion = await _provider.GetCompletionAsync(
                    systemPrompt,
                    generationMessages,
                    jsonSchema: null,
                    temperature: 0.2,
                    maxTokens: maxTokens ?? 8192
                );




                if (completion == null ||
                    string.IsNullOrWhiteSpace(completion.Content))
                {
                    throw new AgentValidationException(
                        $"El modelo devolvió contenido vacío para {instruction.Path}.");
                }

                if (completion.IsTruncated)
                {
                    throw new AgentValidationException(
                        $"El contenido de {instruction.Path} quedó truncado. " +
                        $"Tokens generados: {completion.CompletionTokens}. " +
                        "El archivo no será escrito.");
                }

                var cleaned = ContentResponseCleaner.Clean(completion.Content);

                if (string.IsNullOrWhiteSpace(cleaned.Content))
                {
                    throw new AgentValidationException(
                        $"El modelo no generó contenido válido para: {instruction.Path}");
                }

                tokenTracker.Add(
                    completion.PromptTokens,
                    completion.CompletionTokens);

                interactions.Add(new CAPA_NEGOCIO.IAInteractions
                {
                    Type = CAPA_NEGOCIO.IAInteractionsTypes.TOKEN_USAGE,
                    Response = "Consumo de tokens: planificación",
                    Action = "plan",
                    Status = "completed",
                    InputTokens = completion.PromptTokens,
                    OutputTokens = completion.CompletionTokens,
                    Razonamiento = completion.ReasoningContent,
                    TotalTokens =
                        (completion.PromptTokens ?? 0) +
                        (completion.CompletionTokens ?? 0)
                });

                return new GeneratedFile
                {
                    Path = instruction.Path,
                    Action = instruction.Action,
                    Content = cleaned.Content
                };
            }
            catch (Exception ex)
            {
                LoggerServices.AddMessageError(
                    $"AGENT-GENERATE-CONTENT-ERROR: {instruction.Path}",
                    ex
                );

                throw;
            }
        }


        private static string BuildFileGenerationPrompt(
            string originalRequest,
            FileInstruction file,
            string? currentContent)
        {
            var actionDescription = file.Action switch
            {
                "create_file" => "Crear un archivo nuevo.",
                "overwrite_file" => "Generar el contenido completo para reemplazar un archivo.",
                "edit_file" => "Modificar el archivo existente y devolver su contenido completo actualizado.",
                _ => "Generar el contenido del archivo."
            };

            var sb = new StringBuilder();

            sb.AppendLine("Eres un generador de archivos dentro de un agente de programación.");
            sb.AppendLine();
            sb.AppendLine("SOLICITUD ORIGINAL:");
            sb.AppendLine(originalRequest);
            sb.AppendLine();

            sb.AppendLine("OPERACIÓN:");
            sb.AppendLine(actionDescription);
            sb.AppendLine();

            sb.AppendLine("ARCHIVO:");
            sb.AppendLine(file.Path);
            sb.AppendLine();

            if (currentContent != null)
            {
                sb.AppendLine("CONTENIDO ACTUAL DEL ARCHIVO:");
                sb.AppendLine("----- INICIO DEL ARCHIVO -----");
                sb.AppendLine(currentContent);
                sb.AppendLine("----- FIN DEL ARCHIVO -----");
                sb.AppendLine();

                sb.AppendLine("""
            Debes devolver el archivo completo con los cambios solicitados.
            Conserva las partes existentes que no necesitan modificarse.
            No devuelvas solamente el fragmento modificado.
            """);
            }

            sb.AppendLine();
            sb.AppendLine("""
                REGLAS OBLIGATORIAS:
                - Devuelve únicamente el contenido del archivo.
                - No incluyas explicaciones antes ni después.
                - No uses bloques Markdown ni fences de código.
                - No incluyas JSON.
                - No omitas secciones necesarias.
                - Respeta la extensión y el lenguaje del archivo.
                - Mantén coherencia con la solicitud original y el contexto del proyecto.
                - No generes otros archivos.
                """);

            return sb.ToString();
        }
        private static string? TryRead(AgentWorkspaceService ws, string? rel)
        {
            if (string.IsNullOrWhiteSpace(rel)) return null;
            try
            {
                var norm = rel.Replace('\\', '/').TrimStart('/');
                var full = ws.ResolveSafePath(norm);
                return File.Exists(full) ? File.ReadAllText(full) : null;
            }
            catch { return null; }
        }

        private async Task<string> BuildRagAsync(string? text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            var vector = await _provider.GetEmbeddingAsync(text);
            var matches = new Tbl_Knowledge_Base().Get<Tbl_Knowledge_Base>()
                .Select(k => new
                {
                    k.Content_Text,
                    Score = VectorHelper.CosineSimilarity(vector, VectorHelper.ByteArrayToFloatArray(k.Vector_Data))
                })
                .Where(x => x.Score > 0.75)
                .OrderByDescending(x => x.Score)
                .Take(6)
                .ToList();
            return string.Join("\n", matches.Select(m => m.Content_Text));
        }



        private async Task<FilePlan?> GeneratePlanAsync(
            string userRequest,
            List<ChatMessage> messages,
            AgentTokenUsageTracker tokenTracker,
            List<CAPA_NEGOCIO.IAInteractions> interactions)
        {
            try
            {
                var systemPrompt = _planFormatter.GetSystemPrompt();

                var planningMessages = new List<ChatMessage>();

                // Conservamos el contexto, pero no arrastramos los mensajes
                // system antiguos que podrían contener instrucciones anteriores.
                foreach (var message in messages)
                {
                    if (!string.Equals(
                        message.Role,
                        "system",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        planningMessages.Add(new ChatMessage
                        {
                            Role = message.Role,
                            Content = message.Content
                        });
                    }
                }

                // Reforzamos la instrucción de planificación.
                planningMessages.Add(new ChatMessage
                {
                    Role = "user",
                    Content = _planFormatter.BuildUserPrompt(userRequest)
                });

                var completion = await _provider.GetCompletionAsync(
                    systemPrompt,
                    planningMessages,
                    FilePlan.PlanSchema,
                    temperature: 0.2,
                    maxTokens: 32000
                );
                tokenTracker.Add(completion.PromptTokens,
                    completion.CompletionTokens);

                interactions.Add(new CAPA_NEGOCIO.IAInteractions
                {
                    Type = CAPA_NEGOCIO.IAInteractionsTypes.TOKEN_USAGE,
                    Response = "Consumo de tokens: planificación",
                    Action = "plan",
                    Status = "completed",
                    InputTokens = completion.PromptTokens,
                    OutputTokens = completion.CompletionTokens,
                    Razonamiento = completion.ReasoningContent,
                    TotalTokens =
                        (completion.PromptTokens ?? 0) +
                        (completion.CompletionTokens ?? 0)
                });


                if (completion == null ||
                    string.IsNullOrWhiteSpace(completion.Content))
                {
                    LoggerServices.AddMessageError(
                        "AGENT-PLAN-EMPTY",
                        new InvalidOperationException(
                            "El planificador devolvió una respuesta vacía.")
                    );

                    return null;
                }

                // Nunca interpretar JSON truncado.
                if (completion.IsTruncated)
                {
                    LoggerServices.AddMessageError(
                        "AGENT-PLAN-TRUNCATED",
                        new InvalidOperationException(
                            $"El planificador alcanzó el límite de tokens. " +
                            $"Tokens generados: {completion.CompletionTokens}")
                    );

                    return null;
                }

                var plan = FilePlan.Parse(completion.Content);

                if (plan == null)
                    return null;

                // Conservamos la respuesta real del asistente para que
                // el siguiente paso de lectura mantenga la conversación.
                messages.Add(new ChatMessage
                {
                    Role = "assistant",
                    Content = completion.Content
                });

                return plan;
            }
            catch (Exception ex)
            {
                LoggerServices.AddMessageError(
                    "AGENT-GENERATE-PLAN-ERROR",
                    ex
                );

                return null;
            }
        }


    }
}