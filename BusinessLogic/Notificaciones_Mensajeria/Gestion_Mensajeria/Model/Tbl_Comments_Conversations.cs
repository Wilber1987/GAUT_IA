using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Controllers;
using APPCORE;
using CAPA_NEGOCIO.MAPEO;

namespace BusinessLogic.Notificaciones_Mensajeria.Gestion_Mensajeria.Model
{
    public enum ConversationsState
    {
        ACTIVA, ARCHIVADA, INACTIVA
    }
    public class Tbl_Comments_Conversations : EntityClass
    {
        [PrimaryKey(Identity = true)]
        public int? Id_Conversacion { get; set; }
        public ConversationsState? Estado { get; set; }
        public string? Nombre { get; set; }
        public string? Token { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Id_Perfil { get; set; } 



        public static List<Tbl_Comments_Conversations> GetConversations(string? sessionKey)
        {
            var (user, dbUser) = CAPA_NEGOCIO.MAPEO.Security_Users.GetUserData(sessionKey);
            var profile = Tbl_Profile.Get_Profile(user);
            if (profile != null)
            {
                return new Tbl_Comments_Conversations().Where<Tbl_Comments_Conversations>(
                    FilterData.Equal("Id_Perfil", profile.Id_Perfil)
                );
            }
            return [];
        }
    }

    
}