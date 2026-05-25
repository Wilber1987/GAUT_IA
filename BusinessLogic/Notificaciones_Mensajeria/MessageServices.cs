using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAPA_NEGOCIO;

namespace BusinessLogic.Notificaciones_Mensajeria
{
    public class MessageServices
    {
        public static ResponseWebApi ProcessMessage(UserMessage unifiedMessage)
        {
            if (unifiedMessage != null && unifiedMessage?.Text != null)
            {

                switch (unifiedMessage?.Source)
                {
                    case SourceTypeEnum.WEB_API:
                        // Procesar inmediatamente para webapi
                        var resp = EnqueueMessageWebApi(unifiedMessage, SourceTypeEnum.WEB_API);
                        ResponseWebApi reply = new ResponseWebApi()
                        {
                            Reply = resp.MessageIA,
                            WithAgentResponse = resp.WithAgentResponse ?? false,
                            ProfileName = "IA",
                            Id_Case = resp.Id_case,
                            Id_Comment = resp.Id_comment,
                        };
                        // Respuesta inmediata para webapi
                        return reply;
                    default:
                        throw new Exception("Unsupported platform source.");
                }
            }
            throw new Exception("Unsupported platform source.");
        }

        private static UserMessage EnqueueMessageWebApi(UserMessage message, SourceTypeEnum source)
		{
            message.Source = source;
			var instanceIA = new ApiChatProcessorServices();
			// Ejecutar asincronía sincrónicamente usando GetAwaiter().GetResult()
			var response = instanceIA.GenerateResponse(message).GetAwaiter().GetResult();
			//new WSocketSignalService(_hubContext).SendCaseCommentSignal(message);
			return response;
		}
    }
}