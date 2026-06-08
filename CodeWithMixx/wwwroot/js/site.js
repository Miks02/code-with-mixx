import {initHeader} from './header.js';
import {initHeroAnimation} from './hero.js';


window.addEventListener('DOMContentLoaded', () => {
    console.log('DOM fully loaded and parsed');
    initHeroAnimation();
    initHeader();
})