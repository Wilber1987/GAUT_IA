
//@ts-check
// @ts-ignore
import { ModelProperty } from "../../../WDevCore/WModules/CommonModel.js";
import { EntityClass } from "../../../WDevCore/WModules/EntityClass.js";
import {  Security_Roles_ModelComponent, Security_Users_Roles } from "./SecurityModel.js";
import { Tbl_Profiles_Model, Tbl_Profiles_Model_ModelComponent } from "./Tbl_Profile_Model.js";

class Security_Users  extends EntityClass{
	/**
	* @param {Partial<Security_Users>} [props] 
	*/	
	constructor(props) {
		super(props, "EntitySECURITY");
		Object.assign(this, props);
	}	
	/**@type {Number?} */ Id_User = null;
	/**@type {String?} */ Nombres = null;
	/**@type {String?} */ Descripcion = null;
	/**@type {String?} */ Mail = null;
	/**@type {String?} */ Estado = null;
	/**@type {String?} */ Password = null;
	/**@type {Array<Security_Users_Roles>?} */ Security_Users_Roles = null;
	/**@type {Array<Tbl_Profiles_Model>?} */ Tbl_Profiles_Model  = null;

	/**@return {Tbl_Profiles_Model?} */
	get ActiveProfile() {
		return this.Tbl_Profiles_Model?.[0] ?? null;
	}
}
export { Security_Users }

class Security_Users_ModelComponent  extends EntityClass{
	/**
	* @param {Partial<Security_Users_ModelComponent>} [props] 
	*/	
	constructor(props) {
		super(props, "EntitySECURITY");
		Object.assign(this, props);
	}	
	/**@type {ModelProperty} */ Id_User = { type: "number", primary: true };
	/**@type {ModelProperty} */ Nombres = { type: "text", label: "Nickname" };
	/**@type {ModelProperty} */ Descripcion = { type: "text", require: false };
	/**@type {ModelProperty} */ Mail = { type: "email" };
	/**@type {ModelProperty} */ Estado = { type: "Select", Dataset: ["ACTIVO", "INACTIVO"] };
	/**@type {ModelProperty} */ Password = { type: "password", hiddenInTable: true };	
	/**@type {ModelProperty} */ Security_Users_Roles = {
		type: "multiselect",  ModelObject: () => new Security_Roles_ModelComponent()
	}
	/**@type {ModelProperty} */ Tbl_Profiles_Model = { type: "MASTERDETAIL", ModelObject: new Tbl_Profiles_Model_ModelComponent(), max: 1 };
}
export { Security_Users_ModelComponent }
class ChangePasswordModel {
	/**
	* @param {Partial<ChangePasswordModel>} [props] 
	*/	
	constructor(props) {
		Object.assign(this, props);
	}
	Id_User = { type: "number", primary: true };
	Password = { type: "password", hiddenInTable: true };
}
export { ChangePasswordModel }
class ChangeStateModel {
	/**
	* @param {Partial<ChangePasswordModel>} [props] 
	*/	
	constructor(props) {
		Object.assign(this, props);
	}
	Id_User = { type: "number", primary: true };
	Estado = { type: "radio", hiddenInTable: true, Dataset: ["ACTIVO", "INACTIVO"] };
}
export { ChangeStateModel }
class ChangeRolesModel {
	/**
	* @param {Partial<ChangeRolesModel>} [props] 
	*/	
	constructor(props) {
		
		Object.assign(this, props);
	}
	/**@type {ModelProperty} */ Id_User = { type: "number", primary: true };
	/**@type {ModelProperty} */ Security_Users_Roles = {
		type: "MULTISELECT", Dataset: [{ Descripcion: "Role 1" }], ModelObject: ()=> new Security_Roles_ModelComponent()
	};
}
export { ChangeRolesModel }