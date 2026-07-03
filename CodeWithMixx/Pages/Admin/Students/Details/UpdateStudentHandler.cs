using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Infrastructure.Persistence;
using CodeWithMixx.Pages.Admin.Students.Create;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Students.Details;

public class UpdateStudentHandler(AppDbContext context) : IHandler
{
    public async Task<Result<StudentDetailsViewModel>> HandleAsync(StudentDetailsInputModel request, CancellationToken ct = default)
    {
        var student = await context.Students
            .Include(s => s.AppUser)
            .FirstOrDefaultAsync(s => s.AppUserId == request.Id, ct);
        
        if(student is null)
            return Result<StudentDetailsViewModel>.Failure(UserError.NotFound(request.Id));
        
        student.AppUser.FirstName = request.FirstName;
        student.AppUser.LastName = request.LastName;
        student.AppUser.Email = request.Email;
        student.AppUser.PhoneNumber = request.PhoneNumber;
        student.University = request.University;
        
        await context.SaveChangesAsync(ct);
        
        var vm = new StudentDetailsViewModel
        {
            Id = student.AppUserId,
            Initials = $"{student.AppUser.FirstName[0]}{student.AppUser.LastName[0]}",
            FullName = $"{student.AppUser.FirstName} {student.AppUser.LastName}",
            Email = student.AppUser.Email!,
            PhoneNumber = student.AppUser.PhoneNumber ?? "N/A",
            University = student.University ?? "N/A",
            RegisteredAt = student.AppUser.CreatedAt.ToString("MMMM dd, yyyy", new System.Globalization.CultureInfo("sr-Latn-RS")),
            UpcomingClasses = 0,
            TotalClasses = 0,
            ActiveProjects = 0,
            Status = student.AppUser.AccountStatus
        };
        
        return Result<StudentDetailsViewModel>.Success(vm);
    }
}