

export function initHeader() {
    const header = document.getElementById("header");

    if(!header) {
        console.log("Header not found")
        return;
    }
    const mobileNavbar = document.getElementById("mobile-navbar");
    const openNavbar = document.getElementById("open-navbar");
    const closeNavbar = document.getElementById("close-navbar");
    
    openNavbar.addEventListener("click", () => {
        openNavbar.classList.add("hidden");
        closeNavbar.classList.remove("hidden");
        mobileNavbar.classList.remove("max-h-0", "opacity-0");
        mobileNavbar.classList.add("max-h-[500px]", "opacity-100");
    })
    
    closeNavbar.addEventListener("click", () => {
        openNavbar.classList.remove("hidden");
        closeNavbar.classList.add("hidden");
        mobileNavbar.classList.add("max-h-0", "opacity-0");
        mobileNavbar.classList.remove("max-h-[500px]", "opacity-100");
    })

}