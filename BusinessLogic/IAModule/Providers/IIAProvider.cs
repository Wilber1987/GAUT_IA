using System.Threading.Tasks;
using BusinessLogic.IAModule.AgentsModules.Models;

namespace BusinessLogic.IA.Providers
{
    /// <summary>
    /// Contrato único que deben implementar todos los proveedores de IA.
    /// Permite intercambiar el backend sin tocar la lógica de negocio.
    /// </summary>
    public interface IIAProvider
    {
        /// <summary>Identificador del proveedor activo.</summary>
        IAProvider ProviderType { get; }

        /// <summary>
        /// Genera un vector de embeddings para el texto recibido.
        /// La dimensión del vector depende del modelo configurado en cada proveedor.
        /// </summary>
        /// <param name="text">Texto a vectorizar.</param>
        /// <returns>Array de floats (embedding).</returns>
        Task<float[]> GetEmbeddingAsync(string text);

        /// <summary>
        /// Genera una respuesta de texto a partir del prompt.
        /// </summary>
        /// <param name="prompt">Prompt completo (incluye contexto RAG si aplica).</param>
        /// <returns>Texto generado o null si hubo error.</returns>
        Task<string?> GetCompletionAsyncNormal(
            string? systemPrompt,
            List<ChatMessage> messages,
            string? jsonSchema = null,
            double temperature = 0.7,          // ← nuevo parámetro, default compatible
            int? maxTokens = null);

        Task<CompletionResult> GetCompletionAsync(
            string? systemPrompt,
            List<ChatMessage> messages,
            string? jsonSchema = null,
            double temperature = 0.7,
            int? maxTokens = null
        );
    }
}
