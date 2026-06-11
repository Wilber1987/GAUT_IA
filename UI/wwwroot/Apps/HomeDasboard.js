//@ts-check
import { WRender, ComponentsManager, html } from "../WDevCore/WModules/WComponentsTools.js";
import { StylesControlsV2, StylesControlsV3, StyleScrolls } from "../WDevCore/StyleModules/WStyleComponents.js"
import { css } from "../WDevCore/WModules/WStyledRender.js";
import { WAppNavigator } from "../WDevCore/WComponents/WAppNavigator.js";
import { TemplateData, TemplateData_ModelComponent } from "../WDevCore/WComponents/Models/TemplateModel.js";
import { WPaginatorViewer } from "../WDevCore/WComponents/WPaginatorViewer.js";
import { WCardDetailV1 } from "../WDevCore/WComponents/UIComponents/WCardDataComponent.js";
import { DateTime } from "../WDevCore/WModules/Types/DateTime.js";
import { TemplateEditor } from "../Template/TemplateView.js";
import { WLineChart } from "../WDevCore/WComponents/ChartsComponents/WLineChart.js";
import { WAjaxTools } from "../WDevCore/WModules/WAjaxTools.js";
/**
 * @typedef {Object} ComponentConfig
 * * @property {Object} [propierty]
 */
class HomeDasboard extends HTMLElement {
    /**
     * 
     * @param {ComponentConfig} config 
     */
    constructor(config) {
        super();
        this.props = config;
        this.append(this.CustomStyle);
        this.NavManager = new WAppNavigator({
            NavStyle: "tab",
            Inicialize: true,
            Elements: this.NavElements()
        })
        this.append(
            StylesControlsV2.cloneNode(true),
            StyleScrolls.cloneNode(true),
            StylesControlsV3.cloneNode(true),
            this.NavManager,
        );
        this.Draw();
    }
    Draw = async () => {


    }
    NavElements() {
        return [{
            name: "Element", action: async () => {
                return await this.MainComponent();
            }
        }]
    }
    async MainComponent() {
        /**@type {Array<TemplateData>} */
        const Dataset = await new TemplateData().Get();

        const chart = await this.GetChart()

        const divContent = html`<div class="component">
             ${new WPaginatorViewer({
            maxElementByPage: 5,
            Dataset: Dataset.map(templateData => this.BuildTemplateCard(templateData))
        })}
            ${chart}
        </div>`

        return divContent;
        //throw new Error("Method not implemented.");
    }
    async GetChart() {
        const GroupParams = ["Year", "Month"]
        const EvalParams = ["Descripcion"]
        const request = {
            "Desde": new DateTime(null).subtractDays(800),
            "Hasta": new DateTime(null),
            "GroupParams": GroupParams,
            "EvalParams": EvalParams
        }
        console.log(request);
        

        const response = await WAjaxTools.PostRequest("/api/ApiDashboard/DocumentDataByTime", request);

        return new WLineChart({
            data: response,
            GroupParams: GroupParams,
            EvalParams: EvalParams,
            title: '📊 Documentos'
        });
    }

    /**
     * @param {TemplateData} templateData
     */
    BuildTemplateCard(templateData) {
        return new WCardDetailV1({
            Title: templateData.Descripcion,
            Body: new DateTime(templateData.Fecha).formatDateTimeToDDMMYYHHMM(),
            Image: "/Media/image/cardBackgroundDocument.png",
            Actions: [
                {
                    name: "Edit", action: () => {
                        this.NavManager.Manager?.NavigateFunction("template" + templateData.Id_Template,
                            TemplateEditor(templateData, []))
                    }
                }
            ]
        })
    }

    CustomStyle = css`
        w-home-dashboard {
            display: flex;
            height: 100%;
            flex-direction: column;
            padding:20px 20px  0px 20px ;
            background-color: #fff;
            box-sizing: border-box;
            box-shadow: 0 0 5px 0 #dad9d9;
        }
        .component{
           display: grid;
           grid-template-columns: 400px calc(100% - 420px);
           gap: 20px;
        }           
    `
}
customElements.define('w-home-dashboard', HomeDasboard);
export { HomeDasboard }