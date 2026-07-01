using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Infrastructure.Persistence;
using Htmx;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Students.Details;

public class StudentDetailsHandler(AppDbContext context) : IHandler
{
    public async Task<StudentDetailsViewModel> HandleAsync(string studentId, CancellationToken ct = default)
    {
        var student = await context.Students
            .Where(s => s.AppUserId == studentId)
            .Select(s => new StudentDetailsViewModel
            {
                Id = s.AppUserId,
                Initials = $"{s.AppUser.FirstName[0]}{s.AppUser.LastName[0]}",
                FullName = $"{s.AppUser.FirstName} {s.AppUser.LastName}",
                Email = s.AppUser.Email!,
                PhoneNumber = s.AppUser.PhoneNumber ?? "N/A",
                University = s.University ?? "N/A",
                RegisteredAt = s.AppUser.CreatedAt.ToString("MMMM dd, yyyy"),
                UpcomingClasses = 0,
                TotalClasses = 0,
                ActiveProjects = 0,
                Status = s.AppUser.AccountStatus
            })
            .FirstOrDefaultAsync(ct);

        if (student is null)
        {
            throw new Exception($"Student with ID {studentId} not found.");
        }

        return student;
    }
}
public class IndexModel(
    StudentDetailsHandler handler,
    UpdateAccountStatusHandler updateStatusHandler,
    DeleteAccountHandler deleteAccountHandler) : PageModel
{
    public StudentDetailsViewModel ViewModel = new();
    
    public async Task<IActionResult> OnGet([FromQuery] string id, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(id)) return BadRequest("Student ID is required.");
        var student = await handler.HandleAsync(id, ct);
        ViewModel = student;
        
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
            return Partial("_Details", student);
        }
        
        TempData["SuccessMessage"] = "Status studenta je uspešno ažuriran.";    
        return Partial("_Details", student);
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