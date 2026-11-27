using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CAPA_NEGOCIO;

namespace BusinessLogic.Notificaciones_Mensajeria
{
    public class MessageServices
    {
        public static async Task<ResponseWebApi> ProcessMessage(UserMessage unifiedMessage)
        {
            if (unifiedMessage != null && unifiedMessage?.Text != null)
            {

                switch (unifiedMessage?.Source)
                {
                    case SourceTypeEnum.WEB_API:
                        // Procesar inmediatamente para webapi
                        var resp = await EnqueueMessageWebApi(unifiedMessage, SourceTypeEnum.WEB_API);
                        ResponseWebApi reply = new ResponseWebApi()
                        {
                            Reply = resp.MessageIA,
                            WithAgentResponse = resp.WithAgentResponse ?? false,
                            ProfileName = "IA",
                            Id_Case = resp.Id_case,
                            Id_Comment = resp.Id_comment,
                            Id_Run = resp.Id_Run,
                            Token = resp.Token
                        };
                        // Respuesta inmediata para webapi
                        return reply;
                    default:
                        throw new Exception("Unsupported platform source.");
                }
            }
            throw new Exception("Unsupported platform source.");
        }
        public static async Task<ResponseWebApi> ProcessMessageAsync(UserMessage unifiedMessage)
        {
            if (unifiedMessage != null && unifiedMessage?.Text != null)
            {

                switch (unifiedMessage?.Source)
                {
                    case SourceTypeEnum.WEB_API:
                        // Procesar inmediatamente para webapi
                        var resp = await EnqueueMessageWebApi(unifiedMessage, SourceTypeEnum.WEB_API);
                        ResponseWebApi reply = new ResponseWebApi()
                        {
                            
                            Reply = resp.MessageIA,
                            WithAgentResponse = resp.WithAgentResponse ?? false,
                            ProfileName = "IA",
                            Id_Case = resp.Id_case,
                            Id_Comment = resp.Id_comment,
                            Id_Run = resp.Id_Run,
                            Token = resp.Token,
                            MessageIAResponse = resp.MessageIAResponse,
                            TokenUsage = resp.TokenUsage
                        };
                        // Respuesta inmediata para webapi
                        return reply;
                    default:
                        throw new Exception("Unsupported platform source.");
                }
            }
            throw new Exception("Unsupported platform source.");
        }
        private static async Task<UserMessage> EnqueueMessageWebApi(UserMessage message, SourceTypeEnum source)
        {
            message.Source = source;
            return await new ApiChatProcessorServices().GenerateResponse(message);
        }
    }
}