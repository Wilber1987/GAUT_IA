using APPCORE;
using APPCORE.Services;
using CAPA_NEGOCIO.MAPEO;
using DataBaseModel;
using DatabaseModelNotificaciones;
using CAPA_NEGOCIO.SystemConfig;
using CAPA_NEGOCIO.Gestion_Mensajes.Operations;

namespace BusinessLogic.Notificaciones_Mensajeria.Gestion_Notificaciones.Operations
{
	public class NotificationSenderOperation
	{
		private static bool IsMovileNumber(string? input)
		{
			if (string.IsNullOrWhiteSpace(input))
				return false;

			// Ejemplo: validar número móvil peruano (+51 opcional, empieza con 9, seguido de 8 dígitos)
			var regex = new System.Text.RegularExpressions.Regex(@"^\+?[1-9]\d{6,14}$");
			return regex.IsMatch(input.Replace(" ", ""));
		}

		internal static async Task SendNotificationsAsync()
		{

			List<Notificaciones> notificaciones = new Notificaciones()
				.Where<Notificaciones>(
					FilterData.Distinc("Enviado", true),
					FilterData.NotNull("Email")
			);
			var config = SystemConfigImpl.GetSMTPDefaultConfig();
			foreach (Notificaciones item in notificaciones)
			{
				var send = await SMTPMailServices.SendMail(config?.USERNAME,
					new List<string> { item.Email },
					item?.Titulo,
					item?.Mensaje,
					item?.Media,
					null,
					config
				);
				if (send)
				{
					try
					{
						//item.Estado = MailState.ENVIADO.ToString();
						item.Update();
					}
					catch (System.Exception ex)
					{
						LoggerServices.AddMessageError($"correo enviado, error al actualizar estado del correo", ex);
					}
				}
			}

		}
	
	}

}