//@ts-check
// @ts-ignore
import { EntityClass } from '../../WDevCore/WModules/EntityClass.js';
import { Conversacion_usuarios } from './Conversacion_usuarios.js';
import { Mensajes } from './Mensajes.js';
class Conversacion extends EntityClass {
    /** @param {Partial<Conversacion>} [props] */
    constructor(props) {
        super(props, 'MessageManager');
        Object.assign(this, props);
    }
    /**@type {Number?}*/ Id_conversacion = null;
    /**@type {String?}*/ Descripcion = null;
    /**@type {Number?}*/ MensajesPendientes = null;
    /**@type {Date?}*/ Fecha_Ultimo_Mensaje = null;
    /**@type {Array<Conversacion_usuarios>} OneToMany*/ Conversacion_usuarios = []
    /**@type {Array<Mensajes>} OneToMany*/ Mensajes = []
    /**@type {String?}*/ Nombre_Completo = null;
}
export { Conversacion }
