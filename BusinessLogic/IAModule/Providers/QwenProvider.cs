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
    /// Proveedor Qwen — Alibaba Cloud (DashScope API).
    /// Compatible con formato OpenAI desde finales de 2024.
    ///
    /// Modelos recomendados:
    ///   • Embeddings : text-embedding-v3  (1024 dims)
    ///   • Completion : qwen-plus | qwen-turbo | qwen-max
    ///
    /// Configuración en AppSettings (sección "IAServices"):
    ///   "Qwen_ApiKey"            → "sk-..."  (DashScope key)
    ///   "Qwen_EmbeddingModel"    → "text-embedding-v3"
    ///   "Qwen_CompletionModel"   → "qwen-plus"
    /// 
    /// Docs: https://help.aliyun.com/zh/dashscope/developer-reference/compatibility-of-openai-with-dashscope
    /// </summary>
    public class QwenProvider : IIAProvider
    {
        private const string BaseUrl = "https://dashscope-intl.aliyuncs.com/compatible-mode/v1";

        private readonly string _apiKey;
        private readonly string _embeddingModel;
        private readonly string _completionModel;

        public IAProvider ProviderType => IAProvider.Qwen;

        public QwenProvider()
        {
            _apiKey          = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "Qwen_ApiKey")
                               ?? throw new InvalidOperationException("Qwen_ApiKey no está configurada.");
            _embeddingModel  = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "Qwen_EmbeddingModel")  ?? "text-embedding-v3";
            _completionModel = SystemConfigImpl.AppConfigurationValue(AppConfigurationList.IAServices, "Qwen_CompletionModel") ?? "qwen-plus";
        }

        public async Task<float[]> GetEmbeddingAsync(string text)
        {
            using var client = CreateClient();
            var response = await client.PostAsJsonAsync($"{BaseUrl}/embeddings", new
            {
                model = _embeddingModel,
                input = text,
                // Qwen soporta dimension reduction — útil para alinear con vectores
                // almacenados de otros proveedores. Descomentar si lo necesitas:
                // dimensions = 1024
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
                temperature = 0.7,
                stream      = false
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
