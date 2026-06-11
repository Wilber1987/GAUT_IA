//@ts-check
import { WRender, ComponentsManager, html } from "../WDevCore/WModules/WComponentsTools.js";
import { StylesControlsV2, StylesControlsV3, StyleScrolls } from "../WDevCore/StyleModules/WStyleComponents.js"
import { css } from "../WDevCore/WModules/WStyledRender.js";
import { WTableComponent } from "../WDevCore/WComponents/WTableComponent.js";
import { WTemplateBuilder } from "../WDevCore/WComponents/WTemplateBuilder.js";
import { PageType } from "../WDevCore/WComponents/WDocumentViewer.js";
import { WChatComponent } from "../WDevCore/WComponents/WChatComponent.js";
import { WSecurity } from "../WDevCore/Security/WSecurity.js";
import { ResolveTestViewComponent } from "../Questionnaires/Views/Component/ResolveTestViewComponent.js";
import { TemplateData, TemplateData_ModelComponent } from "../WDevCore/WComponents/Models/TemplateModel.js";

/**
 * @typedef {Object} ComponentConfig
 * * @property {Object} [propierty]
 */
class TemplateView extends HTMLElement {
    /**
     * 
     * @param {ComponentConfig} props 
     */
    constructor(props) {
        super();
        this.attachShadow({ mode: 'open' });
        this.OptionContainer = WRender.Create({ className: "OptionContainer" });
        this.TabContainer = WRender.Create({ className: "TabContainer", id: 'TabContainer' });
        this.Manager = new ComponentsManager({ MainContainer: this.TabContainer, SPAManage: false });
        this.shadowRoot?.append(this.CustomStyle);
        this.shadowRoot?.append(
            StylesControlsV2.cloneNode(true),
            StyleScrolls.cloneNode(true),
            StylesControlsV3.cloneNode(true),
            this.OptionContainer,
            this.TabContainer
        );
        this.Draw();
        /**@type {HTMLElement?} */
        this.SelectedSection = null;
    }
    Draw = async () => {
        this.SetOption();
    }

    async SetOption() {
        this.OptionContainer.append(WRender.Create({
            tagName: 'button', className: 'Btn-Mini-Success', innerText: 'Documentos',
            onclick: async () => this.Manager.NavigateFunction("id", await this.MainComponent())
        }))
        this.OptionContainer.append(WRender.Create({
            tagName: 'button', className: 'Btn-Mini-Success', innerText: 'Crear nuevo documento',
            onclick: async () => this.CreateDocumentComponent()
        }))
        this.Manager.NavigateFunction("id", await this.MainComponent());
    }
    CreateDocumentComponent() {
        this.Manager.Remove("idTest")
        this.Manager.NavigateFunction("idTest", new ResolveTestViewComponent({}));
    }
    async MainComponent() {
        return new WTableComponent({
            ModelObject: new TemplateData_ModelComponent(),
            EntityModel: new TemplateData(),
            Options: {
                UserActions: [{
                    name: 'edit', action: (/** @type {TemplateData} */ TableElement) => {
                        this.Manager.NavigateFunction("idtemplate" + TableElement.Id_Template, TemplateEditor(TableElement, this.ResponseActions()));
                    }
                }]
            }
        })
    }

    ResponseActions() {
        return [
            {
                name: "add_section", label: "agregar a la sección",
                icon: `<svg width="64px" height="64px" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M20 14V7C20 5.34315 18.6569 4 17 4H12M20 14L13.5 20M20 14H15.5C14.3954 14 13.5 14.8954 13.5 16V20M13.5 20H7C5.34315 20 4 18.6569 4 17V12" stroke="#0f279f" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path> <path d="M7 4V7M7 10V7M7 7H4M7 7H10" stroke="#0f279f" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>`,
                action: (/** @type {string | Node} */ response) => {
                    if (this.SelectedSection) {
                        this.SelectedSection.append(response)
                    }
                }

            }
        ]
    }
    GetDiccionary() {
        return html`<ul></ul>`;
    }

    CustomStyle = css`
        :host {
            display: flex;
            height: 100%;
            flex-direction: column;
            padding:20px 20px  0px 20px ;
            background-color: #fff;
            box-sizing: border-box;
            box-shadow: 0 0 5px 0 #dad9d9;
        }
        .OptionContainer {
            margin-bottom: 10px;
        }
        .TabContainer {
            flex:1;
            min-height:0;
            overflow:hidden;
        }
            
    `
}
customElements.define('w-template-view-manager', TemplateView);
export { TemplateView }

/**
 * @param {TemplateData} TableElement
 */
export const TemplateEditor = (TableElement, /** @type {{ name: string; label: string; icon: string; action: (response: string | Node) => void; }[]} */ responseActions) => {
    localStorage.setItem("identity", WSecurity.UserData.nickname)
    const IAChat = new WChatComponent({
        Url: "../api/WebhookSsmpIA",
        UrlGetConfigData: "../api/ApiEntityHelpdesk/getTbl_Comments",
        UrlSearch: "../api/ApiEntityHelpdesk/getTbl_Comments",
        UrlAdd: "../api/ApiEntityHelpdesk/saveTbl_CommentsWeb",
        UserIdProp: "Id_User",
        CommentsIdentify: TableElement.Token,
        CommentsIdentifyName: "Token",
        AddObject: false,
        WithAgent: true,
        ResponseActions: responseActions,
        UseLocalMemory: false,
        IdentityValue: WSecurity.UserData.mail
    })
    return html`<div class="template-element-editor">
        <style>
            w-chat-component {
                height: calc(100% - 20px) !important;
                box-sizing: border-box;
                display: block;
            }  
            .template-element-editor{
                display: grid;
                height: 100%;
                grid-template-columns: calc(100% - 670px) 660px ;
                gap: 10px;
                overflow: hidden;
                min-width: 0; 
                min-height: 0;
                w-template-builder, .diccionary {
                    padding: 0px 0px;
                    border-radius: 10px;
                    height: 100%;
                    min-height: 0;
                }
            }  
        </style>
            ${new WTemplateBuilder({
                Data: TableElement, PageType: PageType.OFICIO, SectionActions: [
                {
                    name: `<svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg"><g id="SVGRepo_bgCarrier" stroke-width="0"></g><g id="SVGRepo_tracerCarrier" stroke-linecap="round" stroke-linejoin="round"></g><g id="SVGRepo_iconCarrier"> <path d="M12 22C17.5228 22 22 17.5228 22 12C22 6.47715 17.5228 2 12 2C6.47715 2 2 6.47715 2 12C2 13.5997 2.37562 15.1116 3.04346 16.4525C3.22094 16.8088 3.28001 17.2161 3.17712 17.6006L2.58151 19.8267C2.32295 20.793 3.20701 21.677 4.17335 21.4185L6.39939 20.8229C6.78393 20.72 7.19121 20.7791 7.54753 20.9565C8.88837 21.6244 10.4003 22 12 22Z" stroke="#1C274C" stroke-width="1.5"></path> <path opacity="0.5" d="M8 12H8.009M11.991 12H12M15.991 12H16" stroke="#1C274C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path> </g></svg>`,
                    title: "Agregar al chat",
                    action: (/** @type {Object<String, any>} */ editingObject, /** @type {HTMLElement|String} */ wrapper) => {
                        IAChat.AddContext(editingObject);
                        IAChat.AddDataChat(wrapper);
                    }
                }
                ]
            })}
            ${IAChat}
        </div>`;
}