using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BusinessLogic.IAModule.AgentsModules
{

    public static class ModelOutputParser
    {
        private static readonly Regex FenceRegex = new Regex(
            @"^```(?:json)?\s*(.*?)\s*```$",
            RegexOptions.Singleline | RegexOptions.Compiled);

        public static JsonDocument Parse(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw))
                throw new InvalidOperationException("Respuesta vacía del modelo.");

            var text = raw.Trim();

            // 1) Quitar bloques ```json ... ``` o ``` ... ```
            var fence = FenceRegex.Match(text);
            if (fence.Success)
                text = fence.Groups[1].Value.Trim();

            // 2) Intento directo
            try
            {
                return JsonDocument.Parse(text);
            }
            catch (JsonException) { /* seguimos con fallback */ }

            // 3) Fallback: extraer el primer objeto { ... } balanceado
            var start = text.IndexOf('{');
            if (start < 0)
                throw new InvalidOperationException("No se encontró JSON en la respuesta.");

            var depth = 0;
            var inString = false;
            var escape = false;

            for (var i = start; i < text.Length; i++)
            {
                var c = text[i];
                if (inString)
                {
                    if (escape) escape = false;
                    else if (c == '\\') escape = true;
                    else if (c == '"') inString = false;
                }
                else
                {
                    if (c == '"') inString = true;
                    else if (c == '{') depth++;
                    else if (c == '}')
                    {
                        depth--;
                        if (depth == 0)
                        {
                            var candidate = text.Substring(start, i - start + 1);
                            return JsonDocument.Parse(candidate);
                        }
                    }
                }
            }

            throw new InvalidOperationException("JSON incompleto o mal formado.");
        }
    }
    

    public static class JsonRepair
    {
        private static readonly Regex FenceRegex = new(
            @"^```(?:json)?\s*(.*?)\s*```$",
            RegexOptions.Singleline |
            RegexOptions.IgnoreCase |
            RegexOptions.Compiled
        );

        public static bool TryParse(
            string? raw,
            out JsonDocument? document,
            out string? repairedJson)
        {
            document = null;
            repairedJson = null;

            if (string.IsNullOrWhiteSpace(raw))
                return false;

            var text = raw.Trim();

            // 1. Eliminar Markdown
            var fence = FenceRegex.Match(text);

            if (fence.Success)
                text = fence.Groups[1].Value.Trim();

            // 2. Extraer el primer objeto JSON
            int start = text.IndexOf('{');

            if (start < 0)
                return false;

            text = text.Substring(start);

            // 3. Intentar parseo original
            if (TryParseDocument(text, out document))
            {
                repairedJson = text;
                return true;
            }

            // 4. Reparación léxica
            var repaired = RepairStructure(text);

            if (repaired == null)
                return false;

            // 5. Limpiar comas finales
            repaired = RemoveTrailingCommas(repaired);

            // 6. Intentar nuevamente
            if (TryParseDocument(repaired, out document))
            {
                repairedJson = repaired;
                return true;
            }

            return false;
        }

        private static bool TryParseDocument(
            string json,
            out JsonDocument? document)
        {
            try
            {
                document = JsonDocument.Parse(json);
                return true;
            }
            catch (JsonException)
            {
                document = null;
                return false;
            }
        }

        private static string? RepairStructure(string text)
        {
            var stack = new Stack<char>();

            bool inString = false;
            bool escape = false;

            var result = new System.Text.StringBuilder();

            foreach (char c in text)
            {
                result.Append(c);

                if (inString)
                {
                    if (escape)
                    {
                        escape = false;
                    }
                    else if (c == '\\')
                    {
                        escape = true;
                    }
                    else if (c == '"')
                    {
                        inString = false;
                    }

                    continue;
                }

                if (c == '"')
                {
                    inString = true;
                }
                else if (c == '{')
                {
                    stack.Push('}');
                }
                else if (c == '[')
                {
                    stack.Push(']');
                }
                else if (c == '}' || c == ']')
                {
                    if (stack.Count == 0 || stack.Pop() != c)
                        return null;
                }
            }

            // 7. Cerrar string incompleto
            if (inString)
            {
                // Si termina con una barra invertida,
                // eliminarla porque escapa un carácter inexistente.
                if (escape)
                    result.Length--;

                result.Append('"');
            }

            // 8. Cerrar objetos y arrays pendientes
            while (stack.Count > 0)
            {
                result.Append(stack.Pop());
            }

            return result.ToString();
        }

        private static string RemoveTrailingCommas(string json)
        {
            var result = new System.Text.StringBuilder();

            bool inString = false;
            bool escape = false;

            for (int i = 0; i < json.Length; i++)
            {
                char c = json[i];

                if (inString)
                {
                    result.Append(c);

                    if (escape)
                        escape = false;
                    else if (c == '\\')
                        escape = true;
                    else if (c == '"')
                        inString = false;

                    continue;
                }

                if (c == '"')
                {
                    inString = true;
                    result.Append(c);
                    continue;
                }

                if (c == ',')
                {
                    int next = i + 1;

                    while (next < json.Length &&
                           char.IsWhiteSpace(json[next]))
                        next++;

                    if (next < json.Length &&
                        (json[next] == '}' || json[next] == ']'))
                        continue;
                }

                result.Append(c);
            }

            return result.ToString();
        }
    }

}