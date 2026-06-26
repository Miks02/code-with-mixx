using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Login;

public class IndexModel(IValidator<LoginInputModel> loginValidator, LoginHandler handler) : PageModel
{
    [BindProperty]
    public LoginInputModel Input { get; set; } = null!;
    
    public async Task<IActionResult> OnPost(CancellationToken ct = default)
    {
        var validationResult = await loginValidator.ValidateAsync(Input, ct);

        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(x => ModelState.AddModelError($"LoginVm.{x.PropertyName}", x.ErrorMessage));
            return Page();
        }
        
        var result = await handler.HandleAsync(Input);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError("Input.Password", "Email adresa ili lozinka nisu validni");
            return Page();
        }

        if (User.IsInRole("Admin"))
            return RedirectToPage("/Admin/Dashboard/Index");
        
        
        return Page();
    }
}