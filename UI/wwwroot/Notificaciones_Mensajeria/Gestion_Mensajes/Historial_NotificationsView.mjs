//@ts-check


import { StylesControlsV2, StylesControlsV3, StyleScrolls } from "../../WDevCore/StyleModules/WStyleComponents.js";
import { ModalMessage } from "../../WDevCore/WComponents/ModalMessage.js";
import { WFilterOptions } from "../../WDevCore/WComponents/WFilterControls.js";
import { WPrintExportToolBar } from "../../WDevCore/WComponents/WPrintExportToolBar.mjs";
import { PageType } from "../../WDevCore/WComponents/WReportComponent.js";
import { WAjaxTools } from "../../WDevCore/WModules/WAjaxTools.js";
import { ComponentsManager, html, WRender } from "../../WDevCore/WModules/WComponentsTools.js";
import { css } from "../../WDevCore/WModules/WStyledRender.js";
import { Notificaciones_ModelComponent } from "../Model/ModelComponent/Notificacion_ModelComponent.js";
import { Notificaciones } from "../Model/Notificaciones.js";

/**
 * @typedef {Object.<string, any>} Historial_NotificationsViewConfig
 * * @property {Object.<string, any>} [propierty]
 */
class Historial_NotificationsView extends HTMLElement {
	/**
	 * @param {Historial_NotificationsViewConfig} [props] 
	 */
	constructor(props) {
		super();
		this.OptionContainer = WRender.Create({ className: "OptionContainer" });
		this.TabContainer = WRender.Create({ className: "TabContainer", id: 'TabContainer' });
		this.Manager = new ComponentsManager({ MainContainer: this.TabContainer, SPAManage: false });
		this.append(this.CustomStyle);
		const container = WRender.Create({ className: "component" });
		// @ts-ignore
		const EntityModel = new Notificaciones({
			// @ts-ignore
			Get: async () => {
				return await EntityModel.GetData("ApiNotificacionesManage/GetNotificacionesEnviadas")
			}
		})
		/**@type {Notificaciones[]} */
		this.Dataset = []
		this.Filter = new WFilterOptions({
			AutoSetDate: true,
			AutoFilter: true,
			EntityModel: EntityModel,
			// @ts-ignore
			ModelObject: new Notificaciones_ModelComponent(),
			UseEntityMethods: true,
			Display: true,
			Dataset: [],
			FilterFunction: async (/** @type {Notificaciones[]} */ filterData) => {
				container.innerHTML = "";
				this.Dataset = filterData;
				container.append(this.DrawInformeNotifications(filterData));
				this.Manager.NavigateFunction("informe", container)
			}
		});
		const notificationReportData = {
			FirstDate: null,
			LastDate: null,
			Mail: "",
			/**@type {Notificaciones[]} */ Notificaciones:  []
		}
		this.OptionContainer.append(html`<div class="control-container">
			<input type="text" placeholder="escribe un correo" onchange="${(/** @type {{ target: { value: string; }; }} */ ev) => {
				notificationReportData.Mail = ev.target.value;
				console.log(notificationReportData);

			}}"/>
			<button class="BtnSuccess" onclick="${async () => {
				notificationReportData.Notificaciones = this.Dataset;
				notificationReportData.FirstDate = this.Filter.ModelObject.FilterData[0].Values[0];
				notificationReportData.LastDate = this.Filter.ModelObject.FilterData[0].Values[1];
				console.log(notificationReportData);
				const response = await WAjaxTools.PostRequest("/api/Report/SenReportNotifications", notificationReportData);
				if (response.status == 200) {
					document.body.append(ModalMessage(response.message))
				}
			}}">Enviar</button>
		</div>`);
		this.OptionContainer.append(new WPrintExportToolBar({
			ExportPdfAction: (/**@type {WPrintExportToolBar} */ tool) => {
				const body = container.cloneNode(true);
				body.appendChild(this.CustomStyle.cloneNode(true));
				tool.ExportPdf(body, PageType.OFICIO_HORIZONTAL)
			}
		}));
		this.append(
			StylesControlsV2.cloneNode(true),
			StyleScrolls.cloneNode(true),
			StylesControlsV3.cloneNode(true),
			this.OptionContainer,
			this.Filter,
			this.TabContainer
		);
		this.Informes = {};
	}
	
