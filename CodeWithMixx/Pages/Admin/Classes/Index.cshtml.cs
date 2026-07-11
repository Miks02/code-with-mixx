using CodeWithMixx.Infrastructure.Web;
using CodeWithMixx.Pages.Admin.Classes.Details;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Classes;

public class IndexModel(
    GetClassesPageHandler getPageHandler, 
    GetClassesHandler getClassesHandler, 
    GetTermDetailsHandler getTermDetailsHandler) : PageModel
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

    public async Task<IActionResult> OnGetTermDetails(int reservationId, CancellationToken ct = default)
    {
        var result = await getTermDetailsHandler.HandleAsync(reservationId, ct);
        
        var filter = Request.Query["filter"].ToString();
        var sort = Request.Query["sort"].ToString();
        var subjectId = Request.Query["subjectId"].ToString();
        var page = Request.Query["page"].ToString();
        
        if (!result.IsSuccess)
        {
            Response.ShowToast("Detalji termina nisu pronadjeni", "error");
            return Partial("_ClassList", ViewModel);
        }
        
        if(Request.IsHtmx())
            return Partial("Admin/Classes/Details/_TermDetails", result.Payload);

        return RedirectToPage("/admin/classes/index", new {filter, sort, subjectId, page});

    }
}