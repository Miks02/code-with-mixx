using CodeWithMixx.Pages.Admin.Classes.Create;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Classes;

public class IndexModel(GetClassesPageHandler getPageHandler, GetClassesHandler getClassesHandler) : PageModel
{
    private readonly int _pageSize = 14;
    
    public ClassPageViewModel ViewModel = new();
    public async Task<IActionResult> OnGet([FromQuery] string? filter, [FromQuery] string? sort, [FromQuery] int? subjectId, [FromQuery] int page = 1, CancellationToken ct = default)
    {
        ViewModel = await getPageHandler.HandleAsync(filter, sort, subjectId, page, _pageSize, ct);

        if(Request.IsHtmx())
            return Partial("_ClassPage", ViewModel);

        return Page();
    }
    
    public async Task<IActionResult> OnGetClasses([FromQuery] string? filter, [FromQuery] string? sort, [FromQuery] int? subjectId, [FromQuery] int page = 1, CancellationToken ct = default)
    {
        var pagedClasses = await getClassesHandler.HandleAsync(filter, sort, subjectId, page, _pageSize, ct);

        if(Request.IsHtmx())
            return Partial("_ClassList", pagedClasses);

        return RedirectToPage("/admin/classes/index", new {filter, sort, subjectId, page});
    }
}