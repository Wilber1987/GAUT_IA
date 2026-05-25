using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using APPCORE;
using APPCORE.SystemConfig;
using BusinessLogic.ApiChat.AutomaticIAOllama.Model;
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

        public async Task<string?> GetCompletionAsync(string prompt)
        {
            using var client = CreateClient();
            var response = await client.PostAsJsonAsync($"{_host}/api/generate", new
            {
                model  = _completionModel,
                prompt = prompt,
                stream = false
            });
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OllamaGenerateResponse>();
            return result?.response;
        }

        private static HttpClient CreateClient() =>
            new HttpClient { Timeout = TimeSpan.FromMinutes(10) };
    }
}
