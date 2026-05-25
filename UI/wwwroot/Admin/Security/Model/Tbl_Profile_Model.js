//@ts-check
//import { Cat_Dependencias, Tbl_Servicios } from "../../ModelProyect/Tbl_CaseModule.js";

import { WForm } from "../../../WDevCore/WComponents/WForm.js";
// @ts-ignore
import { ModelProperty } from "../../../WDevCore/WModules/CommonModel.js";
import { EntityClass } from "../../../WDevCore/WModules/EntityClass.js";
import { WAjaxTools } from "../../../WDevCore/WModules/WAjaxTools.js";
import { Cat_Dependencias, Cat_Dependencias_ModelComponent } from "../OrganizationModel/Cat_Dependencias.js";
import { Tbl_Servicios, Tbl_Servicios_ModelComponent } from "../OrganizationModel/Tbl_Servicios.js";


class Tbl_Profiles_Model_ModelComponent extends EntityClass {
    /**
     * @param {Partial<Tbl_Profiles_Model_ModelComponent>} [props]
     */
    constructor(props) {
        super(props, 'Organization');
        Object.assign(this, props);
    }
    /**@type {ModelProperty}*/ Foto = { type: 'img', require: false, hiddenFilter: true };
    /**@type {ModelProperty}*/ Id_Perfil = { type: 'number', primary: true, hiddenFilter: true };
    /**@type {ModelProperty}*/ Nombres = { type: 'text' };
    /**@type {ModelProperty}*/ Apellidos = { type: 'text' };
    /**@type {ModelProperty}*/ FechaNac = { type: 'date', label: "fecha de nacimiento", hiddenFilter: true };
    /**@type {ModelProperty}*/ Sexo = { type: "Select", Dataset: ["Masculino", "Femenino"], hiddenFilter: true };
    /**@type {ModelProperty}*/ DNI = { type: 'text' };

    /**@type {ModelProperty}*/ Correo_institucional = { type: 'text', label: "correo", disabled: true, hidden: true };
    /**@type {ModelProperty}*/ Estado = { type: "Select", Dataset: ["ACTIVO", "INACTIVO"] };

    /** campos de investigaciones */
    //**@type {ModelProperty}*/ Tbl_Grupos_Profiles = { type: 'masterdetail', require: false , ModelObject: ()=> new Tbl_Grupos_Profiles_ModelComponent() };
    /**@type {ModelProperty}*/ ORCID = { type: 'text', require: false, hiddenFilter: true };

}

export { Tbl_Profiles_Model_ModelComponent };


export class Tbl_Profiles_Model extends EntityClass {
    /** @param {Partial<Tbl_Profiles_Model>} [props] */
    constructor(props) {
        super(props, "Organization")
        /** @type {number|null} */
        this.Id_Perfil = null;

        /** @type {string|null} */
        this.Nombres = null;

        /** @type {string|null} */
        this.Apellidos = null;

        /** @type {Date|null} */
        this.FechaNac = null;

        /** @type {number|null} */
        this.IdUser = null;

        /** @type {string|null} */
        this.Sexo = null;

        /** @type {string|null} */
        this.Foto = null;

        /** @type {string|null} */
        this.DNI = null;

        /** @type {string|null} */
        this.Correo_institucional = null;

        /** @type {string|null} */
        this.Estado = null;

        // -------- PROPIEDADES EXTRA DE LA SUBCLASE --------

        /** @type {number|null} */
        this.Id_Pais_Origen = null;

        /** @type {number|null} */
        this.Id_Institucion = null;

        /** @type {string|null} */
        this.Indice_H = null;

        /** @type {string|null} */
        this.ORCID = null;

        // -------- RELACIONES --------

        /** @type {Object|null} */
        this.Security_Users = null;

        /** @type {Object|null} */
        this.Cat_Paises = null;

        /** @type {Array<Object>|null} */
        this.Tbl_Case = null;

        /** @type {Array<Object>|null} */
        this.Tbl_Agenda = null;

        /** @type {Array<Tbl_Dependencias_Usuarios>|null} */
        this.Tbl_Dependencias_Usuarios = null;

        /** @type {Array<Tbl_Servicios_Profile>|null} */
        this.Tbl_Servicios_Profile = null;

        /** @type {Array<Object>|null} */
        this.Tbl_Grupos_Profiles = null;

        /** @type {Array<Object>|null} */
        this.Tbl_Participantes = null;
        Object.assign(this, props);
    }

}
export class Tbl_Dependencias_Usuarios {
    /** @param {Partial<Tbl_Dependencias_Usuarios>} [props] */
    constructor(props) {
        Object.assign(this, props);
    }
    /**@type {Number?} */ Id_Perfil = null;
    /**@type {Number?} */ Id_Dependencia = null;
    /**@type {Cat_Dependencias?} */ Cat_Dependencias = null;
}
export class Tbl_Dependencias_Usuarios_ModelComponent {
    /** @param {Partial<Tbl_Dependencias_Usuarios_ModelComponent>} [props] */
    constructor(props) {
        Object.assign(this, props);
    }
    /**@type {ModelProperty} */ Id_Perfil = { type: "Number", primary: true };
    /**@type {ModelProperty} */ Id_Dependencia = { type: "Number", primary: true }

