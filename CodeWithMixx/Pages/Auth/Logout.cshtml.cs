using CodeWithMixx.Domain.Entities.AppUsers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Auth;

public class LogoutModel(SignInManager<AppUser> signInManager) : PageModel
{
    public IActionResult OnGet()
    {
        return RedirectToPage("/Auth/Login/Index");
    }
    
    public async Task<IActionResult> OnPost()
    {
        await signInManager.SignOutAsync();
        return RedirectToPage("/Auth/Login/Index");
    }
}