using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.IAModule.AgentsModules.Formatters
{
    public class ContentFormatter
    {
        public string GetSystemPrompt()
        {
            return """
        Eres un generador profesional de archivos de software.

        Tu única responsabilidad es generar el contenido
        completo del archivo solicitado.

        REGLAS OBLIGATORIAS:

        - Devuelve exclusivamente el contenido del archivo.
        - No devuelvas JSON.
        - No expliques lo que hiciste.
        - No incluyas bloques Markdown.
        - No uses ```html, ```css, ```javascript ni otros fences.
        - No omitas secciones del archivo.
        - No utilices placeholders como:
          "resto del código", "continúa aquí" o "etc".
        - Genera código completo y funcional.
        - Respeta las tecnologías indicadas.
        - Respeta la estructura del proyecto.
        - Si estás modificando un archivo existente,
          devuelve el archivo completo actualizado.

        IMPORTANTE:
        El contenido será escrito por una aplicación C#.
        No intentes crear archivos directamente.
        """;
        }

        public string BuildUserPrompt(
            string originalRequest,
            string filePath,
            string fileAction,
            string? existingContent = null,
            string? additionalContext = null)
        {
            var prompt = $"""
                PETICIÓN ORIGINAL:

                {originalRequest}

                ARCHIVO QUE DEBES GENERAR:

                {filePath}

                OPERACIÓN:

                {fileAction}
                """;

            if (!string.IsNullOrWhiteSpace(additionalContext))
            {
                prompt += $"""

                    CONTEXTO ADICIONAL DEL PROYECTO:

                    {additionalContext}
                    """;
            }

            if (!string.IsNullOrWhiteSpace(existingContent))
            {
                prompt += $"""

                    CONTENIDO ACTUAL DEL ARCHIVO:

                    {existingContent}

                    Debes devolver el archivo completo actualizado,
                    conservando las partes que todavía sean necesarias.
                    """;
                    }

                    prompt += """

                Devuelve exclusivamente el contenido completo del archivo.
                No devuelvas JSON ni explicaciones.
                """;

            return prompt;
        }
    }
}