namespace CAPA_NEGOCIO
{
	public class ResponseWebApi
	{
		public string? Reply { get; set; }
		public bool? WithAgentResponse { get; set; }
		public string? ProfileName { get; set; }
		public int? Id_Case { get; set; }
		public int? Id_Comment { get; set; }

		// ResponseWebApi
		public int? Id_Run { get; set; }
		public string? Token { get; set; }
		public AgentTokenUsage? TokenUsage { get; set; }
		public List<IAInteractions>? MessageIAResponse { get; set; } = []; // Contenido del mensaje del usuario

	}
}