using System.Globalization;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Dashboard;

public class IndexModel(GetDashboardHandler getDashboardHandler) : PageModel
{

    public DashboardViewModel ViewModel { get; set; } = new();
    
    public async Task<IActionResult> OnGetAsync([FromQuery] int? selectedYear, CancellationToken ct = default)
    {
        ViewModel = await getDashboardHandler.HandleAsync(selectedYear, ct);
        
        if(Request.IsHtmx())
            return Partial("_Dashboard", ViewModel);
        
        return Page();
    }
    
    public async Task<IActionResult> OnGetFinanceChart([FromQuery] int selectedYear, CancellationToken ct = default)
    {
        var data = await getDashboardHandler.GetStudentsAndIncomeByMonth(selectedYear, ct);
        
        if(Request.IsHtmx())
            return Partial("_FinanceChart", data);
        
        return RedirectToPage("/Admin/Dashboard/Index", new { selectedYear});
    }
}