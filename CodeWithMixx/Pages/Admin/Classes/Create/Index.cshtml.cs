using CodeWithMixx.Infrastructure.Web;
using FluentValidation;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Classes.Create;

public class IndexModel(
    GetCreatePageHandler getPageHandler,
    SearchStudentsHandler searchStudentsHandler,
    GetStudentHandler getStudentHandler,
    IValidator<ClassReservationInput> validator) : PageModel
{
    [BindProperty] 
    public ClassReservationInput Input { get; set; } = new();
    public CreatePageViewModel ViewModel { get; set; } = new();

    public async Task<IActionResult> OnGet([FromQuery] string? studentId, CancellationToken ct = default)
    {
        ViewModel = await getPageHandler.HandleAsync(ct);

        if (string.IsNullOrEmpty(studentId))
        {
            if (Request.IsHtmx())
                return Partial("_Create", this);

            return Page();
        }
        
        var studentResult = await getStudentHandler.HandleAsync(studentId, ct);
        
        Input.StudentId = studentId;
        ViewModel = ViewModel with { SelectedStudent = studentResult.Payload };
        if (Request.IsHtmx())
            return Partial("_Create", this);

        return Page();
    }

    public async Task<IActionResult> OnGetStudents(string search, CancellationToken ct = default)
    {
        var results = await searchStudentsHandler.HandleAsync(search, ct);
        return Partial("_StudentSearchResults", results);
    }

    public async Task<IActionResult> OnGetStudent(string id, CancellationToken ct = default)
    {
        var result = await getStudentHandler.HandleAsync(id, ct);
        if (!result.IsSuccess)
        {
            Response.ShowToast("Student nije pronadjen", "error");
            return Partial("_Create", this);
        }
        return Partial("_StudentCard", result.Payload);
    }

    public async Task<IActionResult> OnPost([FromQuery] string? studentId, CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(studentId))
            Input.StudentId = studentId;
        
        var validationResult = await validator.ValidateAsync(Input, ct);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                var modelStateKey = $"Input.{error.PropertyName}";
                ModelState.AddModelError(modelStateKey, error.ErrorMessage);
            }

            ViewModel = await getPageHandler.HandleAsync(ct);
            var studentResult = await getStudentHandler.HandleAsync(studentId!, ct);
            ViewModel = ViewModel with { SelectedStudent = studentResult.Payload };
            return Partial("_Create", this);
        }
        // Privremeno
        return new OkResult();
    }
}
