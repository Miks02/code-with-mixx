using System.Security.Claims;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Infrastructure.Web;
using CodeWithMixx.Pages.Admin.Classes.Create;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Projects.Create;

public class IndexModel(
    GetSubjectsHandler subjectsHandler, 
    CreateProjectValidator projectValidator,
    SearchStudentsHandler searchStudentsHandler,
    GetStudentHandler getStudentHandler,
    CreateProjectReservationHandler createReservationHandler) : PageModel
{
    [BindProperty]
    public CreateProjectInput Input { get; set; } = new();

    public CreateProjectViewModel ViewModel { get; set; } = new();
    
    public async Task<IActionResult> OnGet([FromQuery] string? studentId, CancellationToken ct = default)
    {
        ViewModel = await subjectsHandler.HandleAsync(ct);

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
            Response.ShowToast("Student nije pronađen", "error");
            return Partial("_Create", this);
        }
        return Partial("_StudentCard", result.Payload);
    }

    public async Task<IActionResult> OnPost([FromQuery] string? studentId, CancellationToken ct = default)
    {
        if (!string.IsNullOrWhiteSpace(studentId))
            Input.StudentId = studentId;

        var validationResult = await projectValidator.ValidateAsync(Input, ct);
        
        if(!validationResult.IsValid)
        {
            validationResult.Errors.ForEach(x => ModelState.AddModelError($"Input.{x.PropertyName}", x.ErrorMessage));
            
            ViewModel = await subjectsHandler.HandleAsync(ct);
            if (!string.IsNullOrWhiteSpace(studentId))
            {
                var studentResult = await getStudentHandler.HandleAsync(studentId, ct);
                ViewModel = ViewModel with { SelectedStudent = studentResult.Payload };
            }
            return Partial("_Create", this);
        }

        var result = await createReservationHandler.HandleAsync(Input, User.FindFirstValue(ClaimTypes.NameIdentifier)!, ct);

        if (!result.IsSuccess)
        {
            Response.ShowToast(GetErrorMessage(result.Errors[0]), "error");
            return Partial("_Create", this);
        }
        
        Response.ShowToast("Projekat je uspešno kreiran");
        
        return RedirectToPage("/Admin/Projects/Index");
    }

    private string GetErrorMessage(Error error)
    {
        return error.Code switch
        {
            "Admin.NotFound" => "Administrator nije pronađen.",
            "Student.NotFound" => "Student nije pronađen.",
            "Subject.NotFound" => "Predmet nije pronađen.",
            _ => "Došlo je do nepoznate greške."
        };
    }
}