import {initHeader} from './header.js';
import {initHeroAnimation} from './hero.js';
import {initReviewsSlider} from './user-reviews.js?v=1.2';
import {initContact} from "./contact.js";
import {initLogin} from "./login.js";


window.addEventListener('DOMContentLoaded', () => {
    initHeroAnimation();
    initHeader();
    initReviewsSlider();
    initContact();
    initLogin();
})