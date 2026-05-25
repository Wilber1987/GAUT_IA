//@ts-check
import { FilterData } from "../../WDevCore/WModules/CommonModel.js";
import { css } from "../../WDevCore/WModules/WStyledRender.js";
import { Tests } from "../FrontModel/Tests.js";
import { TestBuilderUtil } from "../TestBuilderUtil.js";

class ResolveForm extends HTMLElement {
    /**
     * @typedef {Object} ComponentsConfig 
        * @property {Tests} test
    **/
    /**
    * @param {ComponentsConfig} Config 
    */
    constructor(Config) {
        super();
        this.Config = Config
        this.token = sessionStorage.getItem("to")
        this.idt = sessionStorage.getItem("id")
        this.append(this.CustomStyle);
        this.Draw();
    }
    connectedCallback() { }
    Draw = async () => { 
       // const test = await new Tests().Find( FilterData.Equal("Id_test", this.id));
       TestBuilderUtil.BuildTest(this.Config.test, this, this.token);
    }

    update() {
        this.Draw();
    }
    CustomStyle = css`
         .component{
            display: block;
         }           
     `
}
customElements.define('w-resolve-form', ResolveForm);
export { ResolveForm }