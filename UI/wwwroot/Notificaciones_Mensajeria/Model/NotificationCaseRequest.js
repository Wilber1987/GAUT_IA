//@ts-check
// @ts-ignore
import { ModelProperty } from "../../WDevCore/WModules/CommonModel.js";
import { EntityClass } from "../../WDevCore/WModules/EntityClass.js";
import { Notificaciones } from "./Notificaciones.js";

export class NotificationCaseRequest_ModelComponent extends EntityClass {
    /** @param {Partial<NotificationCaseRequest_ModelComponent>} [props] */
	constructor(props) {
		super(props, 'Notificaciones');
		// @ts-ignore
		Object.assign(this, props);
	}
    /**@type {ModelProperty} */ notificacion = { type: "MODEL", ModelObject: new Notificaciones() };
}

export class NotificationCaseRequest extends EntityClass {
    /** @param {Partial<NotificationCaseRequest>} [props] */
	constructor(props) {
		super(props, 'Notificaciones');
		// @ts-ignore
		Object.assign(this, props);
	}
    /**@type {Notificaciones?} */ notificacion = null;
}