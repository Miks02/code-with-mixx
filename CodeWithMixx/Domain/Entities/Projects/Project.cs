using System.Diagnostics;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Domain.Entities.Students;
using CodeWithMixx.Domain.Entities.Subjects;

namespace CodeWithMixx.Domain.Entities.Projects;

public class Project
{
    public int Id { get; set; }
    public Reservation Reservation { get; set; } = null!;
    public int ReservationId { get; set; }
    public Subject Subject { get; set; } = null!;
    public int SubjectId { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public DateOnly ReservedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


}