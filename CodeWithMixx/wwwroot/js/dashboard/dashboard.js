import {initCharts} from "./charts.js";
import {initHeroAnimation} from "../hero.js";
import {initClasses} from "./classes.js";


window.addEventListener("DOMContentLoaded", () => {
    initCharts();
    initHeroAnimation();
    document.addEventListener("htmx:afterOnLoad", () => {
        initCharts();
    })
   initClasses();
})


