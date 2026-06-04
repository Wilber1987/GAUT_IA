//@ts-check
import { StylesControlsV2, StylesControlsV3, StyleScrolls } from "../../../WDevCore/StyleModules/WStyleComponents.js";
import { ComponentsManager, html, WRender } from "../../../WDevCore/WModules/WComponentsTools.js";
import { css } from "../../../WDevCore/WModules/WStyledRender.js";
import { Tests_ModelComponent } from "../../FrontModel/ModelComponent/Tests_ModelComponent.js";
import { Tests } from "../../FrontModel/Tests.js";
// @ts-ignore
import { WModalForm } from "../../../WDevCore/WComponents/WModalForm.js";
// @ts-ignore
import { WArrayF } from "../../../WDevCore/WModules/WArrayF.js";
import { Cat_Categorias_Test } from "../../FrontModel/Cat_Categorias_Test.js";
import { TestSent, TestSent_ModelComponent } from "../../FrontModel/TestSent.js";
import { WAcorden } from "../../../WDevCore/WComponents/UIComponents/WAcordeon.js";
import { DateTime } from "../../../WDevCore/WModules/Types/DateTime.js";
import { TestBuilderUtil } from "../../TestBuilderUtil.js";
/**
 * @typedef {Object} ComponentConfig
 * * @property {Object.<string, any>} [propierty]
 */
class ResolveTestViewComponent extends HTMLElement {
    /**
     * 
     * @param {ComponentConfig} props 
     */
    constructor(props) {
        super();

        this.OptionContainer = WRender.Create({ className: "OptionContainer" });
        this.TabContainer = WRender.Create({ className: "content-container", id: "content-container" });
        this.Manager = new ComponentsManager({ MainContainer: this.TabContainer, SPAManage: false });
        this.append(this.CustomStyle);
        this.append(
            StylesControlsV2.cloneNode(true),
            StyleScrolls.cloneNode(true),
            StylesControlsV3.cloneNode(true),
            this.OptionContainer,
            this.TabContainer,
            this.CustomCss
        );
        this.Draw();
    }
    Draw = async () => {
        this.SetOption();
        /**@type {Tests_ModelComponent} */
        this.ModelComponent = new Tests_ModelComponent();
        /**@type {Tests} */
        this.EntityModel = new Tests();
        /**@type {Array<Tests>} */
        this.Dataset = await this.EntityModel.Get();

        /**@type {Array<Cat_Categorias_Test>} */
        // @ts-ignore
        this.DatasetCategories = WArrayF.GroupBy(this.Dataset.map(d => d.Cat_Categorias_Test), "Descripcion");

        /**
         * @type {{ name: string; content: HTMLElement | HTMLInputElement | HTMLSelectElement; }[]}
         */
        const arrayContent = [];
        // @ts-ignore
        this.DatasetCategories.forEach(cat => {
            const categoryContent = {
                name: cat.Descripcion,
                content: html`<div class="content"></div>`
            }
            const testCategories = this.Dataset?.filter(test =>
                test.Cat_Categorias_Test?.Descripcion.includes(cat.Descripcion)
            );
            testCategories?.forEach(test => {
                categoryContent.content.append(this.TestCard(test))
            });
            arrayContent.push(categoryContent)
        });
        this.AcordeonTestListData = new WAcorden({
            Dataset: arrayContent,
            displayed: true,
            CustomStyle: this.CustomCss.cloneNode(true)
        })

        this.Manager.NavigateFunction("testList", this.AcordeonTestListData)
    }

    SetOption() {
        this.OptionContainer.append(WRender.Create({
            tagName: 'button', className: 'Block-Primary', innerText: 'Formularios',
            onclick: async () => this.Manager.NavigateFunction("testList")
        }))
    }

