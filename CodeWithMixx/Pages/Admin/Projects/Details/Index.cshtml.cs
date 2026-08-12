using System.Security.Claims;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Infrastructure.Web;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Projects.Details;

public class IndexModel(
    GetSubjectsForDetailsHandler subjectsHandler,
    ProjectDetailsValidator projectValidator,
    GetProjectDetailsHandler getProjectHandler,
    UpdateProjectDetailsHandler updateHandler,
    DeleteProjectHandler deleteHandler) : PageModel
{
    [BindProperty]
    public ProjectDetailsInput Input { get; set; } = null!;

    public ProjectDetailsViewModel ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGet(int id, CancellationToken ct)
    {
        var result = await getProjectHandler.HandleAsync(id, ct);
        if (!result.IsSuccess)
        {
            Response.ShowToast("Projekat nije pronađen", "error");
            return RedirectToPage("/Admin/Projects/Index");
        }

        Input = result.Payload.Input;
        var subjectsViewModel = await subjectsHandler.HandleAsync(ct);
        ViewModel = subjectsViewModel with { SelectedStudent = result.Payload.Student };

        if (Request.IsHtmx())
            return Partial("_Details", this);

        return Page();
    }

    public async Task<IActionResult> OnPost(CancellationToken ct)
    {
        var validationResult = await projectValidator.ValidateAsync(Input, ct);

        if (!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(x => ModelState.AddModelError($"Input.{x.PropertyName}", x.ErrorMessage));

            var subjectsViewModel = await subjectsHandler.HandleAsync(ct);
            var projectResult = await getProjectHandler.HandleAsync(Input.ProjectId, ct);
            ViewModel = subjectsViewModel with { SelectedStudent = projectResult.Payload.Student };

            return Partial("_Details", this);
        }

        var result = await updateHandler.HandleAsync(Input, User.FindFirstValue(ClaimTypes.NameIdentifier)!, ct);

        if (!result.IsSuccess)
        {
            Response.ShowToast(GetErrorMessage(result.Errors[0]), "error");
            return Partial("_Details", this);
        }

        Response.ShowToast("Projekat je uspešno ažuriran");

        return RedirectToPage("/Admin/Projects/Index");
    }

    public async Task<IActionResult> OnPostDelete(int id, CancellationToken ct)
    {
        var result = await deleteHandler.HandleAsync(id, ct);

        if (!result.IsSuccess)
        {
            Response.ShowToast("Greška prilikom brisanja projekta", "error");
            return Partial("_Details", this);
        }

        Response.ShowToast("Projekat je uspešno obrisan");
        return RedirectToPage("/Admin/Projects/Index");
    }

    private string GetErrorMessage(Error error)
    {
        return error.Code switch
        {
            "Admin.NotFound" => "Administrator nije pronađen.",
            "Project.NotFound" => "Projekat nije pronađen.",
            "Subject.NotFound" => "Predmet nije pronađen.",
            _ => "Došlo je do nepoznate greške."
        };
    }
}