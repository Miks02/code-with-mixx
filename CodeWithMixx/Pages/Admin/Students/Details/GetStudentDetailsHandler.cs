using System.Globalization;
using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Students.Details;

public class GetStudentDetailsHandler(AppDbContext context) : IHandler
{
    public async Task<Result<StudentDetailsViewModel>> HandleAsync(string id, CancellationToken ct)
    {
        var student = await context.Students
            .Where(s => s.AppUserId == id)
            .Select(s => new StudentDetailsViewModel
            {
                Id = s.AppUserId,
                Initials = $"{s.AppUser.FirstName[0]}{s.AppUser.LastName[0]}",
                FullName = $"{s.AppUser.FirstName} {s.AppUser.LastName}",
                Email = s.AppUser.Email!,
                PhoneNumber = s.AppUser.PhoneNumber ?? "N/A",
                University = s.University ?? "N/A",
                RegisteredAt = s.AppUser.CreatedAt.ToString("MMMM dd, yyyy", new CultureInfo("sr-Latn-RS")),
                UpcomingClasses = 0,
                TotalClasses = 0,
                ActiveProjects = 0,
                Status = s.AppUser.AccountStatus
            })
            .FirstOrDefaultAsync(ct);

        if (student is null)
            return Result<StudentDetailsViewModel>.Failure(UserError.NotFound(id));

        return Result<StudentDetailsViewModel>.Success(student);
    }
}