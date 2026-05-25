//@ts-check
import { WRender } from '../WDevCore/WModules/WComponentsTools.js';
import { WAppNavigator } from "../WDevCore/WComponents/WAppNavigator.js";
import { Transactional_ConfiguracionesView } from './Transactional_ConfiguracionesView.js';
import { LogType, LogView } from './LogErrorView.js';
//const DOMManager = new ComponentsManager({ MainContainer: Main, SPAManage: true });
window.addEventListener("load", async () => {
    // @ts-ignore
    MainContainer.append(WRender.Create({ tagName: "h3", innerText: "Mantenimiento" }));
    // @ts-ignore
    MainContainer.append(new WAppNavigator({
        DarkMode: false,
        NavStyle: "tab",
        Inicialize: true,
        Elements: [
            {
                name: "Config", action: () => {
                    return  new Transactional_ConfiguracionesView();
                }
            }, {
                name: "Acciones", action: () => {
                    return  new LogView({ Type: LogType.ACTION });
                }
            }, {
                name: "Errores", action: () => {
                    return  new LogView({ Type: LogType.ERROR });
                }
            }
        ]
    }));
});