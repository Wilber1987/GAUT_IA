using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPCORE;
using CAPA_NEGOCIO;
using CAPA_NEGOCIO.MAPEO;
using DataBaseModel;
using Microsoft.AspNetCore.SignalR;
using WLLM.Hubs.MensajeriaNotificaciones;

namespace UI.Helpdesk.ApiControllers
{

    public class WSocketSignalService
    {
        private IHubContext<ChatHub> _hubContext;

        public WSocketSignalService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
       

        private void SendCaseCommentSignal(int? Id_Case, int? Id_User)
        {
            if (_hubContext != null)
            {
                try
                {
                    // Obtener la conversación
                    List<string> usuarios = [];
                   
                    foreach (var Id_usuario in usuarios?.Distinct() ?? [])
                    {
                        _hubContext.Clients?.User(Id_usuario).SendAsync("ReadSignal", new {} );//TODO
                    }
                }
                catch (Exception ex)
                {
                    LoggerServices.AddMessageError($"[SignalR] Error al notificar: {ex.Message}", ex);
                }
            }
        }

        public void SendCaseCommentSignal(UserMessage Inst)
        {
            SendCaseCommentSignal(Inst.Id_case, null);
        }
        public void SendMessageSignal(Mensajes Inst)
        {
            // Guardar el mensaje (tu lógica actual)
            if (_hubContext != null)
            {
                try
                {
                    // Obtener la conversación
                    var conversacion = new Conversacion { Id_conversacion = Inst.Id_conversacion }
                        .Find<Conversacion>();

                    // Destinatarios (todos excepto el emisor)
                    var destinatarios = conversacion?.Conversacion_usuarios?
                        .Where(cu => cu.Id_usuario != Inst.Usuario_id)
                        .ToList();

                    foreach (var cu in destinatarios ?? [])
                    {
                        var destinatarioId = cu.Id_usuario.ToString() ?? "";

                        // ✅ Enviar directamente al usuario, sin connectionId
                        _hubContext.Clients.User(destinatarioId).SendAsync("ReadSignal", Inst);

                        Console.WriteLine($"[SignalR] Enviado a usuario: {destinatarioId}");
                    }
                }
                catch (Exception ex)
                {
                    LoggerServices.AddMessageError($"[SignalR] Error al notificar: {ex.Message}", ex);
                }
            }
        }
    }
}