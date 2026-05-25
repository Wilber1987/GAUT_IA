//@ts-check
import { EntityClass } from "../../WDevCore/WModules/EntityClass.js";
//@ts-ignore
import { Resultados_Pregunta_Tests } from "./Resultados_Pregunta_Tests.js";
import { Tests } from './Tests.js';
class Resultados_Tests extends EntityClass {
    /** @param {Partial<Resultados_Tests>} [props] */
    constructor(props) {
        super(props, 'EntityQuestionnaires');
        Object.assign(this, props);
    }
   /**@type {Number?}*/ Id_Perfil = null;
   /**@type {Date?}*/ Fecha = null;
   /**@type {String?}*/ Seccion = null;
   /**@type {String?}*/ Valor = null;
   /**@type {String?}*/ Categoria_valor = null;
   /**@type {String?}*/ Tipo = null;
   /**@type {Tests?} ManyToOne*/ Tests = null;
   /**@type {String?}*/ IdToken = null;
   /**@type {Array<Resultados_Pregunta_Tests>} OneToMany*/ Resultados_Pregunta_Tests = [];
}
export { Resultados_Tests };
