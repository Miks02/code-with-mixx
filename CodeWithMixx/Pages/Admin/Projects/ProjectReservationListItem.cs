using CodeWithMixx.Domain.Entities.Projects;
using CodeWithMixx.Domain.Entities.Reservations;

namespace CodeWithMixx.Pages.Admin.Projects;

public record ProjectReservationListItem
{
    public int Id { get; init; }
    public string SubjectName { get; init; } = string.Empty;
    public string StudentName { get; init; } = string.Empty;
    public string StartDate { get; init; } = string.Empty;
    public string EndDate { get; init; } = string.Empty;
    public string ReservedAt { get; init; } = string.Empty;
    public Decimal TotalPrice { get; init; }
    public PaymentStatus PaymentStatus { get; init; }
    public ProjectStatus ProjectStatus { get; init; }
};