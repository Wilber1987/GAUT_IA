//@ts-check
import { EntityClass } from '../../WDevCore/WModules/EntityClass.js';
class Mensaje_adjuntos extends EntityClass {
   /** @param {Partial<Mensaje_adjuntos>} [props] */
   constructor(props) {
       super(props, 'MessageManager');
       Object.assign(this, props);
   }
   /**@type {Number?}*/ Id = null;
   /**@type {Number?}*/ Mensaje_id = null;
   /**@type {String?}*/ Archivo = null;
   /**@type {Date?}*/ Created_at = null;
   /**@type {Date?}*/ Updated_at = null;
}
export { Mensaje_adjuntos }
