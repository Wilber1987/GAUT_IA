// @ts-check
import { ModalMessage } from "../WDevCore/WComponents/ModalMessage.js";
import { WAlertMessage } from "../WDevCore/WComponents/WAlertMessage.js";
import { WForm } from "../WDevCore/WComponents/WForm.js";
// @ts-ignore
import { ModelProperty } from "../WDevCore/WModules/CommonModel.js";
import { WArrayF } from "../WDevCore/WModules/WArrayF.js";
import { generateGUID, html } from "../WDevCore/WModules/WComponentsTools.js";
import { css } from "../WDevCore/WModules/WStyledRender.js";
import { Cat_Valor_Preguntas } from "./FrontModel/Cat_Valor_Preguntas.js";
import { Pregunta_Tests } from "./FrontModel/Pregunta_Tests.js";
import { Resultados_Pregunta_Tests } from "./FrontModel/Resultados_Pregunta_Tests.js";
import { Resultados_Tests } from "./FrontModel/Resultados_Tests.js";
import { Tests } from "./FrontModel/Tests.js";


export class TestBuilderUtil {
    /**
     * @param {Tests} test
     * @param {HTMLElement} container
     * @param {string?} [token]
     */
    static BuildTest(test, container, token) {
        let form = TestBuilderUtil.BuildForm(token, test);
        container.append(form);
    }
    /**
     * @param {string | null | undefined} token
     * @param {Tests} test
     */
    static BuildForm(token, test) {
        token = token ?? generateGUID();
        /**@type {Object.<string, ModelProperty>} */
        const model = {};
        const sections = [...new Set(test.Pregunta_Tests.map(p => p.Seccion))]
            .map(section => ({ Name: section, /**@type {Array<String>} */ Propertys: [] }));

        test.Pregunta_Tests.forEach(p => {
            const type = this.BuidType(p);

            const section = sections.find(s => s.Name === p.Seccion);
            // @ts-ignore
            section?.Propertys.push(p.Id_pregunta_test.toString());

            //let type = "WRADIO";
            /**@type {ModelProperty} */
            const modelPropierty = {
                type: type,
                placeholder: p.Descripcion_pregunta,
                Dataset: "WRADIO" == type ? p.Cat_Tipo_Preguntas.Cat_Valor_Preguntas : undefined,
                label: p.Descripcion_pregunta,
            };
            model[p.Id_pregunta_test] = modelPropierty;
        });
        const form = new WForm({
            ModelObject: model,
            Groups: sections,
            SaveFunction: async (/**@type {Object.<string, any>} */ editinObject) => {
                /**@type {Array<Resultados_Pregunta_Tests>} */
                await this.SaveTest(editinObject, test, token, model);
            }
        });
        const style = css`
            .form-container {
                position: relative;
                width: 100%;
                max-height: 700px;
                overflow: auto;                
                box-sizing: border-box;
                h1 {
                    position: absolute;
                    top: 150px;
                    margin: auto;
                    color: #fff;
                    font-size: 3rem;
                    left: 50%;
                    transform: translateX(-50%) translateY(-50%);
                    text-align: center;
                    max-width: 80%;
                    text-shadow: 0 0 10px #000;
                }
                img {
                    position: sticky;
                    top: 0;
                    width: 100%;
                    height: 300px;
                    object-fit: cover;
                    border-radius: 20px;
                    z-index: 0;
                     pointer-events: none;
                } w-form {
                    max-width: 900px;
                    margin: 20px auto;
                    background-color: #fff;
                    border-radius: 10px;
                    box-shadow: 0 0 5px 0 #999;
                    position: relative;  
                    padding:30px; 
                }
            }
        `
        return html`<div class="form-container">
            <img src="${test.Image ?? "/Media/Image/covertTestTemplate.png"}">
            <h1>${test.Titulo}</h1>
            ${form}
            ${style}
        </div>`;
    }

    /**
     * @param {Pregunta_Tests} p
     */
    static BuidType(p) {
        switch (p.Cat_Tipo_Preguntas.Tipo_pregunta) {
            case "CATEGORICA": return "WRADIO";
            case "DICOTOMICA": return "WRADIO";
            case "LIKERT": return "WRADIO";
            case "ABIERTA_CORTA": return "TEXT";
            case "ABIERTA": return "RICHTEXT";
            case "NUMERICA": return "NUMBER";
            default: return "UNKNOW"
        }

    }
    /**
     * @param {{ [x: string]: Cat_Valor_Preguntas; }} editinObject
     * @param {Tests} test
     * @param {String} token
     * @param { { [x: string]: ModelProperty; }} model
     */
    static async SaveTest(editinObject, test, token, model) {
        const resultados = [];

        for (const prop in editinObject) {

            const pregunta = editinObject[prop];
            console.log(pregunta, prop);
            //console.log(test);

            const isCatValorPregunta = model[prop].type == "WRADIO";

            const valorPregunta = isCatValorPregunta ? pregunta.Valor : pregunta

            resultados.push(new Resultados_Pregunta_Tests({
                Id_Perfil: 1,
                Cat_Valor_Preguntas: isCatValorPregunta ? pregunta : undefined,
                Pregunta_Tests: test.Pregunta_Tests?.find(p => p.Id_pregunta_test.toString() == prop),
                Fecha: new Date(),
                // @ts-ignore
                Resultado: isCatValorPregunta ? valorPregunta : 0, Respuesta: !isCatValorPregunta ? valorPregunta : undefined,
                Estado: "ACTIVO"
            }));
        }

        /**@type {Array<Pregunta_Tests>} */
        // @ts-ignore
        const secciones = WArrayF.GroupBy(resultados.map(r => r.Pregunta_Tests), "Seccion");
        const Test = new Tests({ Resultados_Tests: [] });

        for (const pregntaTest of secciones) {
            const resultadosSeccion = resultados.filter(r => r.Pregunta_Tests?.Seccion == pregntaTest.Seccion);
            const resultado = new Resultados_Tests({
                Id_Perfil: 1,
                Tests: test,
                IdToken: token,
                Fecha: new Date(),
                Valor: WArrayF.SumValAtt(resultadosSeccion, "Resultado").toString(),
                Seccion: pregntaTest.Seccion,
                Resultados_Pregunta_Tests: resultadosSeccion,
                Tipo: pregntaTest.Seccion != null ? "SUB_ESCALA" : "ESCALA",
                Categoria_valor: "Por definir" //TODO DEFINIR EN BACKEND
            });
            Test.Resultados_Tests.push(resultado);
        }
        const response = await Test.SaveResultado();

        WAlertMessage.Success(response.message)
        ModalMessage(response.message, undefined, true);
    }
}