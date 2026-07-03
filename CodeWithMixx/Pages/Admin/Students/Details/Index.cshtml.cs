using CodeWithMixx.Domain.Entities.AppUsers;
using FluentValidation;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Students.Details;

public class IndexModel(
    GetStudentDetailsHandler handler,
    UpdateAccountStatusHandler updateStatusHandler,
    UpdateStudentHandler updateStudentHandler,
    DeleteAccountHandler deleteAccountHandler,
    IValidator<StudentDetailsInputModel> validator) : PageModel
{
    public StudentDetailsViewModel ViewModel = new();
    [BindProperty]
    public StudentDetailsInputModel Input { get; set; } = new();
    
    public async Task<IActionResult> OnGet([FromQuery] string id, CancellationToken ct = default)
    {
        var result = await handler.HandleAsync(id, ct);
        ViewModel = result.Payload!;
        
        if(Request.IsHtmx())
            return Partial("_Details", ViewModel);
        return Page();
    }

    public async Task<IActionResult> OnPostUpdateStatus([FromQuery] string id, [FromForm] AccountStatus status, CancellationToken ct = default)
    {
        var result = await updateStatusHandler.HandleAsync(id, status, ct);
        var student = await handler.HandleAsync(id, ct);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = "Student nije pronadjen.";
            return Partial("_Details", student.Payload);
        }
        
        TempData["SuccessMessage"] = "Status studenta je uspešno ažuriran.";    
        return Partial("_Details", student.Payload);
    }

    public async Task<IActionResult> OnPostDeleteAccount([FromQuery] string id, CancellationToken ct = default)
    {
        var result = await deleteAccountHandler.HandleAsync(id, ct);

        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = "Student nije pronadjen.";
            return Partial("_Details", ViewModel);
        }

        TempData["SuccessMessage"] = "Student je uspešno obrisan.";
        
        return RedirectToPage("/Admin/Students/Index");
    }

    public async Task<IActionResult> OnGetEdit([FromQuery] string id, CancellationToken ct = default)
    {
        var result = await handler.HandleAsync(id, ct);
        
        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = "Student nije pronadjen.";
            return Partial("_Details", ViewModel);
        }
        
        var student = result.Payload;
        var firstName = student!.FullName.Split(" ")[0];
        var lastName = student.FullName.Split(" ")[1];
        
        Input = new StudentDetailsInputModel
        {
            Id = student.Id,
            FirstName = firstName,
            LastName = lastName,
            Email = student.Email,
            PhoneNumber = student.PhoneNumber,
            University = student.University,
            RegisteredAt = student.RegisteredAt,
            Initials = student.Initials,
        };

        return Partial("_Edit", Input);
    }
    
    public async Task<IActionResult> OnPostEdit(CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(Input, ct);
        if (!validationResult.IsValid)
        {
            foreach (var error in validationResult.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            Response.Headers.Append("HX-Retarget", "#edit-info");
            Response.Headers.Append("HX-Reswap", "outerHTML");
            return Partial("_Edit", Input);
        }

        var result = await updateStudentHandler.HandleAsync(Input, ct);
        
        if (!result.IsSuccess)
        {
            TempData["ErrorMessage"] = "Student nije pronadjen.";
            return Partial("_Edit", Input);
        }
        
        TempData["SuccessMessage"] = "Student je uspešno ažuriran.";
        
        return Partial("_Details", result.Payload);
    }
}