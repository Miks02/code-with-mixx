

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
            case "individual": 
                message.value = "Pozdrav, zainteresovan sam za detalje o individualnim časovima.";
                break;
            case "exam-preparation":
                message.value = "Pozdrav, zainteresovan sam za detalje o paketu za spremanje ispita.";
                break;    
            case "project-assignment":
                message.value = "Pozdrav, zainteresovan sam za detalje o projektu.";
                break;
            default:
                message.value = "";
        }
    })
    

    
}