using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Domain.Entities.Classes;
using CodeWithMixx.Domain.Entities.Reservations;

namespace CodeWithMixx.Pages.Admin.Students.Details;

public record StudentDetailsViewModel
{
    public string Id { get; init; } = string.Empty;
    public string Initials { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string University { get; init; } = string.Empty;
    public string RegisteredAt { get; init; } = string.Empty;
    public int UpcomingClasses { get; init; }
    public int TotalClasses { get; init; }
    public int ActiveProjects { get; init; }
    public AccountStatus Status { get; init; }
    public bool HasActiveProjects => ActiveProjects > 0;
    public List<ClassReservationItem> PreviousClasses { get; init; } = [];

    public record ClassReservationItem
    {
        public int ReservationId { get; init; }
        public string SubjectName { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public ClassStatus ClassStatus { get; init; }
        public PaymentStatus PaymentStatus { get; init; }
    }
    
};