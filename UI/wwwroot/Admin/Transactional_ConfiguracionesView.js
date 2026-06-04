//@ts-check
import { StylesControlsV2, StyleScrolls } from "../WDevCore/StyleModules/WStyleComponents.js";
import { ModalVericateAction } from "../WDevCore/WComponents/ModalVericateAction.js";
import { WAlertMessage } from "../WDevCore/WComponents/WAlertMessage.js";
import { WFilterOptions } from "../WDevCore/WComponents/WFilterControls.js";
import { WModalForm } from "../WDevCore/WComponents/WModalForm.js";
import { WTableComponent } from "../WDevCore/WComponents/WTableComponent.js";
// @ts-ignore
import { ModelProperty } from "../WDevCore/WModules/CommonModel.js";
import { EntityClass } from "../WDevCore/WModules/EntityClass.js";
import { WRender } from "../WDevCore/WModules/WComponentsTools.js";
class Transactional_ConfiguracionesView extends HTMLElement {
    constructor() {
        super();
        this.Draw();
    }
    Draw = async () => {
        const model = new Transactional_Configuraciones_ModelComponent();
        const dataset = await model.Get();
        this.TabContainer = WRender.Create({ type: 'div',  class: 'TabContainer', id: 'TabContainer'  })
        this.MainComponent = new WTableComponent({
            ModelObject: model, Dataset: dataset, Options: {
                UrlUpdate: "../api/ApiEntityADMINISTRATIVE_ACCESS/updateTransactional_Configuraciones",
                UseManualControlForFiltering: true,
                UserActions: [
                    {
                        name: "Editar", action: (/** @type {any} */ element) => {
                            this.append(new WModalForm({
                                AutoSave: false,
                                ModelObject: new Transactional_Configuraciones_ModelComponent({
                                    Valor: { type: this.ConfigType(element),  require: false }
                                }),
                                EditObject: element, ObjectOptions: {                                    
                                    SaveFunction: (/**@type {Transactional_Configuraciones} */ editObject) => {
                                        this.append(ModalVericateAction(async ()=> {
                                            editObject.Valor = editObject.Valor?.toString() ?? "";
                                            const response = await editObject.Update();
                                            WAlertMessage.ResponseMessage(response);
                                            setTimeout(() => {
                                               window.location.reload()
                                            }, 1000);                                           

                                        }, "Esta seguro de guardar los cambios?"))
                                       
                                    }
                                }
                            }))
                        }
                    }
                ]

            }
        })
        this.TabContainer.append(this.MainComponent)
        this.FilterOptions = new WFilterOptions({
            Dataset: dataset,
            ModelObject: model,
            UseManualControlForFiltering: true,
            FilterFunction: (/** @type {any[] | undefined} */ DFilt) => {
                this.MainComponent?.DrawTable(DFilt);
            }
        });
        this.append(
            StylesControlsV2.cloneNode(true),
            StyleScrolls.cloneNode(true),
            this.FilterOptions,
            this.TabContainer
        );
    }
    /**
     * @param {Transactional_Configuraciones} element
     */
    ConfigType(element) {
        if (this.IsImage(element)) {
            return "IMG";
        } else if (this.IsDrawImage(element)) {
            return "DRAW";
        } else if (this.IsNumber(element)) {
            return "NUMBER";
        } else if (this.IsBoolean(element)) {
            // @ts-ignore
            element.Valor = element.Valor == "true" || element.Valor == "1" ? true : false
            return "CHECKBOX";
        } else if (this.ComplexText(element)) {
            return "RICHTEXT";
        } else if (this.IsSelectRadio(element)) {
            return "RADIO";
        }
        return "TEXTAREA"
    }
    /**
     * @param {Transactional_Configuraciones} element
     */
    IsSelectRadio(element) {
        return element.Tipo_Configuracion == "RADIO" || element.Tipo_Configuracion == "SELECT"
    }
    /**
     * @param {Transactional_Configuraciones} element
     */
    ComplexText(element) {
        return element.Tipo_Configuracion == "RICHTEXT"
            || element.Tipo_Configuracion == "TEMPLATE";
    }
    /**
     * @param {Transactional_Configuraciones} element
     */
    IsBoolean(element) {
        return element.Tipo_Configuracion == "CHECKBOX"
            || element.Tipo_Configuracion == "BOOL"
            || element.Tipo_Configuracion == "BOOLEAN"
    }

    /**
     * @param {Transactional_Configuraciones} element
     */
    IsNumber(element) {
        return element.Tipo_Configuracion == "INTERESES" ||
            element.Tipo_Configuracion == "BENEFICIOS" ||
            element.Tipo_Configuracion == "NUMBER";
    }
    /**
     * @param {Transactional_Configuraciones} element
     */
    IsDrawImage(element) {
        return element.Nombre == "FIRMA_DIGITAL_APODERADO" ||
            element.Nombre == "FIRMA_DIGITAL_APODERADO_VICEPRESIDENTE"
    }
    /**
     * @param {Transactional_Configuraciones} element
     */
    IsImage(element) {
        return element.Nombre == "LOGO"
    }
}
customElements.define('w-transactional_configuraciones', Transactional_ConfiguracionesView);
export { Transactional_ConfiguracionesView };

class Transactional_Configuraciones_ModelComponent extends EntityClass {
    
    /**
     * @param {Partial<Transactional_Configuraciones_ModelComponent>} [props]
     */
    constructor(props) {
        super(props, 'EntityADMINISTRATIVE_ACCESS');
        Object.assign(this, props);
    }
    //**@type {ModelProperty} */ property = { type: "TEXT"};    
    /**@type {ModelProperty} */ Id_Configuracion = { type: 'number', primary: true };
    /**@type {ModelProperty} */ Nombre = { type: 'text', disabled: true };
    /**@type {ModelProperty} */ Descripcion = { type: 'text', disabled: true };
    /**@type {ModelProperty} */ Valor = { type: 'text', require: false };
    /**@type {ModelProperty} */ Tipo_Configuracion = { type: 'text', disabled: true, hiddenInTable: true };
}
export { Transactional_Configuraciones_ModelComponent };

class Transactional_Configuraciones extends EntityClass {
    /**
     * @param {Partial<Transactional_Configuraciones>} [props]
     */
    constructor(props) {
        super(props, 'EntityADMINISTRATIVE_ACCESS');
        Object.assign(this, props);
    }
    /**@type {Number?} */ Id_Configuracion = null;
    /**@type {String?} */ Nombre = null;
    /**@type {String?} */ Descripcion = null;
    /**@type {String?} */ Valor = null;
    /**@type {String?} */ Tipo_Configuracion = null;
}
export { Transactional_Configuraciones };

