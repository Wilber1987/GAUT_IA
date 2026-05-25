//@ts-check
import { EntityClass } from "../../WDevCore/WModules/EntityClass.js";

class Contacto extends EntityClass {
    /** @param {Partial<Contacto>} [props] */
    constructor(props) {
        super(props, 'MessageManager');
        Object.assign(this, props);
    }
    /**@type {Number}*/ Id_User;
    /**@type {String}*/ Nombre_Completo;
    /**@type {String}*/ Foto; 
    /**@type {Number}*/ Mensajes;  
}
export {Contacto}