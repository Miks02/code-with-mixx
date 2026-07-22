using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Projects;

public class IndexModel(GetProjectsPageHandler pageHandler, GetProjectsHandler projectsHandler) : PageModel
{
    private const int PageSize = 14;
    public ProjectsPageViewModel ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGet(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        [FromQuery] int? subjectId,
        [FromQuery] int page = 1,
        CancellationToken ct = default)
    {
        ViewModel = await pageHandler.HandleAsync(filter, sort, subjectId, page, PageSize, ct);

        if (Request.IsHtmx())
            return Partial("_ProjectsPage", ViewModel);

        return Page();
    }

    public async Task<IActionResult> OnGetProjects(
        [FromQuery] string? filter,
        [FromQuery] string? sort,
        [FromQuery] int? subjectId,
        [FromQuery] int page = 1,
        CancellationToken ct = default)
    {
        var projectsPage = await projectsHandler.HandleAsync(filter, sort, subjectId, page, PageSize, ct);

        if (Request.IsHtmx())
            return Partial("_ProjectsList", projectsPage);

        return RedirectToPage("/Admin/Projects/Index", new { filter, sort, subjectId, page });
    }
}