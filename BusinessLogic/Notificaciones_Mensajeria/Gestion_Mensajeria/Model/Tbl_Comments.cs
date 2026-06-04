using APPCORE;
using API.Controllers;
using APPCORE.Services;
using System.Threading.Tasks;
using CAPA_NEGOCIO.SystemConfig;

namespace CAPA_NEGOCIO.MAPEO
{
	public class Tbl_Comments : EntityClass
	{
		[PrimaryKey(Identity = true)]
		public int? Id_Comentario { get; set; }
		public CommetsState? Estado { get; set; }
		public string? NickName { get; set; }
		public string? Mail { get; set; }
		public string? Foto { get; set; }
		public string? Body { get; set; }
		[JsonProp]
		public List<ModelFiles>? Attach_Files { get; set; }
		[JsonProp]
		public List<string>? Mails { get; set; }
		public int? Id_Case { get; set; }
		public int? Id_User { get; set; }
		public DateTime? Fecha { get; set; }
        public bool? IsVectorized { get; set; }
		public string? Token { get;  set; }

        public async Task<ResponseService> SaveComment(string identity, Boolean withMail = true)
		{
			try
			{
				//BeginGlobalTransaction();
				UserModel user = AuthNetCore.User(identity);
				var profile = Tbl_Profile.Get_Profile(user);
				Fecha = DateTime.Now;
				Id_User = user.UserId;
				NickName = user.UserData?.Nombres;
				Mail = user.mail;
				Foto = profile.Foto;
				// Manejo de archivos adjuntos
				foreach (var file in Attach_Files ?? new List<ModelFiles>())
				{
					ModelFiles Response = (ModelFiles)FileService.upload(SystemConfigImpl.GetMediaAttachPath(), file).body;
					file.Value = Response.Value;
					file.Type = Response.Type;
				}
				// Guardar el comentario
				Save();
				// Enviar correo si se requiere
				if (withMail)
				{
					CreateMailForComment(user);
				}				
				// Confirmar transacción
				return new ResponseService(200, "mensaje guarado", this);
			}
			catch (System.Exception ex)
			{
				LoggerServices.AddMessageError($@"Error respondiendo enviando respuesta a la API: {ex.Message},
								 (case: {Id_Case})"
								, ex);
				return new ResponseService(500, ex.Message, ex.StackTrace, ex);
			}
		}


	

		public void CreateMailForComment(UserModel user)
		{
			List<string?> toMails = [];		
			if (Mails != null)
			{
				toMails.AddRange(Mails);
			}
			new Tbl_Mails()
			{
				Subject = $"RE: ",
				Body = Body,
				FromAdress = user.mail,
				Estado = MailState.PENDIENTE.ToString(),
				Date = DateTime.Now,
				Attach_Files = Attach_Files,
				ToAdress = toMails.Where(m => m != null && m != user.mail).ToList().Distinct().ToList()
			}.Save();
		}
		public List<Tbl_Comments> GetComments()
		{			
			return Where<Tbl_Comments>(
				FilterData.Equal("Token", this.Token)
			);	
		}
		public List<Tbl_Comments> GetCommentsByCase()
		{			
			return Where<Tbl_Comments>(
				FilterData.Equal("Id_Case", this.Id_Case)
			);	
		}
	}
	public enum CommetsState
	{
		Leido, Pendiente
	}
}
