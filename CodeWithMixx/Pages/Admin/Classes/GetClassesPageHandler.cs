using System.Globalization;
using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Common.Results;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Classes;

public class GetClassesPageHandler(AppDbContext context) : IHandler
{
    public async Task<ClassPageViewModel> HandleAsync(string? filter, string? sort, int? subjectId, int page, int pageSize, CancellationToken ct)
    {
        var query = context.Reservations.AsQueryable();

        query = filter switch
        {
            "ongoing" => query.Where(r => r.Classes.Any(c => c.ClassStatus == ClassStatus.Ongoing)),
            "completed" => query.Where(r => r.Classes.Any(c => c.ClassStatus == ClassStatus.Completed)),
            "cancelled" => query.Where(r => r.Classes.Any(c => c.ClassStatus == ClassStatus.Cancelled)),
            "upcoming" => query.Where(r => r.Classes.Any(c => c.ClassStatus == ClassStatus.Scheduled)),
            _ => query
        };
        
        query = sort switch
        {
            "date_desc" => query.OrderByDescending(r => r.Classes.Min(c => c.StartsAt)),
            "date_asc" => query.OrderBy(r => r.Classes.Min(c => c.StartsAt)),
            _ => query.OrderByDescending(r => r.Classes.Min(c => c.StartsAt))
        };
        
        if(subjectId is not null)
            query = query.Where(r => r.Classes.Any(c => c.SubjectId == subjectId));

        var listQuery = query
            .Select(r => new ClassReservationListItem
            {
                Id = r.Id,
                SubjectName = r.Classes.FirstOrDefault()!.Subject.Name,
                StudentName = r.Student.AppUser.FirstName + " " + r.Student.AppUser.LastName,
                StartsAt = r.Classes.Min(c => c.StartsAt).ToString("yy.M.dd HH:mm", CultureInfo.InvariantCulture),
                EndsAt = r.Classes.Max(c => c.EndsAt).ToString("yy.M.dd HH:mm", CultureInfo.InvariantCulture),
                AmountOfClasses = r.Classes.Count,
                TotalPrice = r.TotalPrice,
                PaymentStatus = r.PaymentStatus,
                ClassStatus = r.Classes.Any(c => c.ClassStatus == ClassStatus.Ongoing) ? ClassStatus.Ongoing :
                    r.Classes.All(c => c.ClassStatus == ClassStatus.Completed) ? ClassStatus.Completed :
                    r.Classes.All(c => c.ClassStatus == ClassStatus.Cancelled) ? ClassStatus.Cancelled :
                    ClassStatus.Scheduled
            });
        

        var subjects = await context.Subjects
            .Select(s => new ClassPageViewModel.SubjectDropdownItem
            {
                Id = s.Id,
                SubjectName = s.Name
            })  
            .ToListAsync(ct);

        var stats = await context.Classes
            .GroupBy(_ => 1)
            .Select(g => new
            { 
                TotalClasses = g.Count(),
                UpcomingClasses = g.Count(c => c.ClassStatus == ClassStatus.Scheduled),
                FinishedClasses = g.Count(c => c.ClassStatus == ClassStatus.Completed),
                CancelledClasses = g.Count(c => c.ClassStatus == ClassStatus.Cancelled)
            })
            .FirstOrDefaultAsync(ct);

        var pagedResult = await PagedResult<ClassReservationListItem>.CreateAsync(listQuery, page, pageSize, ct);


        return new ClassPageViewModel
        {
            TotalClasses = stats?.TotalClasses ?? 0,
            UpcomingClasses = stats?.UpcomingClasses ?? 0,
            FinishedClasses = stats?.FinishedClasses ?? 0,
            CancelledClasses = stats?.CancelledClasses ?? 0,
            Subjects = subjects,
            Classes = pagedResult
        };
    }
}