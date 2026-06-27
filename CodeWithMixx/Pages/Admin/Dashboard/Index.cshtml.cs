using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Dashboard;

public class Index : PageModel
{
    public IActionResult OnGet()
    {
        if(Request.IsHtmx())
            return Partial("_Dashboard");
        
        return Page();
    }
}