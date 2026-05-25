using System;
using APPCORE;
using APPCORE.SystemConfig;
using CAPA_NEGOCIO.SystemConfig;

namespace BusinessLogic.IA.Providers
{
    /// <summary>
    /// Resuelve el proveedor de IA activo a partir de la configuración.
    ///
    /// AppSettings — sección "IAServices":
    ///   "IAProvider" → "Ollama" | "LmStudio" | "OpenAI" | "Qwen"
    ///
    /// Si la clave no existe o el valor es inválido se usa Ollama por defecto.
    /// </summary>
    public static class IAProviderFactory
    {
        /// <summary>
        /// Devuelve la instancia correcta de <see cref="IIAProvider"/>
        /// según la configuración activa.
        /// </summary>
        public static IIAProvider Create()
        {
            var raw = SystemConfigImpl.AppConfigurationValue(
                          AppConfigurationList.IAServices, "IAProvider") ?? "Ollama";

            if (!Enum.TryParse<IAProvider>(raw, ignoreCase: true, out var provider))
                provider = IAProvider.Ollama;

            return provider switch
            {
                IAProvider.Ollama   => new OllamaProvider(),
                IAProvider.LmStudio => new LmStudioProvider(),
                IAProvider.OpenAI   => new OpenAIProvider(),
                IAProvider.Qwen     => new QwenProvider(),
                _                   => throw new NotSupportedException($"Proveedor no implementado: {provider}")
            };
        }

        /// <summary>
        /// Sobrecarga que permite forzar un proveedor específico en tiempo de ejecución
        /// (útil para tests o cambios dinámicos en panel de admin).
        /// </summary>
        public static IIAProvider Create(IAProvider provider) =>
            provider switch
            {
                IAProvider.Ollama   => new OllamaProvider(),
                IAProvider.LmStudio => new LmStudioProvider(),
                IAProvider.OpenAI   => new OpenAIProvider(),
                IAProvider.Qwen     => new QwenProvider(),
                _                   => throw new NotSupportedException($"Proveedor no implementado: {provider}")
            };
    }
}
