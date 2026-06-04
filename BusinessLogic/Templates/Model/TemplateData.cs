using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPCORE;
using CAPA_NEGOCIO.MAPEO;

namespace CAPA_NEGOCIO.Templates.Model
{
    public class TemplateData : EntityClass
    {
        [PrimaryKey(Identity = true)]
        public int? Id_Template { get; set; }
        public TemplatesDataType? Descripcion { get; set; }

        [JsonProp]
        public List<Section>? Sections { get; set; }
        public string? Token { get;  set; }
        public DateTime? Fecha { get;  set; }
        public int? Id_Perfil { get; set; }

        [ManyToOne(TableName = "Tbl_Profile", KeyColumn = "Id_Perfil", ForeignKeyColumn = "Id_Perfil")]
		public  Tbl_Profile? Tbl_Profile { get; set; }

    }

    public class Section
    {
        public int? Id_Section { get; set; }
        public string? Data { get; set; }
        public string? Body { get; set; }
    }

    
    public enum TemplatesDataType
    {
        CONTRATO_ACTUALIZACION, PROCESO_DE_SOLICITUD
    }
}