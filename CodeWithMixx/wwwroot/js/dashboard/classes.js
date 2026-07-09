

 
 export function initCreateClass() {
     const classForm = document.getElementById("create-class-form");

     if(!classForm) {
        return;
     }
     const classPrice = document.getElementById("class-price");
     const numberOfClasses = document.getElementById("class-count");
     const paidAmount = document.getElementById("paid-amount");
     const info = document.getElementById("paid-amount-info");
     
     let previewClassCount = document.getElementById("preview-class-count");
     let previewTotalPrice = document.getElementById("preview-total");
     let previewPaidAmount = document.getElementById("preview-paid");
     let previewRemaining = document.getElementById("preview-remaining");
     

     let totalPrice = document.getElementById("total-price");
     totalPrice.value = Number(classPrice.value * numberOfClasses.value).toFixed(2);
     previewTotalPrice.textContent = totalPrice.value;
     calculateEndTime();

     classForm.addEventListener("input", (e) => {
         if (e.target.id === "class-count") {
             totalPrice.value = Number(e.target.value * classPrice.value).toFixed(2);
             console.log("Total price: " + totalPrice.value);
             previewClassCount.textContent = e.target.value;
             previewTotalPrice.textContent = totalPrice.value;
             togglePaidAmountClasses(info, paidAmount.value, totalPrice.value);
             calculateEndTime();
             return;
         }
         if (e.target.id === "FirstClassStart") {
             calculateEndTime();
             return;
         }
         if (e.target.id === "class-price") {
             totalPrice.value = (e.target.value * numberOfClasses.value).toFixed(2);
             previewTotalPrice.textContent = totalPrice.value;
             togglePaidAmountClasses(info, paidAmount.value, totalPrice.value);
             return;
         }
         if (e.target.id === "paid-amount") {
             previewPaidAmount.textContent = paidAmount.value;
             previewRemaining.textContent = Number(totalPrice.value - paidAmount.value).toFixed(2);
             togglePaidAmountClasses(info, paidAmount.value, totalPrice.value);
             return;
         }
         if (e.target.id === "total-price") {
             previewTotalPrice.textContent = totalPrice.value;
             previewRemaining.textContent = (totalPrice.value - paidAmount.value).toFixed(2);
             return;
         }
     })
 }

 function calculateEndTime() {
     const classCountInput = document.getElementById("class-count");
     const firstClassStartInput = document.getElementById("FirstClassStart");
     const lastClassEndInput = document.getElementById("last-class-end");


     if (!classCountInput || !firstClassStartInput || !lastClassEndInput) return;

     const classCount = parseFloat(classCountInput.value) || 0;
     const startsAtStr = firstClassStartInput.value;

     if (!startsAtStr || classCount <= 0) {
         lastClassEndInput.value = "";
         return;
     }

     const startsAt = new Date(startsAtStr);
     if (isNaN(startsAt.getTime())) {
         lastClassEndInput.value = "";
         return;
     }
     const durationInMinutes = classCount * 45;
     const endsAt = new Date(startsAt.getTime() + durationInMinutes * 60000);

     const tzOffset = endsAt.getTimezoneOffset() * 60000;
     const localISOTime = (new Date(endsAt.getTime() - tzOffset)).toISOString().slice(0, 16);

     lastClassEndInput.value = localISOTime;
 }
 
function togglePaidAmountClasses(paidAmountInfo, paidAmount, totalPrice) {

    paidAmount = parseFloat(paidAmount) || 0;
    totalPrice = parseFloat(totalPrice) || 0;

    const isDebt = paidAmount < totalPrice;
    const isBonus = paidAmount > totalPrice;

    if (!paidAmount) {
        paidAmountInfo.textContent = "";
        return;
    }
    if (isDebt) {
        paidAmountInfo.classList.remove("text-green-400");
        paidAmountInfo.textContent = "Preostalo za uplatu " + (totalPrice - paidAmount) + " RSD";
        paidAmountInfo.classList.add("text-amber-400");
        return;
    }
    if (isBonus) {
        paidAmountInfo.classList.remove("text-amber-400!")
        paidAmountInfo.textContent = "Uračunat je bonus od " + (paidAmount - totalPrice) + " RSD.";
        paidAmountInfo.classList.add("text-green-400");
        return;
    }

    paidAmountInfo.textContent = "Iznos je isplaćen u celosti";
 }
 
 
 