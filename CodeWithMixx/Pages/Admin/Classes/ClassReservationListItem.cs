using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Reservations;

namespace CodeWithMixx.Pages.Admin.Classes;

public record ClassReservationListItem
{
    public int Id { get; init; }
    public string SubjectName { get; init; } = string.Empty;
    public string StudentName { get; init; } = string.Empty;
    public string StartsAt { get; init; } = string.Empty;
    public string EndsAt { get; init; } = string.Empty;
    public int AmountOfClasses { get; init; }
    public Decimal TotalPrice { get; init; }
    public PaymentStatus PaymentStatus { get; init; }
    public ClassStatus ClassStatus { get; init; }
};