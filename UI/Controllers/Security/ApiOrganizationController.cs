using Microsoft.AspNetCore.Mvc;
using CAPA_NEGOCIO.MAPEO;
using APPCORE.Security;
using Microsoft.AspNetCore.SignalR;
//using WLLM.Hubs.MensajeriaNotificaciones;

namespace API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ApiOrganizationController : ControllerBase
	{
		//private readonly IHubContext<ChatHub> _hubContext;
        //public ApiOrganizationController(IHubContext<ChatHub> hubContext)
        //{
          // _hubContext = hubContext;
       // }		
		[HttpPost]
		[AuthController]
		public List<Cat_Cargos_Dependencias> getCat_Cargos_Dependencias(Cat_Cargos_Dependencias Inst)
		{
			return Inst.Get<Cat_Cargos_Dependencias>();
		}
		[HttpPost]
		[AuthController]
		public object? saveCat_Cargos_Dependencias(Cat_Cargos_Dependencias inst)
		{
			return inst.Save();
		}
		[HttpPost]
		[AuthController]
		public object? updateCat_Cargos_Dependencias(Cat_Cargos_Dependencias inst)
		{
			return inst.Update();
		}
		//Cat_Dependencias
		[HttpPost]
		[AuthController]
		public List<Cat_Dependencias> getCat_Dependencias(Cat_Dependencias Inst)
		{
			return Inst.GetDependencias<Cat_Dependencias>();
		}
		[HttpPost]
		[AuthController]
		public object? saveCat_Dependencias(Cat_Dependencias inst)
		{
			return inst.Save();
		}
		[HttpPost]
		[AuthController]
		public object? updateCat_Dependencias(Cat_Dependencias inst)
		{
			return inst.UpdateDependencies();
		}
		
		//Tbl_Dependencias_Usuarios
		[HttpPost]
		[AuthController]
		public List<Tbl_Dependencias_Usuarios> getTbl_Dependencias_Usuarios(Tbl_Dependencias_Usuarios Inst)
		{
			return Inst.Get<Tbl_Dependencias_Usuarios>();
		}
		[HttpPost]
		[AuthController]
		public object? saveTbl_Dependencias_Usuarios(Tbl_Dependencias_Usuarios inst)
		{
			return inst.Save();
		}
		[HttpPost]
		[AuthController]
		public object? updateTbl_Dependencias_Usuarios(Tbl_Dependencias_Usuarios inst)
		{
			return inst.Update();
		}
		
		//Tbl_Datos_Laborales
		//Cat_Paises
		[HttpPost]
		[AuthController]
		public List<Cat_Paises> getCat_Paises(Cat_Paises Inst)
		{
			return Inst.Get<Cat_Paises>();
		}
		[HttpPost]
		[AuthController]
		public object? saveCat_Paises(Cat_Paises inst)
		{
			return inst.Save();
		}
		[HttpPost]
		[AuthController]
		public object? updateCat_Paises(Cat_Paises inst)
		{
			return inst.Update();
		}
		//Cat_TipoLocalidad


		//Tbl_Profile
		[HttpPost]
		[AuthController(Permissions.PERFIL_MANAGER)]
		public List<CAPA_NEGOCIO.MAPEO.Tbl_Profile> getTbl_Profile(CAPA_NEGOCIO.MAPEO.Tbl_Profile Inst)
		{
			return Inst.GetProfiles(HttpContext.Session.GetString("sessionKey"));
		}
		[HttpPost]
		[AuthController(Permissions.PERFIL_MANAGER)]
		public object? saveTbl_Profile(CAPA_NEGOCIO.MAPEO.Tbl_Profile inst)
		{
			return inst.SaveProfile();
		}
		[HttpPost]
		[AuthController(Permissions.PERFIL_MANAGER)]
		public object? updateTbl_Profile(CAPA_NEGOCIO.MAPEO.Tbl_Profile inst)
		{
			inst.IdUser = AuthNetCore.User(HttpContext.Session.GetString("sessionKey")).UserId;
			return inst.SaveProfile();
		}

		
		[HttpPost]
		[AuthController]
		public List<Tbl_Servicios> getTbl_Servicios(Tbl_Servicios Inst)
		{
			return Inst.Get<Tbl_Servicios>();
		}
		[HttpPost]
		[AuthController]
		public object? saveTbl_Servicios(Tbl_Servicios inst)
		{
			return inst.Save();
		}
		[HttpPost]
		[AuthController]
		public object? updateTbl_Servicios(Tbl_Servicios inst)
		{
			return inst.Update();
		}

		

	}
}
