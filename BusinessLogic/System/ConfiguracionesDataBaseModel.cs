using API.Controllers;
using APPCORE;
namespace DataBaseModel
{
	public class Transactional_Configuraciones : EntityClass
	{
		[PrimaryKey(Identity = true)]
		public int? Id_Configuracion { get; set; }
		public string? Nombre { get; set; }
		public string? Descripcion { get; set; }
		public string? Valor { get; set; }
		public string? Tipo_Configuracion { get; set; }
		public Transactional_Configuraciones? GetConfig(string prop)
		{
			Nombre = prop;
			return Find<Transactional_Configuraciones>();
		}
		public List<Transactional_Configuraciones> GetTheme()
		{
			return Get<Transactional_Configuraciones>()
				.Where(x => x.Tipo_Configuracion != null &&
				 x.Tipo_Configuracion.Equals(ConfiguracionesTypeEnum.THEME.ToString())).ToList();
		}
		public List<Transactional_Configuraciones> GetTypeNumbers()
		{
			return Get<Transactional_Configuraciones>()
				.Where(x => x.Tipo_Configuracion != null &&
				 x.Tipo_Configuracion.Equals(ConfiguracionesTypeEnum.NUMBER.ToString())).ToList();
		}

		public List<Transactional_Configuraciones> GetGeneralData()
		{
			return Get<Transactional_Configuraciones>()
			   .Where(x => x.Tipo_Configuracion != null &&
				x.Tipo_Configuracion.Equals(ConfiguracionesTypeEnum.GENERAL_DATA.ToString())).ToList();
		}

		public object? UpdateConfig(string? identity)
		{
			if (!AuthNetCore.HavePermission(APPCORE.Security.Permissions.ADMIN_ACCESS.ToString(), identity))
			{
				throw new Exception("no tienes permisos para configurar la aplicación");
			}
			return this.Update();
		}
		public Transactional_Configuraciones? GetParam(ConfiguracionesThemeEnum prop, string defaultValor = "", ConfiguracionesTypeEnum TYPE = ConfiguracionesTypeEnum.THEME)
		{
			Nombre = prop.ToString();

			var find = Find<Transactional_Configuraciones>();
			if (find == null)
			{

				Valor = defaultValor;
				Descripcion = prop.ToString();
				Nombre = prop.ToString();
				Tipo_Configuracion = TYPE.ToString();
				find = (Transactional_Configuraciones?)Save();
			}
			return find;
		}

		public static void InitDefaultParams()
		{
			Transactional_Configuraciones transactional_Configuraciones = new Transactional_Configuraciones();
			//PIE DE PAGINA DE LAS BOLETAS QUE SE ENVIAN POR CORREO
			transactional_Configuraciones.GetParam(ConfiguracionesThemeEnum.BOLETA_FOOTER, "", ConfiguracionesTypeEnum.TEMPLATE);
		}

		public static Transactional_Configuraciones? GetBoletaFooter()
		{
			return new Transactional_Configuraciones().GetParam(ConfiguracionesThemeEnum.BOLETA_FOOTER, "", ConfiguracionesTypeEnum.TEMPLATE);
		}
		
		public static Transactional_Configuraciones? GetActualizacionFooter()
        {
            return new Transactional_Configuraciones().GetParam(ConfiguracionesThemeEnum.ACTUALIZACION_FOOTER, "", ConfiguracionesTypeEnum.TEMPLATE);
        }

		public static bool GetBooleanParam(ConfiguracionesThemeEnum configuracionesThemeEnum)
		{
			try
			{
				return Convert.ToBoolean(new Transactional_Configuraciones()
					.GetParam(configuracionesThemeEnum, "false", ConfiguracionesTypeEnum.GENERAL_DATA));
			}
			catch (System.Exception)
			{				
				return false;
			}
			
		}
	}
	public class PageConfig
	{
		public PageConfig()
		{
			configuraciones = new Transactional_Configuraciones().Get<Transactional_Configuraciones>();
			TITULO = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.TITULO.ToString()))?.Valor ?? TITULO;
				
