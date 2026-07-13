using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Auth.Login;

public class IndexModel(IValidator<LoginInputModel> loginValidator, LoginHandler handler) : PageModel
{
    [BindProperty]
    public LoginInputModel Input { get; set; } = null!;

    public IActionResult OnGet()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToPage("/Index");

        return Page();
    }
    
    public async Task<IActionResult> OnPost(CancellationToken ct = default)
    {
        var validationResult = await loginValidator.ValidateAsync(Input, ct);

        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(x => ModelState.AddModelError($"Input.{x.PropertyName}", x.ErrorMessage));
            return Page();
        }
        
        var result = await handler.HandleAsync(Input, ct);

        if (!result.IsSuccess)
        {
            var errorCode = result.Errors[0].Code;
            var errorMessage = errorCode switch
            {
                "Auth.LoginFailed" => "Email adresa ili lozinka nisu validni.",
                "Auth.AccountDeactivated" => "Nalog je deaktiviran zbog neaktivnosti, obrati se administratoru.",
                "Auth.AccountSuspended" => "Nalog je suspendovan, obrati se administratoru.",
                "Auth.AccountLocked" => "Nalog je zaključan na 5 minuta zbog previše pokušaja",
                _ => "Došlo je do neočekivane greške prilikom prijavljivanja. Pokušaj ponovo kasnije."
            };
            
            ModelState.AddModelError("Input.Password", errorMessage);
            return Page();
        }
        
        if (User.IsInRole("Admin"))
            return RedirectToPage("/Admin/Dashboard/Index");
        
        return RedirectToPage("/Index");
    }
}