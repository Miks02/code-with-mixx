using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes.Create;

public class GetStudentHandler(AppDbContext context) : IHandler
{
    public async Task<Result<StudentSearchResultItem>> HandleAsync(string id, CancellationToken ct = default)
    {
        var student = await context.Students
            .Where(s => s.AppUserId == id)
            .Select(s => new StudentSearchResultItem
            {
                Id = s.AppUserId,
                Initials = s.AppUser.FirstName.Substring(0, 1) + s.AppUser.LastName.Substring(0, 1),
                FullName = s.AppUser.FirstName + " " + s.AppUser.LastName,
                Email = s.AppUser.Email!,
                University = s.University ?? "N/A"
            })
            .FirstOrDefaultAsync(ct);

        if (student is null)
            return Result<StudentSearchResultItem>.Failure(UserError.NotFound(id));
        

        return Result<StudentSearchResultItem>.Success(student);
    }
}