			SUB_TITULO = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.SUB_TITULO.ToString()))?.Valor ?? SUB_TITULO;
				
			SUB_TITULO2 = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.SUB_TITULO2.ToString()))?.Valor ?? SUB_TITULO2;
				
			NOMBRE_EMPRESA = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.NOMBRE_EMPRESA.ToString()))?.Valor ?? NOMBRE_EMPRESA;
				
			LOGO_PRINCIPAL = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.LOGO_PRINCIPAL.ToString()))?.Valor ?? LOGO_PRINCIPAL;
				
			MEDIA_IMG_PATH = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.MEDIA_IMG_PATH.ToString()))?.Valor ?? MEDIA_IMG_PATH;
				
			VERSION = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.VERSION.ToString()))?.Valor ?? VERSION;
				
			WATHERMARK = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.WATHERMARK.ToString()))?.Valor ?? VERSION;
				
			URL_BASE = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.URL_BASE.ToString()))?.Valor ?? URL_BASE;
				
			FECHA_VENCIMIENTO_BOLETAS_ESTUDIANTES = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.FECHA_VENCIMIENTO_BOLETAS_ESTUDIANTES.ToString()))?.Valor ?? URL_BASE;
				
			RUC = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.RUC.ToString()))?.Valor ?? RUC;


		}
		public string TITULO = "TEMPLATE";
		public string SUB_TITULO = "Template";
		public string SUB_TITULO2 = "Template";
		public string NOMBRE_EMPRESA = "TEMPLATE";
		public string LOGO_PRINCIPAL = "logo.png";
		public string MEDIA_IMG_PATH = "/media/img/";
		public string VERSION = "2024.07";
		public string URL_BASE = "https://portal.ni-alpha.cloud/Security/Login";
		public string FECHA_VENCIMIENTO_BOLETAS_ESTUDIANTES = "N/D";
		public List<Transactional_Configuraciones> configuraciones = new List<Transactional_Configuraciones>();

		public string WATHERMARK = "logo.png";

		public string RUC { get;  set; } = "0000000000000";

		internal static double? GetTasaCambio(string? moneda)
		{
			//TODO: Implementar el servicio de tasa de cambio
			return 36.10;
		}
	}

	public enum ConfiguracionesTypeEnum
	{
		THEME, GENERAL_DATA, NUMBER,
        BOOLEAN,
        TEMPLATE
    }

	public enum ConfiguracionesThemeEnum
	{
		TITULO, SUB_TITULO, NOMBRE_EMPRESA, LOGO_PRINCIPAL, LOGO, MEDIA_IMG_PATH,
		VERSION,
		WATHERMARK,
		TWILLIO_ACCOUNT,
		TWILLIO_TOKEN,
		TWILLIO_NUMBER,
		SUB_TITULO2,
		URL_BASE,
		FECHA_VENCIMIENTO_BOLETAS_ESTUDIANTES,
		RUC,
		ENVIO_NOTIFICACIONES_ACTIVO,
		BOLETA_FOOTER,
		ACTUALIZACION_FOOTER,
        IS_UPDATE_PROCESS_ACTIVE,
        BOLETIN_INACTIVE,
        BOLETIN_INACTIVE_MESSAGE,
        UPDATE_YEAR_ACTIVE,
        MEDIA_ATTACH_PATH,
        IS_ACTIVE_LLM_TRAINING,
        AUTOMATIC_SENDER_REPORT,
        BLACK_LIST
    }



	public class TwillioConfig
	{
		public string TWILLIO_ACCOUNT = "";
		public string TWILLIO_TOKEN = "";
		public string TWILLIO_NUMBER = "";
		public List<Transactional_Configuraciones> configuraciones = new List<Transactional_Configuraciones>();


		public TwillioConfig()
		{
			configuraciones = new Transactional_Configuraciones().Get<Transactional_Configuraciones>();

			TWILLIO_ACCOUNT = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.TWILLIO_ACCOUNT.ToString()))?.Valor ?? TWILLIO_ACCOUNT;
			TWILLIO_TOKEN = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.TWILLIO_TOKEN.ToString()))?.Valor ?? TWILLIO_TOKEN;
			TWILLIO_NUMBER = configuraciones.Find(c => c.Nombre != null &&
				c.Nombre.Equals(ConfiguracionesThemeEnum.TWILLIO_NUMBER.ToString()))?.Valor ?? TWILLIO_NUMBER;


		}

		public static TwillioConfig getTwillioConfig()
		{
			return new TwillioConfig();
		}
	}

}
