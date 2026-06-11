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
        public string? Token { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? UpdateDate { get; set; }
        public int? Id_Perfil { get; set; }

        /*Quien crea el template*/
        [ManyToOne(TableName = "Tbl_Profile", KeyColumn = "Id_Perfil", ForeignKeyColumn = "Id_Perfil")]
        public Tbl_Profile? Tbl_Profile { get; set; }
        /*Quien crea el template*/
        [JsonProp]
        public List<EditorData> EditorDataList { get; set; } = [];

        //segmentacion
        public string Year
		{
			get { return Fecha.GetValueOrDefault().Year.ToString(); }
		}
        public int? Mes { get { return Fecha.GetValueOrDefault().Month; } }

		public string Month
		{
			get
			{
				var month = Fecha.GetValueOrDefault().Month;
				return month switch
				{
					1 => "Enero",
					2 => "Febrero",
					3 => "Marzo",
					4 => "Abril",
					5 => "Mayo",
					6 => "Junio",
					7 => "Julio",
					8 => "Agosto",
					9 => "Septiembre",
					10 => "Octubre",
					11 => "Noviembre",
					12 => "Diciembre",
					_ => "Desconocido"
				};
			}
		}

        public void AddEditor(int? id_Perfil)
        {
            if (id_Perfil != null)
            {
                EditorDataList.Add(new EditorData { Id_Perfil  = id_Perfil, UpdateDate = DateTime.Now });
            }
        }
    }

    public class EditorData
    {
        public int? Id_Perfil { get; set; }
        public DateTime? UpdateDate { get; set; }
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