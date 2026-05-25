

using BusinessLogic.Notificaciones_Mensajeria.Gestion_Notificaciones.Operations;
using APPCORE.Cron.Jobs;
using CAPA_NEGOCIO.Gestion_Mensajes.Operations;
using DataBaseModel;
using Microsoft.Extensions.Logging;
using APPCORE.SystemConfig;
using CAPA_NEGOCIO.SystemConfig;
using APPCORE;
using BusinessLogic.ApiChat.AutomaticIA.TrainingModule;

namespace BusinessLogic.FormsIA
{
	public class MySchedulerJob : CronBackgroundJob
	{
		private readonly ILogger<MySchedulerJob> _log;

		public MySchedulerJob(CronSettings<MySchedulerJob> settings, ILogger<MySchedulerJob> log)
			: base(settings.CronExpression, settings.TimeZone)
		{
			_log = log;
		}

		protected override async Task<Task> DoWork(CancellationToken stoppingToken)
		{
			_log.LogInformation(":::::::::::Running... at {0}", DateTime.UtcNow);
			//CARGA AUTOMATICA DE CASOS
			try
			{
				await new FormsIAOperation().ProcessForm();
				await Task.Delay(60000, stoppingToken);
			}
			catch (Exception ex)
			{
				_log.LogInformation(":::::::::::ERROR... at {0}", ex);
			}

			return Task.CompletedTask;
		}
	}
	
}

