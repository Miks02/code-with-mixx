using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Login;

public class IndexModel(IValidator<LoginViewModel> loginValidator) : PageModel
{
    [BindProperty]
    public LoginViewModel LoginVm { get; set; } = null!;
    
    public async Task<IActionResult> OnPost(CancellationToken ct = default)
    {
        var validationResult = await loginValidator.ValidateAsync(LoginVm, ct);

        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(x => ModelState.AddModelError($"LoginVm.{x.PropertyName}", x.ErrorMessage));
            return Page();
        }
        
        return Page();
    }
}