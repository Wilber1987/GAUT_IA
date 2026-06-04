using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPCORE;
using APPCORE.SystemConfig;
using APPCORE.BDCore.Abstracts;
using Microsoft.Extensions.Configuration;
using CAPA_NEGOCIO.SystemConfig;

namespace BusinessLogic.Connection
{
	public class BDConnection
	{
		public BDConnection()
		{
			var configuration = SystemConfigImpl.AppConfiguration();
			SqlCredentials = configuration.GetSection("SQLCredentials");
			
		}
		//  public WDataMapper? DataMapper = SqlADOConexion.BuildDataMapper("localhost", "sa", "zaxscd", "IPS5Db");
		public WDataMapper? DataMapperSeguimiento { get; set; }
		public IConfigurationSection SqlCredentials { get; private set; }
		public bool InitMainConnection(bool isDebug = false)
		{
			if (isDebug)
			{
				string machineName = Environment.MachineName;				
				switch (machineName)
				{
					//case "WILBER":	return SqlADOConexion.IniciarConexion("sa", "zaxscd", "localhost", "GAUT_IA_PROD");
					case "WILBER":	return SqlADOConexion.IniciarConexion("sa", "zaxscd", "localhost", "GAUT_IA");
				}
			}
			//CONEXIONES DE PRODUCCION
			return SqlADOConexion.IniciarConexion(
				SqlCredentials["User"],
				SqlCredentials["Password"],
				SqlCredentials["Server"],
				SqlCredentials["Database"]
			);//SIASMOP USAV
		}
	}
}