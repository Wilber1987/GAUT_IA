using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPCORE;

namespace CAPA_NEGOCIO.Templates.Model
{
    public class TemplateData : EntityClass
    {
        [PrimaryKey(Identity = true)]
        public int? Id_Template { get; set; }
        public TemplatesDataType? Descripcion { get; set; }
        [JsonProp]
        public List<Section>? Sections { get; set; }

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