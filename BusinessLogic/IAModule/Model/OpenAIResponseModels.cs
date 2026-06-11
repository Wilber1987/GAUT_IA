using System.Collections.Generic;

namespace BusinessLogic.IA.Models
{
    // ── Embeddings (formato OpenAI /v1/embeddings) ───────────────────────────

    public class OpenAIEmbeddingResponse
    {
        public List<EmbeddingData>? data { get; set; }
    }

    public class EmbeddingData
    {
        public float[]? embedding { get; set; }
    }

    // ── Chat Completions (formato OpenAI /v1/chat/completions) ───────────────

    public class OpenAIChatResponse
    {
        public List<ChatChoice>? choices { get; set; }
    }

    public class ChatChoice
    {
        public ChatMessage? message { get; set; }
    }

    public class ChatMessage
    {
        public string? role    { get; set; }
        public string? content { get; set; }
    }
}
