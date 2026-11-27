using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using APPCORE;
using APPCORE.SystemConfig;
using BusinessLogic.ApiChat.AutomaticIAOllama.Model;
using BusinessLogic.IAModule.AgentsModules.Models;
using CAPA_NEGOCIO.SystemConfig;

namespace BusinessLogic.IA.Providers
{
    /// <summary>
    /// Proveedor Ollama — inferencia 100 % local.
    /// Modelos recomendados:
    ///   • Embeddings : nomic-embed-text (768 dims)
    ///   • Completion : llama3.1:8b | mistral-nemo
    /// </summary>
    public class OllamaProvider : IIAProvider
    {
        // ── Modelos por defecto (sobreescribibles desde config) ──────────────
        private readonly string _embeddingModel;
        private readonly string _completionModel;
        private readonly string _host;

        public IAProvider ProviderType => IAProvider.Ollama;

        public OllamaProvider()
        {
            _host           = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "IAHost");
            _embeddingModel = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "Ollama_EmbeddingModel")  ?? "nomic-embed-text";
            _completionModel= SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "Ollama_CompletionModel") ?? "llama3.1:8b";
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            using var client = CreateClient();
            var response = await client.PostAsJsonAsync($"{_host}/api/embeddings", new
            {
                model  = _embeddingModel,
                prompt = text
            });
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OllamaEmbeddingResponse>();
            return result?.embedding ?? Array.Empty<float>();
        }

       public async Task<string?> GetCompletionAsync(string systemPrompt, List<ChatMessage> messages)
        {
            using var client = CreateClient();

            // Ollama /api/chat espera role: "system" | "user" | "assistant"
            var ollamaMessages = new List<object>();

            if (!string.IsNullOrWhiteSpace(systemPrompt))
            {
                ollamaMessages.Add(new { role = "system", content = systemPrompt });
            }

            ollamaMessages.AddRange(messages.Select(m => (object)new
            {
                role = m.Role,       // "user" o "assistant"
                content = m.Content
            }));

            var response = await client.PostAsJsonAsync($"{_host}/api/chat", new
            {
                model    = _completionModel,
                messages = ollamaMessages,
                stream   = false
            });
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<OllamaGenerateResponse>();
            return result?.message?.content;
        }

        private static HttpClient CreateClient() =>
            new HttpClient { Timeout = TimeSpan.FromMinutes(10) };

        public Task<string?> GetCompletionAsynca(string? systemPrompt, List<ChatMessage> messages, string? jsonSchema = null, double temperature = 0.7, int? maxTokens = null)
        {
            throw new NotImplementedException();
        }

        Task<CompletionResult> IIAProvider.GetCompletionAsync(string? systemPrompt, List<ChatMessage> messages, string? jsonSchema, double temperature, int? maxTokens)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetCompletionAsyncNormal(string? systemPrompt, List<ChatMessage> messages, string? jsonSchema = null, double temperature = 0.7, int? maxTokens = null)
        {
            throw new NotImplementedException();
        }

    }
}
