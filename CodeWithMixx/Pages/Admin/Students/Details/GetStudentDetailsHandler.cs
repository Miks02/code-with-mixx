using System.Globalization;
using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Domain.Entities.Classes;
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
                UpcomingClasses = s.Reservations.SelectMany(r => r.Classes).Count(c => c.ClassStatus == ClassStatus.Scheduled && c.StartsAt > DateTime.UtcNow),
                TotalClasses = s.Reservations.SelectMany(r => r.Classes).Count(),
                ActiveProjects = 0,
                Status = s.AppUser.AccountStatus,
                PreviousClasses = s.Reservations
                    .Where(r => r.Classes.Any(c => c.StartsAt < DateTime.UtcNow))
                    .Select(r => new StudentDetailsViewModel.ClassReservationItem
                    {
                        ReservationId = r.Id,
                        SubjectName = r.Classes.Select(c => c.Subject.Name).FirstOrDefault() ?? "N/A",
                        StartDate = r.Classes.Min(c => c.StartsAt),
                        EndDate = r.Classes.Max(c => c.EndsAt),
                        ClassStatus = r.Classes.Any(c => c.ClassStatus == ClassStatus.Ongoing) ? ClassStatus.Ongoing :
                            r.Classes.All(c => c.ClassStatus == ClassStatus.Completed) ? ClassStatus.Completed :
                            r.Classes.All(c => c.ClassStatus == ClassStatus.Cancelled) ? ClassStatus.Cancelled :
                            ClassStatus.Scheduled,
                        PaymentStatus = r.PaymentStatus
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);

        if (student is null)
            return Result<StudentDetailsViewModel>.Failure(UserError.NotFound(id));

        return Result<StudentDetailsViewModel>.Success(student);
    }
}