    CustomStyle = css`
        .TabContainer{
           overflow: auto;
        }           
    `
    TestCard(/**@type { Tests } */ test) {
        return WRender.Create({
            className: "task", children: [
                {
                    className: "tags", children: [{
                        tagName: 'img', className: "img-cover",
                        src: "" + test.Image
                    }, {
                        className: "viewer", children: [
                            { tagName: "span", className: "tag", style: { backgroundColor: test.Color }, innerHTML: test.Titulo },
                            {
                                tagName: 'button', className: 'options', children:
                                    [WRender.CreateStringNode(`<div><svg xml:space="preserve" viewBox="0 0 41.915 41.916" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns="http://www.w3.org/2000/svg" id="Capa_1" version="1.1" fill="#000000"><g stroke-width="0" id="SVGRepo_bgCarrier"></g><g stroke-linejoin="round" stroke-linecap="round" id="SVGRepo_tracerCarrier"></g><g id="SVGRepo_iconCarrier"><g><g><path d="M11.214,20.956c0,3.091-2.509,5.589-5.607,5.589C2.51,26.544,0,24.046,0,20.956c0-3.082,2.511-5.585,5.607-5.585 C8.705,15.371,11.214,17.874,11.214,20.956z"></path> <path d="M26.564,20.956c0,3.091-2.509,5.589-5.606,5.589c-3.097,0-5.607-2.498-5.607-5.589c0-3.082,2.511-5.585,5.607-5.585 C24.056,15.371,26.564,17.874,26.564,20.956z"></path> <path d="M41.915,20.956c0,3.091-2.509,5.589-5.607,5.589c-3.097,0-5.606-2.498-5.606-5.589c0-3.082,2.511-5.585,5.606-5.585 C39.406,15.371,41.915,17.874,41.915,20.956z"></path></g></g></g></svg></div>`)],
                                onclick: async () => {
                                    //code.....
                                }
                            }]

                    }]
                },/* {
                    tagName: "label", className: "labelheader", innerHTML: test.Titulo
                },*/ {
                    tagName: "p", className: "", innerHTML: test.Descripcion
                }, {
                    className: "stats", children: [
                        [
                            WRender.CreateStringNode(`<div><svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24"><g stroke-width="0" id="SVGRepo_bgCarrier"></g><g stroke-linejoin="round" stroke-linecap="round" id="SVGRepo_tracerCarrier"></g><g id="SVGRepo_iconCarrier"> <path stroke-linecap="round" stroke-width="2" d="M12 8V12L15 15"></path> <circle stroke-width="2" r="9" cy="12" cx="12"></circle> </g></svg>
                                ${new DateTime(test.Fecha_publicacion ?? new DateTime()).toLocaleDateString()}</div>`),

                            //ACTIONS
                            {
                                tagName: "a", innerHTML: "Resolver", onclick: async () => {
                                    const form = TestBuilderUtil.BuildForm(undefined, test);
                                    this.Manager.Remove("resolveForm")
                                    this.Manager.NavigateFunction("resolveForm", form)
                                }
                            },
                            {
                                tagName: "a", innerHTML: "Enviar", onclick: async () => {
                                    this.SentTest(test);
                                }
                            }
                        ]
                    ]
                }
            ]
        });
    }
    /**
     * @param {Tests} test
     */
    SentTest(test) {
        this.append(new WModalForm({
            AutoSave: true,
            ModelObject: new TestSent_ModelComponent(),
            EntityModel: new TestSent({
                Id_test: test.Id_test
            }),
        }))
    }
    CustomCss = css`
        w-resolve-test-view {
            display: block;
            height: 100%;
            box-sizing: border-box;
        }
        
        .content {
            display: grid;
            gap: 15PX;
            grid-template-columns: repeat(3, 1fr);
            @media (max-width: 900px) {
                grid-template-columns: repeat(2, 1fr);
            } 
            @media (max-width: 600px) {
                grid-template-columns: repeat(1, 1fr);
            } 
        }
        .task {
            position: relative;
            color: #2e2e2f;
            background-color: var(--secundary-color);
            overflow: hidden;
            border-radius: 8px;
            box-shadow: rgba(99, 99, 99, 0.1) 0px 2px 8px 0px;
            border: 3px dashed transparent;
            min-width: 340px;
            border: solid #e6e6e6 1px;
            min-height: 220px;
            display: grid;
            grid-template-rows: 80px 100px 40px;
            gap: 5px;
        }
        .labelheader {
            margin: 0px 10px;
            display: block;
            font-size: 12px;
            font-weight: 600;
            text-transform: capitalize;
            z-index: 1;
            color: #fff;
            background-color: rgba(0, 0, 0, 0.5);
            padding: 10px;
            border-radius: 10px;
        }

        .task p {
            font-size: 13px;
            margin: 0px 10px;           
            text-overflow: ellipsis;
            white-space: nowrap;
            overflow: hidden;
            z-index: 1;
            color: #fff;
            background-color: rgba(0, 0, 0, 0.4);
            padding: 10px;
            border-radius: 10px;
            height: 40px;
        }

        .task:hover {
            box-shadow: rgba(99, 99, 99, 0.3) 0px 2px 8px 0px;
            border-color: rgba(162, 179, 207, 0.2) !important;
        }

        .tag {
            border-radius: 100px;
            padding: 4px 13px;
            font-size: 0.8rem;
            font-weight: bold;
            color: #ffffff;
            background-color: #1389eb;
            display: -webkit-box;
            -webkit-line-clamp: 2; /* Limitar a 2 líneas */
            -webkit-box-orient: vertical;
            overflow: hidden;
            text-overflow: ellipsis;
            max-width: 300px;
        }

        .tags {
            width: 100%;
            display: flex;
            align-items: center;
            justify-content: space-between;
            position: relative;
        }
        .tags .img-cover {
            position: absolute;
            width: 100%;
            height: 180px;
            top: 0;
            left: 0;   
            object-fit: cover;       
        }

        .options {
            background: transparent;
            border: 0;
            color: #c4cad3;
            font-size: 17px;
        }

        .options svg {
            fill: #9fa4aa;
            width: 20px;
        }

        .stats {
            position: relative;
            width: -webkit-fill-available;
            color: #9fa4aa;
            font-size: 12px;
            display: flex;
            align-items: center;
            justify-content: space-between;  
            padding: 10px;      
        }
        .viewer {
            justify-content: space-between;
            display: flex;
            width: -webkit-fill-available;
            align-items: center;
            z-index: 1;
            margin: 10px;
        }
        .stats div {
            margin-right: 1rem;
            height: 20px;
            display: flex;
            align-items: center;
            cursor: pointer;
            width: 100%;
            gap: 10px;
        }
        .stats div a{
            transition: all 0.5s;
        }
        .stats div a:hover{
            color: #2e2e2f;
        }

        .stats svg {
            margin-right: 5px;
            height: 100%;
            stroke: #9fa4aa;
        }

        .viewer .img-participantes {
            height: 30px;
            width: 30px;
            background-color: rgb(28, 117, 219);
            margin-right: -10px;
            border-radius: 50%;
            border: 1px solid #fff;
            display: grid;
            align-items: center;
            text-align: center;
            font-weight: bold;
            color: #fff;
            padding: 2px;
        }

        .viewer span svg {
            stroke: #fff;
        }       
    `
}
customElements.define('w-resolve-test-view', ResolveTestViewComponent);
export { ResolveTestViewComponent };

