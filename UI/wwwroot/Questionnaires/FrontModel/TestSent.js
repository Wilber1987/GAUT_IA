//@ts-check
// @ts-ignore
import { ModelProperty } from "../../WDevCore/WModules/CommonModel.js";
import { EntityClass } from "../../WDevCore/WModules/EntityClass.js";
import { DateTime } from "../../WDevCore/WModules/Types/DateTime.js";


export class TestSent extends EntityClass {
    /** @param {Partial<TestSent>} [props] */
    constructor(props) {
        super(props, 'TestSentManager');
        Object.assign(this, props);
    }
    /**@type {Number?}*/ Id_test = null;
    /**@type {Number?}*/ IdTestSent = null;
    /**@type {String?}*/ Token = null;
    /**@type {String?}*/ PhoneNumber = null;
    /**@type {String?}*/ Email = null;
    /**@type {String?}*/ State = null;
    /**@type {DateTime?}*/ ShippingDate = null;
    /**@type {DateTime?}*/ DateOfFilling = null;
}


export class TestSent_ModelComponent extends EntityClass {
    /** @param {Partial<TestSent_ModelComponent>} [props] */
    constructor(props) {
        super(props, 'TestSentManager');
        Object.assign(this, props);
    }
    /**@type {ModelProperty}*/ Id_test = { type: "Number", hidden: true };
    /**@type {ModelProperty}*/ IdTestSent = { type: "Number", primary: true };
    //**@type {ModelProperty}*/ Token = { type: "Text" };
    //**@type {ModelProperty}*/ PhoneNumber = { type: "Text", label: "Telefono" };
    /**@type {ModelProperty}*/ Email = { type: "Text" };
    //**@type {ModelProperty}*/ State = { type: "Text" };
    //**@type {ModelProperty}*/ ShippingDate = { type: "Date" };
    //**@type {ModelProperty}*/ DateOfFilling = { type: "Date" };
}

