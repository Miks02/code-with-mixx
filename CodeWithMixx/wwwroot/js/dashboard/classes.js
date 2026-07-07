

 export function initClasses() {
    initCreateClass();
 }
 
 function initCreateClass() {
     const classForm = document.getElementById("create-class-form");

     if(!classForm) {
        return;
     }
     const classPrice = document.getElementById("class-price");
     const numberOfClasses = document.getElementById("class-count");

     let totalPrice = document.getElementById("total-price");
     totalPrice.value = classPrice.value * numberOfClasses.value;

     classForm.addEventListener("input", (e) => {
         if(e.target.id !== "class-count" && e.target.id !== "class-price") {
             return;
         }
         if(e.target.id === "class-count") {
             totalPrice.value = e.target.value * classPrice.value;
             return;
         }
         if(e.target.id === "class-price") {
             totalPrice.value = e.target.value * numberOfClasses.value;
             return;
         }
     })
 }
 