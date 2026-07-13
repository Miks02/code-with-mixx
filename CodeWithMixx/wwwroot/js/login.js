
export function initLogin() {
    const form = document.getElementById("login-form");
    
    if(!form) return;

    const loginBtn = document.getElementById('login-btn');
    
    loginBtn.addEventListener("click", (e) => {
        e.preventDefault();

        const emailValidation = document.getElementById('email-validation');
        const passwordValidation = document.getElementById('password-validation');
        
        const email = document.getElementById('login-email').value.trim();
        const password = document.getElementById('login-password').value.trim();
        
        if(!validateLoginCredentials(email, password, emailValidation, passwordValidation)) 
            return;
        
        form.submit();
    })
}

function validateLoginCredentials(email, password, emailValidation, passwordValidation) {
    const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;

    if(!email && !password) {
        emailValidation.textContent = "Email adresa je obavezna.";
        passwordValidation.textContent = "Lozinka je obavezna.";
        return false;
    }

    if(!email) {
        emailValidation.textContent = "Email adresa je obavezna.";
        return false;
    }

    if(!emailRegex.test(email)) {
        emailValidation.textContent = "Nevalidan format email adrese.";
        return false;
    }

    if(!password) {
        passwordValidation.textContent = "Lozinka je obavezna.";
        return false;
    }
    
    return true;
}

