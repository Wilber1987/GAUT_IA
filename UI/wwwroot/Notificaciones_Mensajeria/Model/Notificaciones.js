//@ts-check

//@ts-ignore
import { ModelProperty } from "../../WDevCore/WModules/CommonModel.js";
import { ModelFiles } from "../../WDevCore/WModules/CommonModel.js";
import { EntityClass } from "../../WDevCore/WModules/EntityClass.js";
import { DateTime } from "../../WDevCore/WModules/Types/DateTime.js";
class Notificaciones extends EntityClass {
	/** @param {Partial<Notificaciones>} [props] */
	constructor(props) {
		super(props, 'Notificaciones');
		Object.assign(this, props);
	}
	/**@type {Number?}*/ Id = null;
	/**@type {String?}*/ Titulo = null;
	/**@type {String?}*/ Mensaje = null;
	/**@type {Date?}*/ Fecha = null;
	/**@type {Date?}*/ Fecha_Envio = null;
	/**@type {Array<ModelFiles>}*/ Media = [];
	/**@type {boolean?}*/ Enviado = null;
	/**@type {boolean?}*/ Leido = null;
	/**@type {string?}*/ Tipo = null;
	/**@type {string?}*/ Month = null;
	/**@type {string?}*/ Year = null;
	/**@type {string?}*/ Telefono = null;
   	/**@type {NotificationData?}*/ NotificationData = null;
	/**@type {Array<String>}*/ NotificationsService = [];

	//get Month() {return new DateTime(this.Fecha).getMonthFormatEs();}
	//get Year() {return new DateTime(this.Fecha).getFullYear();}
	MarcarComoLeido = async () => {
		return await this.GetData("ApiNotificaciones/MarcarComoLeido");
	}
	/**
	 * @param {string} value
	 */
	GetParam(value) {
		return this.NotificationData?.Params?.find(p => p.Name?.toLowerCase() == value.toLowerCase())?.Value ?? "";
	}
}
export { Notificaciones }

export class NotificationData {
	/**@type {String?} */Departamento = null;
	/**@type {String?} */Direccion = null;
	/**@type {String?} */Destinatario = null;
	/**@type {String?} */Identificacion = null;
	/**@type {String?} */Correlativo = null;
	/**@type {String?} */Fecha = null;
	/**@type {String?} */Municipio = null;
	/**@type {String?} */Agencia = null;
	/**@type {String?} */Correo = null;
	/**@type {String?} */Telefono = null;
	/**@type {String?} */Dpi = null;
	/**@type {String?} */Nit = null;
	/**@type {String?} */NumeroPaquete = null;
	/**@type {String?} */NumeroAduana = null;
	/**@type {Number?}*/ Reenvios = null;
	/**@type {Array<NotificationsParams>?} */  Params = null;
}

export class NotificationsParams {
	/**@type {String?} */ Name = null;
	/**@type {String?} */ Type = null;
	/**@type {String?} */ Value = null;
}