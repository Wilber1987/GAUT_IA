// BusinessLogic/IA/Agent/AgentJsonUtils.cs
using System;

namespace BusinessLogic.IA.Agent
{
    public static class AgentJsonUtils
    {
        /// <summary>
        /// Recorre el string y devuelve el primer objeto JSON balanceado ({...}),
        /// respetando strings y escapes. Devuelve null si no hay objeto o está truncado.
        /// </summary>
        public static string? ExtractFirstJsonObject(string? raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;

            int start = raw.IndexOf('{');
            if (start < 0) return null;

            int depth = 0;
            bool inString = false;
            bool escaped = false;

            for (int i = start; i < raw.Length; i++)
            {
                char c = raw[i];

                if (inString)
                {
                    if (escaped) { escaped = false; continue; }
                    if (c == '\\') { escaped = true; continue; }
                    if (c == '"') inString = false;
                    continue;
                }

                if (c == '"') { inString = true; continue; }
                if (c == '{') depth++;
                else if (c == '}')
                {
                    depth--;
                    if (depth == 0) return raw.Substring(start, i - start + 1);
                }
            }

            // JSON truncado (depth > 0 al final del string)
            return null;
        }
    }
}