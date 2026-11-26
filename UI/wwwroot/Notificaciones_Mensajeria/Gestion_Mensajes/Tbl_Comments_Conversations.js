import {EntityClass} from "../../WDevCore/WModules/EntityClass.js";

// ============================================
// VERSIÓN _ModelComponents (Componente)
// ============================================
class Tbl_Comments_Conversations_ModelComponent extends EntityClass {
    constructor(props) {
        super(props, 'EntityHelpdesk');
        Object.assign(this, props)
    }

    Id_Conversacion = {type: "number", primary: true};
    Estado = {type: "text"};
    Nombre = {type: "text"};
    Token = {type: "text"};
    CreatedAt = {type: "date"};
    Id_Perfil = {type: "number"};
}

// ============================================
// VERSIÓN NORMAL (Entidad completa)
// ============================================
class Tbl_Comments_Conversations extends EntityClass {
    /** @param {Partial<Tbl_Comments_Conversations>} [props] */
    constructor(props) {
        super(props, 'EntityHelpdesk');
        Object.assign(this, props);
    }

    /**@type {Number}*/ Id_Conversacion;
    /**@type {String}*/ Estado;
    /**@type {String}*/ Nombre;
    /**@type {String}*/ Token;
    /**@type {Date}*/ CreatedAt;
    /**@type {Number}*/ Id_Perfil;
}

export {Tbl_Comments_Conversations_ModelComponent, Tbl_Comments_Conversations};
