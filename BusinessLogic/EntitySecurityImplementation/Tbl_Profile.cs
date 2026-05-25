using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using APPCORE;
using APPCORE.Security;
using APPCORE.Services;
using CAPA_NEGOCIO.SystemConfig;

namespace CAPA_NEGOCIO.MAPEO
{
	public class Tbl_Profile : APPCORE.Security.Tbl_Profile
	{
		public static Tbl_Profile? GetUserProfile(string identity)
		{
			return new Tbl_Profile() { IdUser = AuthNetCore.User(identity).UserId }.Find<Tbl_Profile>();
		}
		public int? Id_Pais_Origen { get; set; }
		public int? Id_Institucion { get; set; }
		public string? Indice_H { get; set; }

		public string? ORCID { get; set; }
		public Security_Users? Security_Users { get; set; }

		[OneToMany(TableName = "Tbl_Dependencias_Usuarios", KeyColumn = "Id_Perfil", ForeignKeyColumn = "Id_Perfil")]
		public List<Tbl_Dependencias_Usuarios>? Tbl_Dependencias_Usuarios { get; set; }

		public List<Cat_Dependencias?>? Cat_Dependencias { get; set; }
		public Object? TakeProfile()
		{
			try
			{
				return this.Find<Tbl_Profile>();
			}
			catch (Exception)
			{

				throw;
			}
		}
		public Object Postularse()
		{
			try
			{
				this.Estado = "POSTULANTE";
				SaveProfile();
				return true;
			}
			catch (Exception) { return false; }

		}
		public Object SaveProfile()
		{
			try
			{
				BeginGlobalTransaction();
				if (Foto != null)
				{
					ModelFiles? pic = (ModelFiles?)FileService.upload(SystemConfigImpl.GetMediaImagePath(), new ModelFiles
					{
						Value = Foto,
						Type = "png",
						Name = "profile"
					}).body;
					Foto = pic?.Value?.Replace("wwwroot", "");

				}
				if (this.Id_Perfil == null)
				{
					this.Save();
				}
				{
					this.Update();
				}
				Correo_institucional = null;
				IdUser = null;
				CommitGlobalTransaction();
				return this;

			}
			catch (System.Exception)
			{
				RollBackGlobalTransaction();
				throw;
			}
		}

		public List<Tbl_Profile> GetProfiles(string? identity)
		{
			List<Tbl_Profile> profiles = new List<Tbl_Profile>();
			if (AuthNetCore.HavePermission(Permissions.ADMIN_ACCESS.ToString(), identity))
			{
				profiles.AddRange(Where<Tbl_Profile>(FilterData.NotNull("IdUser")));
			}
			else if (AuthNetCore.HavePermission(Permissions.PERFIL_MANAGER.ToString(), identity))
			{
				UserModel user = AuthNetCore.User(identity);
				Tbl_Profile? userProfile = new Tbl_Profile { IdUser = user.UserId }.Find<Tbl_Profile>();

			}
			return profiles;
		}


		public static Tbl_Profile Get_Profile(UserModel User)
		{
			return Get_Profile(User.UserId.GetValueOrDefault());
		}

		public static Tbl_Profile Get_Profile(int UserId)
		{
			Tbl_Profile? tbl_Profile = new Tbl_Profile { IdUser = UserId }.Find<Tbl_Profile>();
			return tbl_Profile ?? new Tbl_Profile { IdUser = UserId };
		}

	}
}