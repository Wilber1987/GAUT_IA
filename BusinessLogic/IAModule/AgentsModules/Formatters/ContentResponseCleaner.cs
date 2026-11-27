using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BusinessLogic.IAModule.AgentsModules.Formatters
{
   public class ContentResponseCleaner
    {
        public class CleanResult
        {
            // Contenido que se utilizará para escribir el archivo.
            public string Content { get; set; } = "";

            // Todos los bloques encontrados, en orden.
            public List<string> SubResponses { get; set; } = new();

            // Indica si encontró bloques Markdown.
            public bool HasMarkdownBlocks { get; set; }
        }

        public static CleanResult Clean(string? response)
        {
            var result = new CleanResult();

            if (string.IsNullOrWhiteSpace(response))
                return result;

            response = response.Trim();

            // Busca bloques Markdown:
            // ```html ... ```
            // ```css ... ```
            // ```javascript ... ```
            // ``` ... ```
            var matches = Regex.Matches(
                response,
                @"```[^\r\n]*\r?\n([\s\S]*?)```"
            );

            if (matches.Count > 0)
            {
                result.HasMarkdownBlocks = true;

                foreach (Match match in matches)
                {
                    string content = match.Groups[1].Value.Trim();

                    if (!string.IsNullOrWhiteSpace(content))
                        result.SubResponses.Add(content);
                }

                // La primera respuesta será la principal.
                if (result.SubResponses.Count > 0)
                    result.Content = result.SubResponses[0];

                return result;
            }

            // Si no hay bloques Markdown, conservar todo.
            result.Content = response;
            result.SubResponses.Add(response);

            return result;
        }
    }
}