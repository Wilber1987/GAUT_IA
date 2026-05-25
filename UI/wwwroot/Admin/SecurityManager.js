//@ts-check
import { WAppNavigator } from "../WDevCore/WComponents/WAppNavigator.js";
import { WModalForm } from "../WDevCore/WComponents/WModalForm.js";
import { WTableComponent } from "../WDevCore/WComponents/WTableComponent.js";
import { html } from '../WDevCore/WModules/WComponentsTools.js';
import { WAjaxTools } from "../WDevCore/WModules/WAjaxTools.js";
import { Security_Permissions, Security_Permissions_ModelComponent, Security_Roles, Security_Roles_ModelComponent } from "./Security/Model/SecurityModel.js";
import { ChangePasswordModel, ChangeRolesModel, ChangeStateModel, Security_Users, Security_Users_ModelComponent } from "./Security/Model/SecurityUsers.js";
import { css } from "../WDevCore/WModules/WStyledRender.js";
import { WForm } from "../WDevCore/WComponents/WForm.js";
import { Tbl_Profiles_Model, Tbl_Profiles_Model_ModelComponent, Tbl_Profiles_Services_Manager } from "./Security/Model/Tbl_Profile_Model.js";
import { ModalVericateAction } from "../WDevCore/WComponents/ModalVericateAction.js";
import { WCard } from "../WDevCore/WComponents/WCard.js";
import { WAlertMessage } from "../WDevCore/WComponents/WAlertMessage.js";
import { Cat_Dependencias, Cat_Dependencias_ModelComponent } from "./Security/OrganizationModel/Cat_Dependencias.js";
import { Tbl_Servicios, Tbl_Servicios_ModelComponent } from "./Security/OrganizationModel/Tbl_Servicios.js";


/**
 * @typedef {Object} ComponentConfig
 * * @property {Object} [propierty]
 */
class SecurityManager extends HTMLElement {

	/**
	 * 
	 * @param {ComponentConfig} props 
	 */
	constructor(props) {
		super();
		this.props = props;
		this.id = "MainProyect";
		this.className = "MainProyect DivContainer";
		this.DrawComponent();
	}
	BuildNavElements() {
		const nav = [
			{
				name: "Usuarios", action: async () => { return this.NavToUsers(); }
			}, {
				name: "Seguridad", action: async () => { return this.NavToSecurity(); }
			}, {
				name: "Organización", action: async () => { return this.NavToOrganizacion(); }
			},
		];
		return nav;
	}

