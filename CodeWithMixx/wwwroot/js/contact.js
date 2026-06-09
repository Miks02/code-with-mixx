

export function initContact() {
    console.log("Initializing Contact...");
    
    const contactForm = document.getElementById("contact");
    const queryType = contactForm.querySelector("#type");
    const pricing = document.getElementById("pricing-grid");
    const message = contactForm.querySelector("#message");
    
    pricing.addEventListener("click", (e) => {
        const target = e.target.closest(".pricing-button")
        
        if(!target)
            return;
        
        queryType.value = target.dataset.type;
        
        
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
    })
    

    
}