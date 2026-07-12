using CodeWithMixx.Domain.Entities.Reservations;

namespace CodeWithMixx.Pages.Admin.Dashboard;

public record UpcomingTermItem
{
    public int Id { get; set; }
    public string SubjectName { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string StartsAt { get; set; } = string.Empty;
    public string EndsAt { get; set; } = string.Empty;
    public int AmountOfClasses { get; set; }
    public Decimal TotalPrice { get; set; }
    public PaymentStatus PaymentStatus { get; set; }
}