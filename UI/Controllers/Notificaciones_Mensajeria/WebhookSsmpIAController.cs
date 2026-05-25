using System.Text.Json;
using API.Controllers;
using APPCORE;
using BusinessLogic.Notificaciones_Mensajeria;
using CAPA_NEGOCIO;
using CAPA_NEGOCIO.SystemConfig;
using DataBaseModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using UI.Helpdesk.ApiControllers;
using WLLM.Hubs.MensajeriaNotificaciones;


namespace UI.SSMP_IA.ApiControllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WebhookSsmpIAController : ControllerBase
	{
		//private IHubContext<ChatHub> _hubContext;

		// public WebhookSsmpIAController(IHubContext<ChatHub> hubContext)
		// {
		// 	//_hubContext = hubContext;
		// }
		private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(100); // Limitar a 10 tareas simultáneas

		[HttpPost]
		public async Task<IActionResult> ReceiveMessage(UserMessage unifiedMessage)
		{
			try
            {
                return Ok(MessageServices.ProcessMessage(unifiedMessage));
            }
            catch (Exception ex)
			{
				// Manejo de errores genérico
				Console.WriteLine($"Error processing message: {ex}");
				LoggerServices.AddMessageError($"ERROR: procesando mensaje", ex);
				return StatusCode(500, "Internal Server Error");
			}
		}     

		[HttpGet]
		public IActionResult VerifyToken()
		{
			try
			{
				string? AccessToken = SystemConfigImpl.AppConfigurationValue(APPCORE.SystemConfig.AppConfigurationList.MettaApi, "AppToken");
				var token = Request.Query["hub.verify_token"].ToString();
				var challenge = Request.Query["hub.challenge"].ToString();
				if (challenge != null && token != null && token == AccessToken)
				{
					return Ok(challenge);
				}
				else
				{
					return BadRequest();
				}
			}
			catch (System.Exception ex)
			{
				LoggerServices.AddMessageError($"ERROR: Verificando token", ex);
				throw;
			}

		}

	}
}
