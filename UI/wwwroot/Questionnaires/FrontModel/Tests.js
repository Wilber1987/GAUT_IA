//@ts-check
//@ts-ignore
import { EntityClass } from "../../WDevCore/WModules/EntityClass.js";
import { Cat_Categorias_Test } from './Cat_Categorias_Test.js';
import { Pregunta_Tests } from './Pregunta_Tests.js';
import { Resultados_Tests } from './Resultados_Tests.js';

class Tests extends EntityClass {
    /** @param {Partial<Tests>} [props] */
    constructor(props) {
        super(props, 'EntityQuestionnaires');
        Object.assign(this, props);
    }
   /**@type {Number?}*/ Id_test = null;
   /**@type {String?}*/ Titulo = null;
   /**@type {String?}*/ Descripcion = null;
   /**@type {Number?}*/ Grado = null;
   /**@type {String?}*/ Tipo_test = null;
   /**@type {String?}*/ Estado = null;
   /**@type {Date?}*/ Fecha_publicacion = null;
   /**@type {Date?}*/ Created_at = null;
   /**@type {Date?}*/ Updated_at = null;
   /**@type {String?}*/ Referencias = null;
   /**@type {Number?}*/ Tiempo = null;
   /**@type {Number?}*/ Caducidad = null;
   /**@type {String?}*/ Color = null;
   /**@type {String?}*/ Image = null;
   /**@type {Cat_Categorias_Test?} ManyToOne*/ Cat_Categorias_Test = null;
   /**@type {Array<Pregunta_Tests>} OneToMany*/ Pregunta_Tests = [];
   /**@type {Array<Resultados_Tests>} OneToMany*/ Resultados_Tests = [];
    SaveResultado() {
       return this.SaveData("QuestionnairesTransactions/SaveResultado", this);
    }
}
export { Tests };


