using System.Text.Json;
using APPCORE;
using CAPA_NEGOCIO.MAPEO;

namespace BusinessLogic.IA.Agent
{
    public static class AgentRunServices
    {
        public static object GetTrace(int id, string? userId)
        {
            var run = new Tbl_Agent_Run().Find<Tbl_Agent_Run>(FilterData.Equal("Id_Run", id))
                ?? throw new AgentValidationException("Run no encontrado.");
            AgentWorkspaceService.ForToken(run.Token, userId); // valida propiedad

            return new
            {
                run.Id_Run, run.Token, run.Skill, run.Status, run.Steps,
                run.Error, run.StartedAt, run.EndedAt,
                Trace = JsonSerializer.Deserialize<JsonElement>(
                    string.IsNullOrWhiteSpace(run.Trace) ? "[]" : run.Trace)
            };
        }

        public static object ListByToken(string? token, string? userId)
        {
            AgentWorkspaceService.ForToken(token, userId); // valida propiedad
            return new Tbl_Agent_Run().Where<Tbl_Agent_Run>(FilterData.Equal("Token", token))
                .OrderByDescending(r => r.Id_Run).Take(20)
                .Select(r => new { r.Id_Run, r.Skill, r.Status, r.Steps, r.StartedAt, r.EndedAt })
                .ToList();
        }
    }
}