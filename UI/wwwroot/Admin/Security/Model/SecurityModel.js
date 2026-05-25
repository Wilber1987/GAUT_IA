//@ts-check
// @ts-ignore
import { ModelProperty } from "../../../WDevCore/WModules/CommonModel.js";
import { EntityClass } from "../../../WDevCore/WModules/EntityClass.js";


class Security_Roles extends EntityClass {
	/**
	* @param {Partial<Security_Roles>} [props] 
	*/
	constructor(props) {
		super(props, "EntitySECURITY");
		Object.assign(this, props);
	}
	/**@type {Number?} */ Id_Role = null;
	/**@type {String?} */ Descripcion = null;
	/**@type {String?} */ Security_Permissions_Roles = null;
	/**@type {String?} */ Estado = null;
}
export { Security_Roles }

class Security_Roles_ModelComponent extends EntityClass {
	/**
	* @param {Partial<Security_Roles_ModelComponent>} [props] 
	*/
	constructor(props) {
		super(props, "EntitySECURITY");
		Object.assign(this, props);
	}
	Id_Role = { type: "number", primary: true };
	Descripcion = { type: "text" };
	Estado = { type: "Select", Dataset: ["ACTIVO", "INACTIVO"] };
	Security_Permissions_Roles = {
		type: "MASTERDETAIL", ModelObject: new Security_Permissions_Roles_ModelComponent()
	};

}
export { Security_Roles_ModelComponent }
class Security_Permissions extends EntityClass {
	/**
	* @param {Partial<Security_Permissions>} [props] 
	*/
	constructor(props) {
		super(props, "EntitySECURITY");
		Object.assign(this, props);
	}
	/**@type {Number?} */ Id_Permission = null;
	/**@type {String?} */ Descripcion = null;
	/**@type {String?} */ Detalles = null;
	/**@type {String?} */ Estado = null;
}
export { Security_Permissions }

class Security_Permissions_ModelComponent extends EntityClass {
	/**
	* @param {Partial<Security_Permissions_ModelComponent>} [props] 
	*/
	constructor(props) {
		super(props, "EntitySECURITY");
		Object.assign(this, props);
	}
	Id_Permission = { type: "number", primary: true };
	/**@type {ModelProperty} */ Descripcion = { type: "text", disabled: true };
	/**@type {ModelProperty} */ Detalles = { type: "text" };
	//Estado = { type: "Select", Dataset: ["ACTIVO", "INACTIVO"] };
}
export { Security_Permissions_ModelComponent }

class Security_Permissions_Roles {
	/**
	* @param {Partial<Security_Permissions_Roles>} [props] 
	*/
	constructor(props) {
		Object.assign(this, props);
	}
	Id_Role = { type: "number", primary: true };
	Id_Permission = { type: "number", primary: true };
	Estado = { type: "Select", Dataset: ["ACTIVO", "INACTIVO"] };
}
export { Security_Permissions_Roles }

class Security_Permissions_Roles_ModelComponent {
	/**
	* @param {Partial<Security_Permissions_Roles_ModelComponent>} [props] 
	*/
	constructor(props) {
		Object.assign(this, props);
	}
	Id_Role = { type: "number", primary: true };
	Id_Permission = { type: "number", primary: true };
	//Estado = { type: "Select", Dataset: ["ACTIVO", "INACTIVO"] };
	/**@type {ModelProperty} */ Security_Permissions = { type: "wselect", ModelObject: ()=> new Security_Permissions_ModelComponent()};
	
	
}
export { Security_Permissions_Roles_ModelComponent }

class Security_Users_Roles {
	/**
	* @param {Partial<Security_Users_Roles>} [props] 
	*/
	constructor(props) {
		Object.assign(this, props);
	}
	/**@type {Number?} */ Id_Role = null;
	/**@type {Number?} */ Id_User = null;
	/**@type {Security_Roles?} */ Security_Role = null;

	get Descripcion() {
		return this.Security_Role?.Descripcion;
	}

	///**@type {Number} */ Estado = null;
}
export { Security_Users_Roles }

class Security_Users_Roles_ModelComponent {
	/**
	* @param {Partial<Security_Users_Roles_ModelComponent>} [props] 
	*/
	constructor(props) {
		Object.assign(this, props);
	}
	Id_Role = { type: "number", primary: true };
	Id_User = { type: "number", primary: true };
	Estado = { type: "Select", Dataset: ["ACTIVO", "INACTIVO"] };
}
export { Security_Users_Roles_ModelComponent }




