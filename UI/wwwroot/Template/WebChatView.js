//@ts-check

import { Tbl_Comments } from "../Notificaciones_Mensajeria/Gestion_Mensajes/Tbl_Comments.js";
import { Tbl_Comments_Conversations } from "../Notificaciones_Mensajeria/Gestion_Mensajes/Tbl_Comments_Conversations.js";
import { WSecurity } from "../WDevCore/Security/WSecurity.js";
import { StylesControlsV2, StylesControlsV3, StyleScrolls } from "../WDevCore/StyleModules/WStyleComponents.js";
import { WAppNavigator } from "../WDevCore/WComponents/WAppNavigator.js";
import { WAgentChatComponent } from "../WDevCore/WComponents/WAgentChatComponent.js";
import { FilterData } from "../WDevCore/WModules/CommonModel.js";
import { ComponentsManager, generateGUID, html, WRender } from "../WDevCore/WModules/WComponentsTools.js";
import { css } from "../WDevCore/WModules/WStyledRender.js";


/**
 * @typedef {Object} ComponentConfig
 * * @property {Object} [propierty]
 */
export class WebChatView extends HTMLElement {
    /**
     * 
     * @param {ComponentConfig} [Config] 
     */
    constructor(Config) {
        super();
        this.attachShadow({ mode: 'open' });
        this.Config = Config
        this.ComponentsTitle = undefined;
        this.OptionContainer = WRender.Create({ className: "OptionContainer" });
        this.TabContainer = WRender.Create({ className: "TabContainer", id: 'TabContainer' });
        this.Container = html`<div class="chat-component">
            ${this.OptionContainer}
            ${this.TabContainer}
        </div>`

        this.Manager = new ComponentsManager({ MainContainer: this.TabContainer, SPAManage: false });
        /*this.AppNavigator = new WAppNavigator({
            NavTitle: this.ComponentsTitle,
            Inicialize: true,
            Elements: this.SetOption()
        })*/
        this.shadowRoot?.append(this.CustomStyle);
        this.shadowRoot?.append(
            StylesControlsV2.cloneNode(true),
            StyleScrolls.cloneNode(true),
            StylesControlsV3.cloneNode(true),
            this.Container
        );
        this.Draw();
    }
    Draw = async () => {
        /**@type {Array<Tbl_Comments_Conversations>} */
        const conversations = await new Tbl_Comments_Conversations().Get();
        this.Manager.NavigateFunction("main", this.MainComponent())
        this.OptionContainer.innerHTML = "";
        this.OptionContainer.append(html`<button class="conversation-btn new-conversation" onclick="${() => {
            const token = generateGUID()
            this.Manager.NavigateFunction(token, this.ChargeChat(token, []))
        }}"></button>`)
        conversations.forEach(conversation => {
            const displayName = conversation.Nombre || "Sin nombre";
            const truncatedName = displayName.length > 20
                ? displayName.substring(0, 18) + "..."
                : displayName;
            this.OptionContainer.append(html`<button class="conversation-btn" onclick="${() => {
                this.Navigate(conversation);
            }}">
                <span>${truncatedName}</span>
            </button>`)
        });
    }

    /**
     * @param {Tbl_Comments_Conversations} conversation
     */
    async Navigate(conversation) {
        /**@type {Array<Tbl_Comments>} */
        const history = await new Tbl_Comments({ Token: conversation.Token }).Get();
        console.log("conversation" + conversation.Id_Conversacion);
        
        this.Manager.NavigateFunction("conversation" + conversation.Id_Conversacion, this.ChargeChat(conversation.Token, history));
    }

    MainComponent() {
        return html`<div class="chat-panel"></div>`
    }
    /**
     * @param {string} token
     * @param {Tbl_Comments[]} history
     */
    ChargeChat(token, history) {
        console.log(WSecurity.UserData);
        return new WAgentChatComponent({
            Url: "../api/WebhookSsmpIA",
            UrlGetConfigData: "../api/ApiEntityHelpdesk/getTbl_CommentsWeb",
            UrlSearch: "../api/ApiEntityHelpdesk/getTbl_CommentsWeb",
            UrlAdd: "../api/ApiEntityHelpdesk/saveTbl_CommentsWeb",
            UserIdProp: "Id_User",
            Dataset: history,
            CommentsIdentify: token ?? generateGUID(),
            CommentsIdentifyName: "Token",
            AddObject: false,
            IdentityValue: WSecurity.UserData.mail
        })
    }



    CustomStyle = css`
        .chat-component{
            display: grid;
            grid-template-columns: 260px calc(100% - 270px);
            gap: 0;
            height: 100%;
        }

        /* Sidebar de conversaciones */
        .OptionContainer {
            display: flex;
            flex-direction: column;
            padding: 8px;
            overflow-y: auto;
            background-color: var(--secundary-color);
            border-right: 1px solid var(--primary-border);
            gap: 4px;
        }

        /* Botón Nueva conversación */
        .conversation-btn.new-conversation {
            display: flex;
            align-items: center;
            justify-content: center;
            width: 100%;
            padding: 12px 16px;
            background-color: var(--primary-background);
            color: #fff;
            border: none;
            border-radius: 8px;
            font-size: 13px;
            font-weight: 500;
            cursor: pointer;
            transition: all 0.2s ease-in-out;
            gap: 8px;
        }

        .conversation-btn.new-conversation:hover {
            background-color: var(--secundary-background);
            transform: translateY(-1px);
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
        }

        .conversation-btn.new-conversation::before {
            content: "+";
            font-size: 18px;
            font-weight: bold;
            line-height: 1;
        }
         

        /* Botones de conversaciones existentes */
        .conversation-btn {
            display: flex;
            align-items: center;
            justify-content: space-between;
            width: 100%;
            padding: 10px 14px;
            background-color: var(--primary-color);
            color: var(--font-primary-color);
            border: none;
            border-radius: 8px;
            font-size: 13px;
            cursor: pointer;
            transition: all 0.2s ease-in-out;
            text-align: left;
            overflow: hidden;
        }

        .conversation-btn:hover {
            background-color: var(--fourth-color);
            transform: translateX(3px);
        }

        .conversation-btn::after {
            content: "";
            display: inline-block;
            width: 6px;
            height: 6px;
            border-radius: 50%;
            background-color: var(--primary-background);
            opacity: 0.4;
            transition: all 0.2s ease-in-out;
        }
        .conversation-btn.new-conversation::after { 
            display: none;
        }

        .conversation-btn:hover::after {
            opacity: 1;
            transform: scale(1.5);
        }
    `
}
customElements.define('w-view-component', WebChatView);