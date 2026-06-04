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
		public List<IAInteractions>? MessageIAResponse { get; set; } // Contenido del mensaje del usuario
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
		public string? Token { get;  set; }
	}

	public class IAInteractions
	{
		public IAInteractionsTypes Type { get; set; }
		public string? Response { get; set; }
	}

	public enum IAInteractionsTypes
	{
		TEXT
	}

	public enum SourceTypeEnum
	{
		WHATSAPP,
		MESSENGER,
		EMAIL,
		WEB_API
	}
}
