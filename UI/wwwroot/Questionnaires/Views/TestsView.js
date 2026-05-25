//@ts-check
import { TestsViewManager } from "./Component/TestsViewManager.js";
window.onload = ()=>{
    // @ts-ignore
    MainContainer.append(new TestsViewManager({}));
}
