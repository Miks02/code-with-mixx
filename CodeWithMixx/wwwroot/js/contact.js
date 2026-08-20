

export function initContact() {
    const pricing = document.getElementById("pricing-grid");
    if (!pricing) return;

    pricing.addEventListener("click", (e) => {
        const target = e.target.closest(".pricing-button");
        if (!target) return;

        const contactForm = document.getElementById("contact");
        if (!contactForm) return;

        const queryType = contactForm.querySelector("#type");
        const message = contactForm.querySelector("#message");

        if (queryType) {
            queryType.value = target.dataset.type;
        }

        if (message) {
            switch(target.dataset.type) {
                case "Individual": 
                    message.value = "Pozdrav, treba mi malo pomoći oko nekih oblasti, pa me zanimaju detalji za individualne časove.";
                    break;
                case "ExamPreparation":
                    message.value = "Pozdrav, u frci sam sa odredjenim ispitom pa me zanima tačno kako funkcioniše paket spremanja ispita.";
                    break;    
                case "Project":
                    message.value = "Pozdrav, imam seminarski rad koji moram hitno da rešim pa me zanimaju odredjeni detalji.";
                    break;
                default:
                    message.value = "";
            }
        }
    });
}