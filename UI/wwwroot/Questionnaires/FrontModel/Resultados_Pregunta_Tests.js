//@ts-check
import { EntityClass } from "../../WDevCore/WModules/EntityClass.js";
//@ts-ignore
import { Cat_Valor_Preguntas } from './Cat_Valor_Preguntas.js';
import { Pregunta_Tests } from './Pregunta_Tests.js';
class Resultados_Pregunta_Tests extends EntityClass {
    /** @param {Partial<Resultados_Pregunta_Tests>} [props] */
    constructor(props) {
        super(props, 'EntityQuestionnaires');
        Object.assign(this, props);
    }
   /**@type {Number?}*/ Id_Perfil = null;
   /**@type {Number?}*/ Resultado = null;
   /**@type {String?}*/ Respuesta = null;
   /**@type {String?}*/ Estado = null;
   /**@type {Date?}*/ Fecha = null;
   /**@type {Cat_Valor_Preguntas?} ManyToOne*/ Cat_Valor_Preguntas = null;
   /**@type {Pregunta_Tests?} ManyToOne*/ Pregunta_Tests = null;
}
export { Resultados_Pregunta_Tests };
