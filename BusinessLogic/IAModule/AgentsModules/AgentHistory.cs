using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using APPCORE;
using BusinessLogic.IA;
using BusinessLogic.IA.Agent;
using BusinessLogic.IA.Providers;
using CAPA_NEGOCIO.MAPEO;

namespace BusinessLogic.IAModule.AgentsModules
{
    public static class AgentHistory
    {
        public static List<ChatMessage> Load(string? token, AgentWorkspaceService ws, int maxTurns = 4)
        {
            var result = new List<ChatMessage>();
            if (token == null) return result;

            if (!string.IsNullOrWhiteSpace(ws.Workspace.Resumen))
                result.Add(new ChatMessage { Role = "system", Content = "Resumen de la conversación previa: " + ws.Workspace.Resumen });

            var recent = new Tbl_Comments().Where<Tbl_Comments>(FilterData.Equal("Token", token))
                .OrderByDescending(h => h.Fecha).Take(maxTurns * 2)
                .OrderBy(h => h.Fecha).ToList();

            foreach (var h in recent)
                result.Add(new ChatMessage
                {
                    Role = h.NickName == "IA" ? "assistant" : "user",
                    Content = AgentContextBuilder.Truncate(h.Body ?? "", 800)
                });
            return result;
        }
     
    }
}