//@ts-check
// @ts-ignore
import { Security_Users } from '../../WDevCore/Security/SecurityModel.js';
import { EntityClass } from '../../WDevCore/WModules/EntityClass.js';
import { Conversacion } from './Conversacion.js';
class Conversacion_usuarios extends EntityClass {
    /** @param {Partial<Conversacion_usuarios>} [props] */
    constructor(props) {
        super(props, 'MessageManager');
        Object.assign(this, props);
    }
    /**@type {Number?} */ Id_conversacion = null;
    /**@type {Number?} */ Id_usuario = null;
    /**@type {String?} */ Name = null;
    /**@type {String?} */ Avatar = null;
    /**@type {Conversacion?} ManyToOne*/ Conversacion = null;
    /**@type {Security_Users?} ManyToOne*/ Security_Users = null;
}
export { Conversacion_usuarios }
