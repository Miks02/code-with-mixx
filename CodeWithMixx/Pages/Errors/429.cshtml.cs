using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RateLimiting;

namespace CodeWithMixx.Pages.Errors;

[DisableRateLimiting]
public class Error429 : PageModel
{
    public IActionResult OnGet()
    {
        if (TempData["Error"] is null)
            return RedirectToPage("/Index");
        
        return Page();
    }
}