using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Classes.Create;

public class IndexModel(GetCreatePageHandler getPageHandler, SearchStudentsHandler searchStudentsHandler) : PageModel
{
    public CreatePageViewModel ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGet(CancellationToken ct = default)
    {
        ViewModel = await getPageHandler.HandleAsync(ct);

        if (Request.IsHtmx())
            return Partial("_Create", ViewModel);

        return Page();
    }

    public async Task<IActionResult> OnGetStudents(string search, CancellationToken ct = default)
    {
        var results = await searchStudentsHandler.HandleAsync(search, ct);
        return Partial("_StudentSearchResults", results);
    }
}
