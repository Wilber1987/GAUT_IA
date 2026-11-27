using System.Threading.Tasks;
using APPCORE;
using APPCORE.Services;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;

namespace CAPA_NEGOCIO
{
	public class UserMessage
	{
		public string? ServicesIdentification;
		public string? Id { get; set; } // Identificador único del mensaje
		public string? Text { get; set; } // Contenido del mensaje del usuario
		public string? MessageIA { get; set; }
				public SourceTypeEnum? Source { get; set; } // Fuente del mensaje (WebAPI, WhatsApp, Messenger)
		public DateTime? Timestamp { get; set; } // Marca de tiempo del mensaje recibido
		public string? UserId { get; set; } // Identificador único del usuario que envió el mensaje
		public string? SessionId { get; set; }// Identificador de sesión para seguir la conversación
		public string? TypeProcess { get; set; }

		public bool? WithAgentResponse { get; set; } = false;
		public int? Id_case { get; set; }
		public bool IsMetaWhatsAppApi { get { return Source == SourceTypeEnum.WHATSAPP; } }
		public bool IsMessenger { get { return Source == SourceTypeEnum.MESSENGER; } }
		public bool IsWithIaResponse { get; set; }
		public ModelFiles? Attach { get; set; }
		public int? Id_comment { get; set; }
		public string? Token { get; set; }

		public AgentTokenUsage? TokenUsage { get; set; }

		public List<IAInteractions>? MessageIAResponse { get; set; } = []; // Contenido del mensaje del usuario

		public bool AgentMode { get; set; } = false;
		public string? Skill { get; set; }
		public int? Id_Run { get; set; }
		public int? Id_Workspace { get; set; }   // opcional: adjuntar workspace existente a la conversación
	}
	public class AgentTokenUsage
	{
		public int InputTokens { get; set; }
		public int OutputTokens { get; set; }
		public int TotalTokens { get; set; }

		public int ModelCalls { get; set; }
	}

	public class IAInteractions
	{
		public IAInteractionsTypes Type { get; set; }

		// Texto para mostrar en la UI
		public string? Response { get; set; }

		// Información de la acción
		public string? Action { get; set; }
		public string? Path { get; set; }
		public string? Status { get; set; }
		public string? Razonamiento { get; set; }

		// Consumo de tokens de esta interacción
		public int? InputTokens { get; set; }
		public int? OutputTokens { get; set; }
		public int? TotalTokens { get; set; }

		// Identificador de la operación o llamada al modelo
		public string? OperationId { get; set; }

		// Duración de la operación, si está disponible
		public long? DurationMs { get; set; }
	}

	public enum IAInteractionsTypes
	{
		TEXT,
		ACTION,
		TOKEN_USAGE,
		REASONING_SUMMARY
	}

	public enum SourceTypeEnum
	{
		WHATSAPP,
		MESSENGER,
		EMAIL,
		WEB_API
	}
}