    //**@type {ModelProperty} */ Tbl_Profile = { type: "WSelect", ModelObject: ()=> new Tbl_Profiles_Model(), hidden: true };
    /**@type {ModelProperty} */ Cat_Dependencias = { type: "WSelect", ModelObject: () => new Cat_Dependencias_ModelComponent() };
}
export class Tbl_Servicios_Profile {
    /** @param {Partial<Tbl_Servicios_Profile>} [props] */
    constructor(props) {
        Object.assign(this, props);
    }
   /**@type {Number?} */ Id_Perfil = null;
   /**@type {Number?} */ Id_Servicio = null;

    //**@type {ModelProperty} */ Tbl_Profile = null;
    /**@type {Tbl_Servicios?} */ Tbl_Servicios = null;
}

export class Tbl_Servicios_Profile_ModelComponent {
    /** @param {Partial<Tbl_Servicios_Profile_ModelComponent>} [props] */
    constructor(props) {
        Object.assign(this, props);
    }
    /**@type {ModelProperty} */ Id_Perfil = { type: "Number", primary: true };
    /**@type {ModelProperty} */ Id_Servicio = { type: "Number", primary: true }

    //**@type {ModelProperty} */ Tbl_Profile = { type: "WSelect", ModelObject: ()=> new Tbl_Profiles_Model(), hidden: true };
    /**@type {ModelProperty} */ Tbl_Servicios = { type: "WSelect", ModelObject: () => new Tbl_Servicios_ModelComponent() };
}

export class Tbl_Profiles_Services_Manager {
    /**
     * @param {Partial<Tbl_Profiles_Services_Manager>} [props]
     */
    constructor(props) {
        Object.assign(this, props);
    }
    /**@type {ModelProperty}*/ Id_Perfil = { type: 'number', primary: true, hiddenFilter: true };
    //PROPIEDADES DE HELPDESK
    /**@type {ModelProperty}*/ Tbl_Dependencias_Usuarios = {
        type: 'masterdetail',
        hiddenFilter: true,
        ModelObject: () => new Tbl_Dependencias_Usuarios_ModelComponent(),
        require: false,
        action: async (/**@type {Tbl_Profiles_Model} */ Profile, /** @type {WForm} */ Form) => {
            if (Profile?.Tbl_Dependencias_Usuarios?.length ?? 0 > 0) {
                const servicios = await new Tbl_Servicios_ModelComponent({
                    FilterData: [{
                        PropName: "Id_Dependencia", FilterType: "in", Values:
                            Profile.Tbl_Dependencias_Usuarios?.map(d => d.Cat_Dependencias?.Id_Dependencia?.toString())

                    }]
                }).Get();
                this.Tbl_Servicios_Profile.ModelObject.Tbl_Servicios.Dataset = servicios;
                this.Tbl_Servicios_Profile.disabled = false;
                Profile.Tbl_Servicios_Profile?.forEach(servicio => {
                    const servicioF = Profile.Tbl_Dependencias_Usuarios?.map(d => d.Cat_Dependencias?.Id_Dependencia).includes(servicio.Tbl_Servicios?.Id_Dependencia ?? -1)
                    if (!servicioF) {
                        let filtObject = Profile.Tbl_Servicios_Profile?.indexOf(servicio);
                        // @ts-ignore
                        Profile.Tbl_Servicios_Profile?.splice(filtObject, 1);
                    }
                });
                Form.DrawComponent();
            } else {
                this.Tbl_Servicios_Profile.disabled = true;
                this.Tbl_Servicios_Profile.Dataset = [];
                Profile.Tbl_Servicios_Profile = [];
                Form.DrawComponent();
            }
        }
    }
    /**@type {ModelProperty}*/ Tbl_Servicios_Profile = {
        type: 'masterdetail', hiddenFilter: true,
        ModelObject: new Tbl_Servicios_Profile_ModelComponent,
        require: false,
        disabled: (/**@type {Tbl_Profiles_Model} */  Profile, /** @type {WForm} */ Form) => {
            return Profile.Tbl_Servicios_Profile?.length == 0
        }, action: (/**@type {Tbl_Profiles_Model} */ Profile, /** @type {WForm} */ Form) => {
            const mapServices = [...new Map(
                    Profile.Tbl_Servicios_Profile?.map(item => [item.Tbl_Servicios?.Id_Servicio, item])
                ).values()]
            // @ts-ignore
            Profile.Tbl_Servicios_Profile.length = 0
            Profile.Tbl_Servicios_Profile?.push(...mapServices)
            console.log(Profile.Tbl_Servicios_Profile);            
        }
    }

}