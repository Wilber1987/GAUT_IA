namespace BusinessLogic.IA.Providers
{
    /// <summary>
    /// Proveedores de IA soportados por el sistema.
    /// Configurable desde AppSettings vía IAProviderFactory.
    /// </summary>
    public enum IAProvider
    {
        /// <summary>Ollama local (llama3.1, mistral, nomic-embed-text, etc.)</summary>
        Ollama = 0,

        /// <summary>LM Studio local — expone API compatible con OpenAI en puerto 1234</summary>
        LmStudio = 1,

        /// <summary>OpenAI Cloud (GPT-4o, text-embedding-3-small, etc.)</summary>
        OpenAI = 2,

        /// <summary>Qwen (Alibaba Cloud) vía API externa</summary>
        Qwen = 3
    }
}