	NavToOrganizacion() {
		return new WAppNavigator({
			NavStyle: "tab",
			Inicialize: true,
			Elements: [
				{
					name: "Dependencias", action: () =>
						new WTableComponent({
							ModelObject: new Cat_Dependencias_ModelComponent(),
							EntityModel: new Cat_Dependencias(), 
							UseEntityMethods: true
						})
				}, {
					name: "Servicios/Dependencias", action: () =>
						new WTableComponent({
							ModelObject: new Tbl_Servicios_ModelComponent(),
							EntityModel: new Tbl_Servicios(), 
							UseEntityMethods: true
						})
				}
			],
		});
	}
	NuevoUsuario() {
		//throw new Error("Method not implemented.");
	}
	NavToSecurity() {
		return html`<div>
			<h2>Roles</h2>
			<p>Se utilizan para asignar grupos de permisos a usuarios, un usuario puede poseer mas de un rol y un rol puede tener multples permisos.</p>
			${new WTableComponent({
			ModelObject: new Security_Roles_ModelComponent(),
			UseEntityMethods: true,
			EntityModel: new Security_Roles()
		})}
			<h2>Permisos</h2>
			<p>Dan acceso a funcionalidades de la aplicacion a nivel de seguridad, un usuario tendra funcionalidades bloqueadas si estas estan bloquedas por permisos.</p>
			${new WTableComponent({
			ModelObject: new Security_Permissions_ModelComponent(),
			EntityModel: new Security_Permissions(),
			UseEntityMethods: true,
			Options: { Search: true, Edit: true }
		})}
		</div>`
	}
	async NavToUsers() {
		//const Roles = await WAjaxTools.PostRequest("../api/ApiEntitySECURITY/getSecurity_Roles", {});
		return new WTableComponent({
			EntityModel: new Security_Users(),
			ModelObject: new Security_Users_ModelComponent(),
			UseEntityMethods: true,
			Options: {
				Filter: true,
				UserActions: this.UserActions,
				Add: true
			}
		})
	}
	connectedCallback() { }
	DrawComponent = async () => {
		this.MainNav = new WAppNavigator({
			NavStyle: "tab",
			Inicialize: true,
			Elements: this.BuildNavElements()
		});
		this.append(this.MainNav);
	}
	UserActions = [{
		name: "Cambiar estado", action: (/** @type {Security_Users} */ editingUser) => {
			this.append(this.ChangeState(editingUser));
		}
	}, {
		name: "Editar contraseña", action: (/** @type {Security_Users} */ editingUser) => {
			this.append(this.ChangePassword(editingUser));
		}
	}, {
		name: "Editar roles", action: async (/** @type {Security_Users} */ editingUser) => {
			this.append(await this.ChangeRoles(editingUser));
		}
	}, {
		name: "Editar datos", action: async (/** @type {Security_Users} */ editingUser) => {
			await this.ChangeDatos(editingUser);
		}
	}, {
		name: "Asignación de servicios", action: (/** @type {Security_Users} */ editingUser) => {
			this.ChangeServices(editingUser);
		}
	},]
	ChangeDatos = async (/**@type {Security_Users} */ object) => {
		const form = new WForm({
			EditObject: object,
			ModelObject: new Security_Users_ModelComponent({
				Password: { type: "password", hidden: true },
				Security_Users_Roles: { type: "text", hidden: true }
			}),
			AutoSave: true,
			SaveFunction: () => {
				this.MainNav?.Manager?.NavigateFunction("element0");
			}
		})
		this.MainNav?.Manager?.NavigateFunction("element" + object.Id_User, form);
		//return 
	}
	ChangeRoles = async (/**@type {Security_Users} */ object) => {
		const Roles = await WAjaxTools.PostRequest("../api/ApiEntitySECURITY/getSecurity_Roles", {});
		const security_Users_Roles = object.Security_Users_Roles?.map(userRol => new Security_Roles(userRol.Security_Role ?? {}))
		return new WModalForm({
			title: "CAMBIO DE ROLES",
			EditObject: { Id_User: object.Id_User, Security_Users_Roles: security_Users_Roles },
			ModelObject: new ChangeRolesModel({
				Security_Users_Roles: {
					type: "multiselect", Dataset: Roles, ModelObject: () => new Security_Roles_ModelComponent()
				}
			}),
			EntityModel: new Security_Users(),
			AutoSave: true
		})
	}
	ChangePassword = (/**@type {Security_Users} */ object) => {
		return new WModalForm({
			title: "CAMBIO DE CONTRASEÑA",
			EditObject: { Id_User: object.Id_User, Password: object.Password },
			ModelObject: new ChangePasswordModel(),
			EntityModel: new Security_Users(),
			AutoSave: true
		})
	}
	ChangeState = (/**@type {Security_Users} */ object) => {
		return new WModalForm({
			title: "CAMBIO DE ESTADO",
			EditObject: { Id_User: object.Id_User, Estado: object.Estado },
			ModelObject: new ChangeStateModel(),
			EntityModel: new Security_Users(),
			AutoSave: true
		})
	}
	/**
	 * @param {Security_Users} editingUser
	 */
	ChangeServices(editingUser) {
		const form = new WForm({
			EditObject: new Tbl_Profiles_Model({
				IdUser: editingUser.Id_User,
				Id_Perfil: editingUser.ActiveProfile?.Id_Perfil,
				Tbl_Dependencias_Usuarios: editingUser.ActiveProfile?.Tbl_Dependencias_Usuarios,
				Tbl_Servicios_Profile: editingUser.ActiveProfile?.Tbl_Servicios_Profile
			}),
			ModelObject: new Tbl_Profiles_Services_Manager(),
			SaveFunction: async (/**@type {Tbl_Profiles_Model} */ editObject) => {
				this.append(ModalVericateAction(async () => {
					if (editingUser.Tbl_Profiles_Model != undefined && editingUser.Tbl_Profiles_Model[0] != undefined) {
						editingUser.Tbl_Profiles_Model[0].Tbl_Dependencias_Usuarios = editObject.Tbl_Dependencias_Usuarios;
						editingUser.Tbl_Profiles_Model[0].Tbl_Servicios_Profile = editObject.Tbl_Servicios_Profile;
						try {
							const response = editingUser.Update();
							WAlertMessage.Success("Datos guardados correctamente", true);
						} catch (error) {
							// @ts-ignore
							WAlertMessage.Danger(error.toString());
						}

					}
				}, "Esta seguro que desea guardar los cambios?"))

			}
		})
		const dataWrapper = html`<div>
			<div class="header">
				${new WCard(
			// @ts-ignore
			editingUser?.Tbl_Profiles_Model[0], new Tbl_Profiles_Model_ModelComponent(), {})}
				<hr>
				<p>Cada dependencia posee una lista de servicios a los que debe atender, por lo que el perfil asociado a una o varias dependencias solo tendrá acceso a los servicios disponibles en esa dependencia</p>
				<p><strong>Nota:</strong> Todo usuario que necesite acceso a la gestión los casos, debe tener como mínimo un <strong>ROL</strong> con acceso al permiso <strong>TECNICO CASOS DEPENDENCIA</strong></p>
				<hr>
			</div>			
			${form}
		</div>`
		this.MainNav?.Manager?.NavigateFunction("elementServices" + editingUser.Id_User, dataWrapper);
	}
}
customElements.define('w-security-manager', SecurityManager);
export { SecurityManager }