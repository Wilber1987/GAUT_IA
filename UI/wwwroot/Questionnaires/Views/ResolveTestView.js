//@ts-check
import { ResolveTestViewComponent } from "./Component/ResolveTestViewComponent.js";
window.onload = ()=>{
    // @ts-ignore
    MainContainer.append(new ResolveTestViewComponent({}));
}
