using Microsoft.AspNetCore.Mvc;
using APPCORE.Security;
using APPCORE;

namespace API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiEntitySecurityController : ControllerBase
    {
        #region SECURITY
        [HttpPost]
        [AuthController(Permissions.ADMINISTRAR_USUARIOS)]
        public Object getSecurity_Permissions(Security_Permissions inv) { return inv.Get<Security_Permissions>(); }
        [HttpPost]
        [AuthController(Permissions.ADMINISTRAR_USUARIOS)]
        public Object? getSecurity_Roles(Security_Roles inv) { return inv.GetRoles(); }
        [HttpPost]
        [AuthController(Permissions.ADMINISTRAR_USUARIOS)]
        public Object getSecurity_Users(CAPA_NEGOCIO.MAPEO.Security_Users inv) { return inv.GetUsers(); }

        [HttpPost]
        [AuthController(Permissions.ADMINISTRAR_USUARIOS)]
        public Object? saveSecurity_Permissions(Security_Permissions inv) { return inv.Save(); }
        [HttpPost]
        [AuthController(Permissions.ADMINISTRAR_USUARIOS)]
        public Object saveSecurity_Roles(Security_Roles inv) { return inv.SaveRole(); }
        [HttpPost]
        [AuthController(Permissions.ADMINISTRAR_USUARIOS)]
        public Object updateSecurity_Roles(Security_Roles inv) { return inv.SaveRole(); }
        [HttpPost]
        [AuthController(Permissions.ADMINISTRAR_USUARIOS)]
        public Object saveSecurity_Users(CAPA_NEGOCIO.MAPEO.Security_Users inv)
        {
            return inv.SaveUserT(HttpContext.Session.GetString("sessionKey"));
        }
        [HttpPost]
        [AuthController(Permissions.ADMINISTRAR_USUARIOS)]
        public Object updateSecurity_Users(CAPA_NEGOCIO.MAPEO.Security_Users inv)
        {
            return inv.SaveUserT(HttpContext.Session.GetString("sessionKey"));
        }

        [HttpPost]
        [AuthController(Permissions.ADMINISTRAR_USUARIOS)]
        public ResponseService updateSecurity_Permissions(Security_Permissions inv) { return inv.UpdatePermision(); }
        [HttpPost]
        [AuthController]
        public Object changePassword(CAPA_NEGOCIO.MAPEO.Security_Users inv) { return inv.changePassword(HttpContext.Session.GetString("sessionKey")); }

        #endregion
    }
}
