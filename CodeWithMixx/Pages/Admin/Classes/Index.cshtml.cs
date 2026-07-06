using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Classes;

public class IndexModel(GetClassesPageHandler getPageHandler) : PageModel
{
    public ClassPageViewModel ViewModel = new();
    public async Task<IActionResult> OnGet(CancellationToken ct = default)
    {
        ViewModel = await getPageHandler.HandleAsync("", "", 1, 10, ct);

        if(Request.IsHtmx())
            return Partial("_ClassPage", ViewModel);

        return Page();
    }
}