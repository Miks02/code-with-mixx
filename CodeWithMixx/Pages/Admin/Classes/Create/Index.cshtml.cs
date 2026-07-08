using FluentValidation;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Classes.Create;

public class IndexModel(
    GetCreatePageHandler getPageHandler,
    SearchStudentsHandler searchStudentsHandler,
    IValidator<ClassReservationInput> validator) : PageModel
{
    [BindProperty] 
    public ClassReservationInput Input { get; set; } = new();
    public CreatePageViewModel ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGet(CancellationToken ct = default)
    {
        ViewModel = await getPageHandler.HandleAsync(ct);

        if (Request.IsHtmx())
            return Partial("_Create", this);

        return Page();
    }

    public async Task<IActionResult> OnGetStudents(string search, CancellationToken ct = default)
    {
        var results = await searchStudentsHandler.HandleAsync(search, ct);
        return Partial("_StudentSearchResults", results);
    }

    public async Task<IActionResult> OnPost(CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(Input, ct);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                var modelStateKey = $"Input.{error.PropertyName}";
                ModelState.AddModelError(modelStateKey, error.ErrorMessage);
            }

            ViewModel = await getPageHandler.HandleAsync(ct);
            return Partial("_Create", this);
        }

        return new OkResult();
    }
}
