import {initHeader} from './header.js';
import {initHeroAnimation} from './hero.js';
import {initReviewsSlider} from './user-reviews.js?v=1.2';


window.addEventListener('DOMContentLoaded', () => {
    console.log('DOM fully loaded and parsed');
    initHeroAnimation();
    initHeader();
    initReviewsSlider();
})