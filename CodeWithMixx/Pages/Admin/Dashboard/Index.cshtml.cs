using System.Globalization;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Dashboard;

public class IndexModel(GetDashboardHandler getDashboardHandler) : PageModel
{

    public DashboardViewModel ViewModel { get; set; } = new();
    
    public async Task<IActionResult> OnGetAsync(CancellationToken ct = default)
    {
        ViewModel = await getDashboardHandler.HandleAsync(ct);
        
        if(Request.IsHtmx())
            return Partial("_Dashboard", ViewModel);
        
        return Page();
    }
}