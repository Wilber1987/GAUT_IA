using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using APPCORE;
using APPCORE.SystemConfig;
using BusinessLogic.IA.Models;
using CAPA_NEGOCIO.SystemConfig;

namespace BusinessLogic.IA.Providers
{
    /// <summary>
    /// Proveedor OpenAI — API cloud oficial.
    ///
    /// Modelos recomendados:
    ///   • Embeddings : text-embedding-3-small  (1536 dims, muy económico)
    ///                  text-embedding-3-large  (3072 dims, máxima calidad)
    ///   • Completion : gpt-4o-mini (rápido/económico) | gpt-4o (máxima calidad)
    ///
    /// Configuración en AppSettings (sección "IAServices"):
    ///   "OpenAI_ApiKey"            → "sk-..."
    ///   "OpenAI_EmbeddingModel"    → "text-embedding-3-small"
    ///   "OpenAI_CompletionModel"   → "gpt-4o-mini"
    /// </summary>
    public class OpenAIProvider : IIAProvider
    {
        private const string BaseUrl = "https://api.openai.com/v1";

        private readonly string _apiKey;
        private readonly string _embeddingModel;
        private readonly string _completionModel;

        public IAProvider ProviderType => IAProvider.OpenAI;

        public OpenAIProvider()
        {
            _apiKey          = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "OpenAI_ApiKey")
                               ?? throw new InvalidOperationException("OpenAI_ApiKey no está configurada.");
            _embeddingModel  = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "OpenAI_EmbeddingModel")  ?? "text-embedding-3-small";
            _completionModel = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "OpenAI_CompletionModel") ?? "gpt-4o-mini";
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            using var client = CreateClient();
            var response = await client.PostAsJsonAsync($"{BaseUrl}/embeddings", new
            {
                model = _embeddingModel,
                input = text
            });
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OpenAIEmbeddingResponse>();
            return result?.data?.FirstOrDefault()?.embedding ?? Array.Empty<float>();
        }

        public async Task<string?> GetCompletionAsync(string prompt)
        {
            using var client = CreateClient();
            var response = await client.PostAsJsonAsync($"{BaseUrl}/chat/completions", new
            {
                model    = _completionModel,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.7
            });
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OpenAIChatResponse>();
            return result?.choices?.FirstOrDefault()?.message?.content;
        }

        private HttpClient CreateClient()
        {
            var client = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
            return client;
        }
    }
}
