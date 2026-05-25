//@ts-check

import { ModelFiles } from "../../WDevCore/WModules/CommonModel.js";
import { EntityClass } from "../../WDevCore/WModules/EntityClass.js";
class NotificationRequest extends EntityClass {
    /** @param {Partial<NotificationRequest>} [props] */
    constructor(props) {
        super(props, 'Notificaciones');
        Object.assign(this, props);
    }
   /**@type {Number?}*/ Id = null;
   /**@type {String?}*/ Titulo = null;
   /**@type {String?}*/ Mensaje = null;
   /**@type {Array<ModelFiles>?}*/ Files = null;
   /**@type {String?}*/ NotificationType = null;
   /**@type {Array<Number>}*/ Usuarios = [];
   /**@type {Array<Number>}*/ Dependencias = [];
   /**@type {Array<String>}*/ ToAdress = []; 
   /**@type {String?}*/ NotificationsServicesEnum = null;
   /**@type {Array<NotificactionDestinatarios>?}*/  Destinatarios = null;
}

export { NotificationRequest }

export class NotificactionDestinatarios {
    /**@type {String?} */ Correo = null;
    /**@type {String?} */ Telefono = null;
    /**@type {Object.<string, any>?} */ NotificationData = null;
}