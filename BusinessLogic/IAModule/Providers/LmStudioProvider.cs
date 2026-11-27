using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using APPCORE;
using APPCORE.SystemConfig;
using BusinessLogic.IA.Models;
using BusinessLogic.IAModule.AgentsModules.Models;
using CAPA_NEGOCIO.SystemConfig;

namespace BusinessLogic.IA.Providers
{
    /// <summary>
    /// Proveedor LM Studio — servidor local con API compatible con OpenAI.
    /// Puerto por defecto: 1234  →  http://localhost:1234
    /// 
    /// Modelos recomendados (cargarlos en LM Studio antes de usar):
    ///   • Embeddings : nomic-embed-text-v1.5
    ///   • Completion : cualquier GGUF cargado (p.ej. mistral-7b-instruct)
    /// 
    /// Configuración en AppSettings (sección "IAServices"):
    ///   "LmStudio_Host"            → "http://localhost:1234"
    ///   "LmStudio_EmbeddingModel"  → nombre exacto visible en LM Studio
    ///   "LmStudio_CompletionModel" → nombre exacto visible en LM Studio
    /// </summary>
    public class LmStudioProvider : IIAProvider
    {
        private readonly string _host;
        private readonly string _embeddingModel;
        private readonly string _completionModel;

        public IAProvider ProviderType => IAProvider.LmStudio;

        public LmStudioProvider()
        {
            _host = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "LmStudio_Host") ?? "http://localhost:1234";
            _embeddingModel = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "LmStudio_EmbeddingModel") ?? "nomic-embed-text-v1.5";
            _completionModel = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "LmStudio_CompletionModel") ?? "local-model";
        }

        // ── Embeddings (/v1/embeddings — estándar OpenAI) ───────────────────
        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            using var client = CreateClient();
            var response = await client.PostAsJsonAsync($"{_host}/v1/embeddings", new
            {
                model = _embeddingModel,
                input = text
            });
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OpenAIEmbeddingResponse>();
            return result?.data?.FirstOrDefault()?.embedding ?? Array.Empty<float>();
        }

        // ── Completion (/v1/chat/completions — estándar OpenAI) ─────────────
        public async Task<string?> GetCompletionAsyncNormal(
            string? systemPrompt,
            List<ChatMessage> messages,
            string? jsonSchema = null,
            double temperature = 0.7,
            int? maxTokens = null)
        {
            using var client = CreateClient();

            // Construir el array de mensajes completo
            var payload = new Dictionary<string, object?>
            {
                ["model"] = _completionModel,
                ["temperature"] = temperature,
                ["stream"] = false,
                ["messages"] = messages.Select(m => new { role = m.Role, content = m.Content }).ToList()
            };

            if (!string.IsNullOrWhiteSpace(jsonSchema))
            {
                payload["format"] = System.Text.Json.Nodes.JsonNode.Parse(jsonSchema);
            }

            if (maxTokens.HasValue)
            {
                payload["max_tokens"] = maxTokens.Value;
            }

            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_host}/v1/chat/completions", content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OpenAIChatResponse>();
            return result?.choices?.FirstOrDefault()?.message?.content;
        }

        public async Task<CompletionResult> GetCompletionAsync(
          string? systemPrompt,
          List<ChatMessage> messages,
          string? jsonSchema = null,
          double temperature = 0.7,
          int? maxTokens = null)
        {
            using var client = CreateClient();

            var finalMessages = new List<object>();

            if (!string.IsNullOrWhiteSpace(systemPrompt))
            {
                finalMessages.Add(new
                {
                    role = "system",
                    content = systemPrompt
                });
            }

            finalMessages.AddRange(
                messages.Select(m => new
                {
                    role = m.Role,
                    content = m.Content
                })
            );

            var payload = new Dictionary<string, object?>
            {
                ["model"] = _completionModel,
                ["temperature"] = temperature,
                ["stream"] = false,
                ["messages"] = finalMessages
            };

            if (!string.IsNullOrWhiteSpace(jsonSchema))
            {
                payload["format"] =
                    System.Text.Json.Nodes.JsonNode.Parse(jsonSchema);
            }

            if (maxTokens.HasValue)
            {
                payload["max_tokens"] = maxTokens.Value;
            }

            var json = System.Text.Json.JsonSerializer.Serialize(payload);

            using var content = new StringContent(
                json,
                Encoding.UTF8,
                "application/json"
            );

            using var response = await client.PostAsync(
                $"{_host}/v1/chat/completions",
                content
            );

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(responseJson);

            var root = document.RootElement;

            if (!root.TryGetProperty("choices", out var choices) ||
                choices.ValueKind != JsonValueKind.Array ||
                choices.GetArrayLength() == 0)
            {
                throw new InvalidOperationException(
                    "El proveedor no devolvió choices."
                );
            }

            var choice = choices[0];

            var message = choice.GetProperty("message");

            string? reasoningContent = null;

            if (message.TryGetProperty("reasoning_content", out var reasoningElement))
            {
                reasoningContent = reasoningElement.ValueKind == JsonValueKind.String
                    ? reasoningElement.GetString()
                    : reasoningElement.ToString();
            }

            var result = new CompletionResult
            {
                Content = message.TryGetProperty("content", out var contentElement) &&
                          contentElement.ValueKind == JsonValueKind.String
                    ? contentElement.GetString() ?? "" : "",
                ReasoningContent = reasoningContent,
                FinishReason = choice.TryGetProperty("finish_reason", out var finishElement) &&
                               finishElement.ValueKind == JsonValueKind.String
                    ? finishElement.GetString()
                    : null
            };

            if (root.TryGetProperty("usage", out var usage))
            {
                if (usage.TryGetProperty("prompt_tokens", out var promptTokens) &&
                    promptTokens.TryGetInt32(out var promptValue))
                {
                    result.PromptTokens = promptValue;
                }

                if (usage.TryGetProperty("completion_tokens", out var completionTokens) &&
                    completionTokens.TryGetInt32(out var completionValue))
                {
                    result.CompletionTokens = completionValue;
                }
            }

            return result;
        }



        private static HttpClient CreateClient() =>
            new HttpClient { Timeout = TimeSpan.FromMinutes(10) };
    }
}