	/**
	 * @param {Array<Notificaciones>} notificaciones 
	 */
	DrawInformeNotifications(notificaciones) {
		console.log(notificaciones);
		// @ts-ignore                    
		const NotificationsNotificacion = Object.groupBy(notificaciones, p => p.Year);
		const containerInforme = this.ViewNotificacionInforme(NotificationsNotificacion);
		return containerInforme;
	}
	/**
	 * @param {Partial<Record<PropertyKey, Notificaciones[]>>} notificaciones
	 */
	ViewNotificacionInforme(notificaciones) {
		// @ts-ignore
		const div = html`<div class="Notification-container">
			<style>
				.Notification-container {
					display: flex;
					flex-direction: column;
					gap: 10px;
					padding: 10px;
				}
			</style>            
		</div>`;
		return div;
	}	

	CustomStyle = css`
		.component{
		   display: block;
		}    
		.CANCELADO {
			color: green;
		}  
		.PENDIENTE {
			color: red
		}  
		.control-container {
			display: flex;
			width: 400px;
			height: 45px;
		}
		.component {
			background-color: #FFF;
			color: #000;	
			border-radius: 10px;
		}
		.OptionContainer {
			display: flex;
			justify-content: flex-end;
			align-items: center;
		}
		.mes-container {
			/* display: grid;
			grid-template-columns: repeat(14, 1fr); */
			gap: 5px;
			& h3 {
				grid-column: span 14;
				font-size: 1em;
				border-bottom: 1px solid #919191;
			}
			
		}   
		table.mes-container {
			gap: 5px;
			border-collapse: collapse;
			width: 100%;
		}
		.Notification-details-container {
			/* display: contents; */
			grid-column: span 14;
		}
		.total-container {
			display: flex;
			justify-content: space-between;
			background-color: #f1f1f1;
			font-weight: bold;
		}
		.Notification-title {
			font-size: 0.8em;
			padding: 8px;
			font-weight: bold;
			background-color: #f1f1f1;
		}
		.Notification-value {
			font-size: 0.8em;
			padding: 8px;
			vertical-align: top;
		}
		.total-container {			
			font-size: 1em;
		} 
		.value, .total-cargos {
			text-align: right;
		}  
		.Historial{
			display: flex;
			flex-direction: column;            
			gap: 20px;
		}   
		.Historial .options-container {
			display: flex;
			align-items: center;
			justify-content: space-between;
			grid-column: span 2;
		}
		.Notificacion-card-container {
			display: flex;
			border: 1px solid #d6d6d6;;
			border-radius: 10px;
			cursor: pointer;
			padding: 10px;
			max-width: 400px; 
		}
		.Notificacion-card {
			display: flex;         
			gap: 10px;
			min-width: 400px;
			align-items: center;
		}
		.alumnos-container {
			display: flex;
			flex-direction: row;
			flex-wrap: wrap;
			gap: 10px;
		}
		.TabContainer {
			min-height: 200px;
		}
		.avatar-est{
			height: 100px;
			width: 80px;
			min-width: 80px;
			border-radius: 10px !important;
			object-fit: cover;
		}
		
		.aside-container {            
			padding: 0;
			border-radius: 0;
			box-shadow: unset
		}
		.Notificacion-container {
			display: flex;
			flex-direction: column;
			gap: 0px;
			padding: 10px;
			& .Notificacion {
				margin-bottom: 20px;
			}
			& .data-container {
				display: flex;
				justify-content: flex-start;
				/* border-bottom: 1px solid #d6d6d6; */
				& .Notificacion-prop {
					background-color: #f1f1f1;
					width: 100px;
				}
				& label {
					padding: 10px;
					margin-bottom: 0;
				}
			}
		}
		
		@media (max-width: 768px) {
			.Historial{               
				grid-template-columns: 100%;
			} 
			.Historial .options-container {
				grid-column: span 1;
			}
			.TabContainer {
				border-left: unset;
				padding-left: unset;                
			}
		}
	`
}
customElements.define('w-component', Historial_NotificationsView);
export { Historial_NotificationsView };
