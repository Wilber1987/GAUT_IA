//@ts-check
import { Security_Users } from '../../WDevCore/Security/SecurityModel.js';
import { EntityClass } from '../../WDevCore/WModules/EntityClass.js';
import { Conversacion } from './Conversacion.js';
class Mensajes extends EntityClass {
    /** @param {Partial<Mensajes>} [props] */
    constructor(props) {
        super(props, 'MessageManager');
        Object.assign(this, props);
    }
    /**@type {Number?}*/ Id_mensaje = null;
    /**@type {String?}*/ Remitente = null;
    /**@type {String?}*/ Destinatarios = null;
    /**@type {String?}*/ Asunto = null;
    /**@type {String?}*/ Body = null;
    /**@type {Date?}*/ Created_at = null;
    /**@type {Date?}*/ Updated_at = null;
    /**@type {Boolean?}*/ Enviado = null;
    /**@type {Boolean?}*/ Leido = null;
    /**@type {Conversacion?} ManyToOne*/ Conversacion = null;
    /**@type {Security_Users?} ManyToOne*/ Security_Users = null;
 }
 export { Mensajes }
