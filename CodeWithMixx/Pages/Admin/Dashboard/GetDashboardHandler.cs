using System.Globalization;
using CodeWithMixx.Common.Interfaces;
using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Pages.Admin.Dashboard;

public class GetDashboardHandler(AppDbContext context) : IHandler
{
    public async Task<DashboardViewModel> HandleAsync(CancellationToken ct)
    {
        var totalIncome = await context.Reservations
            .Where(r => r.PaymentStatus == PaymentStatus.Paid || r.PaymentStatus == PaymentStatus.PartiallyPaid)
            .SumAsync(r => r.PaidAmount, ct);
        
        var completedClasses = await context.Classes
            .Where(c => c.ClassStatus == ClassStatus.Completed)
            .CountAsync(ct);

        var activeStudents = await context.Students
            .Where(s => s.AppUser.AccountStatus == AccountStatus.Active)
            .CountAsync(ct);
        
        var upcomingTerms = await context.Reservations
            .Where(r => r.Classes.Any(c => c.StartsAt > DateTime.UtcNow))
            .OrderByDescending(r => r.Classes.Min(c => c.StartsAt))
            .Select(r => new UpcomingTermItem
            {
                AmountOfClasses = r.Classes.Count,
                SubjectName = r.Classes.Select(c => c.Subject.Name).FirstOrDefault() ?? "N/A",
                StudentName = $"{r.Student.AppUser.FirstName} {r.Student.AppUser.LastName}",
                StartsAt = r.Classes.Min(c => c.StartsAt).ToString("yy.M.dd HH:mm", CultureInfo.InvariantCulture),
                EndsAt = r.Classes.Max(c => c.EndsAt).ToString("yy.M.dd HH:mm", CultureInfo.InvariantCulture),
                TotalPrice = r.TotalPrice,
                PaymentStatus = r.PaymentStatus,
                Id = r.Id
            })
            .Take(6)
            .ToListAsync(ct);
        
        var classCountBySubject = await GetClassCountBySubject(ct);

        foreach (var item in classCountBySubject)
        {
            var subject = item.Key;
            var count = item.Value;
            
            Console.WriteLine($"Subject: {subject}, Count: {count}");
            
        }

        return new DashboardViewModel
        {
            TotalIncome = totalIncome,
            CompletedClasses = completedClasses,
            ActiveStudents = activeStudents,
            SubjectsChart = classCountBySubject,
            UpcomingTerms = upcomingTerms
        };

    }
    
    private async Task<Dictionary<string, int>> GetClassCountBySubject(CancellationToken ct)
    {
        return await context.Classes
            .GroupBy(c => c.Subject.Name)
            .Select(g => new {SubjectName = g.Key, Count = g.Count()})
            .ToDictionaryAsync(g => g.SubjectName, g => g.Count, ct);
    }
}