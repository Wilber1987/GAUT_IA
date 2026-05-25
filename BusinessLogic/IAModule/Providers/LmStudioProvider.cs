using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using APPCORE;
using APPCORE.SystemConfig;
using BusinessLogic.IA.Models;
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
            _host            = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "LmStudio_Host")            ?? "http://localhost:1234";
            _embeddingModel  = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "LmStudio_EmbeddingModel")  ?? "nomic-embed-text-v1.5";
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
        public async Task<string?> GetCompletionAsync(string prompt)
        {
            using var client = CreateClient();
            var response = await client.PostAsJsonAsync($"{_host}/v1/chat/completions", new
            {
                model    = _completionModel,
                messages = new[]
                {
                    new { role = "user", content = prompt }
                },
                temperature = 0.7,
                stream      = false
            });
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<OpenAIChatResponse>();
            return result?.choices?.FirstOrDefault()?.message?.content;
        }

        private static HttpClient CreateClient() =>
            new HttpClient { Timeout = TimeSpan.FromMinutes(10) };
    }
}
