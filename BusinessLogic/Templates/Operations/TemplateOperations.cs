using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using APPCORE;
using CAPA_NEGOCIO.Templates.Model;

namespace BusinessLogic.Templates.Operations
{
    public class TemplateOperations
    {
        public static ResponseService SaveTemplate(string? sessionKey,  TemplateData templateData)
        {
            try
            {
                UserModel user = AuthNetCore.User(sessionKey);
                var profile = CAPA_NEGOCIO.MAPEO.Tbl_Profile.Get_Profile(user);
                templateData.Id_Perfil = user.ActiveProfile?.Id_Perfil;
                templateData.Fecha = DateTime.Now;
                templateData.AddEditor(user.ActiveProfile?.Id_Perfil);
                templateData.Update();
                return new ResponseService(200, "Guardado");
            }
            catch (System.Exception ex)
            {
                return new ResponseService
                {
                    status = 500,
                    message = "Error al guardar test",
                    body = ex
                };
            }
        }
        public static ResponseService UpdateTemplate(string? sessionKey, TemplateData templateData)
        {
            try
            {
                UserModel user = AuthNetCore.User(sessionKey);
                templateData.UpdateDate = DateTime.Now;
                templateData.AddEditor(user.ActiveProfile?.Id_Perfil);
                templateData.Update();             
                return new ResponseService(200, "Acutualizar");
            }
            catch (System.Exception ex)
            {
                return new ResponseService
                {
                    status = 500,
                    message = "Error al actualizar test",
                    body = ex
                };
            }
        }
    }
}