
export function initReviewsSlider() {
    const sliderContainer = document.getElementById("reviews-slider-container");
    if (!sliderContainer) {
        console.log("Reviews slider container not found");
        return;
    }

    const cards = sliderContainer.querySelectorAll(".review-card");
    const prevBtn = document.getElementById("reviews-prev");
    const nextBtn = document.getElementById("reviews-next");
    
    if (!cards.length) {
        console.log("No cards found in Reviews Slider");
        return;
    }
    
    if (!prevBtn || !nextBtn) {
        console.log("Navigation buttons not found for Reviews Slider");
        return;
    }

    let currentIndex = 0;

    function updateSlider() {
        if (window.innerWidth >= 768) {
            cards.forEach(card => {
                card.classList.remove("hidden");
                card.classList.add("flex");
                card.style.opacity = "1";
                card.style.display = ""; 
            });
            return;
        }

        cards.forEach((card, index) => {
            if (index === currentIndex) {
                card.classList.remove("hidden");
                card.classList.add("flex");
                card.style.display = "flex";
                setTimeout(() => {
                    card.style.opacity = "1";
                    card.classList.remove("opacity-0");
                    card.classList.add("opacity-100");
                }, 10);
            } else {
                card.classList.add("hidden");
                card.classList.remove("flex", "opacity-100");
                card.classList.add("opacity-0");
                card.style.display = "none";
                card.style.opacity = "0";
            }
        });
    }

    prevBtn.addEventListener("click", () => {
        currentIndex = (currentIndex - 1 + cards.length) % cards.length;
        updateSlider();
    });

    nextBtn.addEventListener("click", () => {
        currentIndex = (currentIndex + 1) % cards.length;
        updateSlider();
    });

    window.addEventListener("resize", updateSlider);
    updateSlider();
}
