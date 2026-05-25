using System.Text;
using System.Text.RegularExpressions;
using BusinessLogic.IA;
using APPCORE;
using CAPA_NEGOCIO.MAPEO;
using DataBaseModel;
using DatabaseModelNotificaciones;
using iText.Kernel.XMP.Options;
using Newtonsoft.Json;
using CAPA_NEGOCIO.SystemConfig;
using APPCORE.SystemConfig;
using APPCORE.Services;

namespace CAPA_NEGOCIO
{
	public class ApiChatProcessorServices
	{
		public async Task<UserMessage> GenerateResponse(UserMessage question)
		{
			try
			{
				bool IsInBlackList = BlackListServices.IsInBlackList(question.UserId);
				if (IsInBlackList)
				{
					var ex = new Exception($"Usuario en lista negra {question.Source} - {question.UserId}");
					LoggerServices.AddMessageError($"ERROR:" + ex.Message, ex);
					throw ex;
				}
				bool isWithIaResponse = true;
				bool isValidProcess = true;
				if (!isValidProcess)
				{
					var ex = new Exception($"Plataforma invalida {question.Source} - {question.ServicesIdentification}");
					LoggerServices.AddMessageError($"ERROR:" + ex.Message, ex);
					throw ex;
				}
				question.Text = question.Text?.Trim();
				question.IsWithIaResponse = isWithIaResponse;
				//PROCESA EL MENSAJE Y NO AGREGA CONTESTACION DEL BOT SI ESTE NO ESTA SIENDO ATENDIDO POR NINGUN AGENTE
				//EVALUACION DE TRAKING			
				question!.Timestamp = DateTime.Now;
				#region RESPUESTAS CONTROLADAS
				if (!isWithIaResponse)
				{
					question!.WithAgentResponse = true;
					AddComment(question);
					return question;
				}
				else
				{
					//CUANDO ENTRA LA IA REALMENTE
					var IAResponse = await new IAService().Chat(question);
					AddComment(IAResponse);
					return IAResponse;
				}
				#endregion
			}
			catch (Exception ex)
			{
				LoggerServices.AddMessageError($"ERROR: GenerateResponse", ex);
				throw;
			}
		}


		private static bool IsMenuQuestion(UserMessage question)
		{
			return question?.Text?.ToUpper().Trim() == "MENU" || question?.Text?.ToLower().Trim() == "menú";
		}

		public static void AddComment(UserMessage interaction, bool isSystem = false)
		{
			ModelFiles? Response = null;
			if (interaction.Source == SourceTypeEnum.WEB_API)
			{
				Response = (ModelFiles)FileService.upload(SystemConfigImpl.GetMediaAttachPath(), interaction.Attach).body;
			}
			//interaction.Attach.Value = Response.Value;
			//interaction.Attach.Type = Response.Type;		
			Tbl_Comments? us = new Tbl_Comments()
			{
				Body = interaction.Text,
				NickName = interaction.UserId,
				Fecha = DateTime.Now,
				Estado = CommetsState.Leido,
				Mail = interaction.UserId,
				Attach_Files = Response == null ? [interaction.Attach] : [Response],
			}.Save() as Tbl_Comments;

			interaction.Id_comment = us?.Id_Comentario;
			if (interaction.MessageIA != null)
			{
				if (isSystem)
				{
					Tbl_Comments ia = new Tbl_Comments()
					{
						Body = interaction.MessageIA,
						NickName = "traking_system",
						Fecha = DateTime.Now,
						Estado = CommetsState.Leido,
						Mail = "traking_system@soporte.net",
					};
					ia.Save();
				}
				else
				{
					Tbl_Comments ia = new Tbl_Comments()
					{
						Body = interaction.MessageIA,
						NickName = "IA",
						Fecha = DateTime.Now,
						Estado = CommetsState.Leido,
						Mail = "IA@soporte.net",
					};
					ia.Save();
				}
			}
			/*else
			{

				Tbl_Comments ia = new Tbl_Comments()
				{
					Body = "Si desea regresar a las otras opciones escriba la palabra \"Menu\"",
					NickName = "IA",
					Fecha = DateTime.Now,
					Estado = CommetsState.Leido,
					Mail = "IA@soporte.net",
				};
				ia.Save();

			}*/
		}
	}

}
