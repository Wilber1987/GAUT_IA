using System.Linq;
using System.Threading.Tasks;
using APPCORE;
using BusinessLogic.ApiChat.AutomaticIA.TrainingModule;
using BusinessLogic.ApiChat.AutomaticIAOllama.Model;
using BusinessLogic.IA.Providers;
using CAPA_NEGOCIO;
using CAPA_NEGOCIO.MAPEO;

namespace BusinessLogic.IA
{
    /// <summary>
    /// Servicio de IA refactorizado.
    /// Toda la lógica de negocio (RAG, ranking, prompt) vive aquí.
    /// El proveedor (Ollama, LmStudio, OpenAI, Qwen) se inyecta o se
    /// resuelve automáticamente desde la configuración.
    /// </summary>
    public class IAService
    {
        private readonly IIAProvider _provider;

        // ── Constructor por defecto: lee proveedor desde AppSettings ────────
        public IAService() : this(IAProviderFactory.Create()) { }

        // ── Constructor con inyección (tests, cambio dinámico de proveedor) ─
        public IAService(IIAProvider provider)
        {
            _provider = provider;
        }

        // ── API pública heredada (mantiene compatibilidad con código existente)

        /// <summary>Genera el vector de embeddings para el texto dado.</summary>
        public Task<float[]> GetEmbedding(string text)
            => _provider.GetEmbeddingAsync(text);

        /// <summary>Genera una respuesta fluida a partir del prompt.</summary>
        public Task<string?> GetCompletion(string prompt)
            => _provider.GetCompletionAsync(prompt);

        // ── Lógica RAG ───────────────────────────────────────────────────────

        public async Task<UserMessage> Chat(UserMessage request)
        {
            // A. Vectorizar consulta actual
            float[] queryVector = await _provider.GetEmbeddingAsync(request.Text);

            // B. Recuperar conocimiento del mismo ServiceTag
            /*var localKnowledge = new Tbl_Knowledge_Base().Where<Tbl_Knowledge_Base>(
                FilterData.Equal("ServiceTag", request.ServicesIdentification)
            );*/
            var localKnowledge = new List<Tbl_Knowledge_Base>();
            // C. Ranking de Similitud (RAG)
            var contextMatches = localKnowledge
                .Select(k => new
                {
                    k.Content_Text,
                    Score = VectorHelper.CosineSimilarity(
                                queryVector,
                                VectorHelper.ByteArrayToFloatArray(k.Vector_Data))
                })
                .Where(x => x.Score > 0.75)
                .OrderByDescending(x => x.Score)
                .Take(3)
                .ToList();

            /*if (contextMatches.Count == 0)
            {
                request.MessageIA = "No he sido capaz de procesar esta pregunta. "
                    + "Te recomiendo contactar con un agente de servicio al cliente. "
                    + "Envía '5' o 'Menu' para ver otras opciones.";
                return request;
            }*/

            // D. Construir prompt con contexto RAG
            string promptContexto = string.Join("\n", contextMatches.Select(x => x.Content_Text));
            promptContexto = "Asistente de ventas de articulos tecnologicos";
            string promptFinal = $@"Eres un asistente de ayuda. Usa este contexto para responder: {promptContexto} Pregunta del usuario: {request.Text}";

            // E. Generar respuesta fluida con el proveedor activo
            request.MessageIA = await _provider.GetCompletionAsync(promptFinal);
            request.IsWithIaResponse = true;
            return request;
        }
    }
}
