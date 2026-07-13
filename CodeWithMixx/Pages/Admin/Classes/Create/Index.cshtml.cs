using System.Security.Claims;
using CodeWithMixx.Common.Results;
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
    CreateReservationHandler createReservationHandler,
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

        var result = await createReservationHandler.HandleAsync(Input, User.FindFirstValue(ClaimTypes.NameIdentifier)!, ct);

        if (!result.IsSuccess)
        {
            Response.ShowToast(GetErrorMessage(result.Errors[0]), "error");
            return Partial("_Create", this);
        }
        
        
        Response.ShowToast("Rezervacija je kreirana");
        return RedirectToPage("/Admin/Classes/Index");
    }
    
    private string GetErrorMessage(Error error)
    {
        return error.Code switch
        {
            "Admin.NotFound" => "Administrator nije pronadjen.",
            "Student.NotFound" => "Student nije pronadjen.",
            "Subject.NotFound" => "Predmet nije pronadjen.",
            _ => "Došlo je do nepoznate greške. Pogledaj logove za detalje"
        };
    }
}
