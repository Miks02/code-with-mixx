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
    public async Task<DashboardViewModel> HandleAsync(int? year, CancellationToken ct)
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

        var years = await context.Classes
            .Select(c => c.StartsAt.Year)
            .Distinct()
            .ToListAsync(ct);
        
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
        
        return new DashboardViewModel
        {
            TotalIncome = totalIncome,
            CompletedClasses = completedClasses,
            ActiveStudents = activeStudents,
            Years = years,
            SubjectsChart = await GetClassCountBySubject(ct),
            FinanceChartData = await GetStudentsAndIncomeByMonth(year, ct),
            UpcomingTerms = upcomingTerms
        };

    }

    public async Task<FinanceChart> GetStudentsAndIncomeByMonth(int? year, CancellationToken ct)
    {
        var selectedYear = year ?? DateTime.UtcNow.Year;
        
        var incomeData = await context.Reservations
            .Where(r => r.Classes.Min(c => c.StartsAt).Year == selectedYear && (r.PaymentStatus == PaymentStatus.Paid || r.PaymentStatus == PaymentStatus.PartiallyPaid))
            .GroupBy(r => r.Classes.Min(c => c.StartsAt).Month)
            .Select(g => new {Month = g.Key, Income = g.Sum(r => r.PaidAmount)})
            .ToDictionaryAsync(g => g.Month, g => g.Income, ct);
        
        
        var studentData = await context.Classes
            .Where(c => c.StartsAt.Year == selectedYear) 
            .GroupBy(c => c.StartsAt.Month)             
            .Select(g => new 
            {
                Month = g.Key,
                StudentCount = g.Select(c => c.Reservation.StudentId).Distinct().Count()
            })
            .ToDictionaryAsync(x => x.Month, x => x.StudentCount, ct);
        
        var finalIncomeList = new List<decimal>();
        var finalStudentList = new List<int>();

        for(int i = 1; i <= 12; i++)
        {
            finalIncomeList.Add(incomeData.TryGetValue(i, out var value) ? value : 0);
            finalStudentList.Add(studentData.TryGetValue(i, out var value1) ? value1 : 0);
        }
        
        
        return new FinanceChart
        {
            SelectedYear = selectedYear,
            IncomeByMonth = finalIncomeList,
            StudentsCountByMonth = finalStudentList
        };
    }
    
    private async Task<Dictionary<string, int>> GetClassCountBySubject(CancellationToken ct)
    {
        return await context.Classes
            .Where(c => c.StartsAt < DateTime.UtcNow && c.ClassStatus != ClassStatus.Cancelled)
            .GroupBy(c => c.Subject.Name)
            .Select(g => new {SubjectName = g.Key, Count = g.Count()})
            .ToDictionaryAsync(g => g.SubjectName, g => g.Count, ct);
    }
}