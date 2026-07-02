using CodeWithMixx.Domain.Entities.AppUsers;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeWithMixx.Pages.Admin.Students.Details;

public class IndexModel(
    GetStudentDetailsHandler handler,
    UpdateAccountStatusHandler updateStatusHandler,
    DeleteAccountHandler deleteAccountHandler) : PageModel
{
    public StudentDetailsViewModel ViewModel = new();
    
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
}