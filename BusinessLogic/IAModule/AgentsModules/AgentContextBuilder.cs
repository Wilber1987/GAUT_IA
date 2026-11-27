using System.Text;
using BusinessLogic.ApiChat.AutomaticIAOllama.Model;
using CAPA_NEGOCIO.MAPEO;

namespace BusinessLogic.IA.Agent
{
    public static class AgentContextBuilder
    {
        private const string BaseRules = """
            Eres un agente constructor de archivos. C# aplica tu plan; tú solo decides.

            Respondes EXACTAMENTE un objeto JSON con UNO de estos tres formatos:

            A) Si necesitas leer un archivo antes de decidir (opcional; el contenido ya
            suele estar en el contexto de abajo):
            {"action": "read_file", "readPath": "ruta/relativa.md"}

            B) Si vas a crear, sobreescribir o editar archivos:
            {
            "action": "create_files",
            "files": [
                {"path": "index.html",    "action": "create_file",    "content": "<!DOCTYPE html>..."},
                {"path": "css/style.css", "action": "overwrite_file", "content": "body {...}"},
                {"path": "js/app.js",     "action": "edit_file",      "oldText": "...", "newText": "..."}
            ],
            "answer": "Resumen en una frase."
            }

            C) Si solo vas a responder al usuario:
            {"action": "final", "answer": "Tu respuesta."}

            IMPORTANTE: Devuelve ÚNICAMENTE el objeto JSON. No uses bloques markdown,
                no uses ```json, no añadas texto antes ni después del JSON.

            Reglas:
            1. read_file es opcional. El contenido de USUARIO y AGENTE ya está abajo. No leas archivos que ya ves completos. Solo lee si necesitas la parte truncada.
            2. Rutas relativas al workspace, sin '/' inicial, sin '..'.
            3. create_file falla si el archivo existe. Usa overwrite_file o edit_file.
            4. edit_file requiere oldText ÚNICO y exacto tal como aparece en el contexto.
            5. C# aplica el plan completo. Si un archivo falla, C# reporta el error al usuario en el chat. NO intentas arreglarlo en una segunda respuesta.
            6. Sin texto fuera del JSON. Sin markdown alrededor. Sin explicaciones.
            7. Cada archivo lleva SOLO: path, action, y content (o oldText/newText si es edit_file). NO añadas campos como "create_file": true ni duplicados. Sin campos extra.
            """;

        public static List<ChatMessage> Build(
            SkillDefinition skill,
            AgentWorkspaceService ws,
            List<ChatMessage> history,
            string? rag,
            string? userText)
                {
                    var messages = new List<ChatMessage>
            {
                new() { Role = "system", Content = BaseRules + "\n\n" + skill.Instructions }
            };

            var ctx = new StringBuilder();
            ctx.AppendLine("## WORKSPACE");
            ctx.AppendLine(ws.BuildSnapshot());

            ctx.AppendLine("## CONTENIDO ARCHIVOS DE USUARIO (ya incluido; read_file opcional)");
            var userFiles = ws.GetFiles().Where(f => f.Origin == "USER").Take(10).ToList();
            if (userFiles.Count == 0) ctx.AppendLine("(ninguno)");
            foreach (var f in userFiles)
            {
                var full = ws.ResolveSafePath(f.RelativePath);
                ctx.AppendLine($"### {f.RelativePath}");
                ctx.AppendLine(Truncate(File.ReadAllText(full), 1800));
            }

            if (skill.IncludeAgentFilesInContext)
            {
                ctx.AppendLine("## CONTENIDO ARCHIVOS DEL AGENTE (ya incluido; editables)");
                var agentFiles = ws.GetFiles().Where(f => f.Origin == "AGENT").Take(15).ToList();
                if (agentFiles.Count == 0) ctx.AppendLine("(ninguno)");
                foreach (var f in agentFiles)
                {
                    var full = ws.ResolveSafePath(f.RelativePath);
                    ctx.AppendLine($"### {f.RelativePath}");
                    ctx.AppendLine(Truncate(File.ReadAllText(full), 1500));
                }
            }

            // RAG restaurado
            if (!string.IsNullOrWhiteSpace(rag))
            {
                ctx.AppendLine("## CONOCIMIENTO RELEVANTE (RAG)");
                ctx.AppendLine(Truncate(rag, 2800));
            }

            messages.Add(new ChatMessage { Role = "user", Content = ctx.ToString() });
            messages.AddRange(history);
            messages.Add(new ChatMessage { Role = "user", Content = userText ?? "" });
            return messages;
        }

        public static string Truncate(string text, int maxChars)
            => text.Length <= maxChars ? text
             : text[..maxChars] + "\n…[TRUNCADO: usa read_file con fromLine/toLine para ver el resto]";
    }
}