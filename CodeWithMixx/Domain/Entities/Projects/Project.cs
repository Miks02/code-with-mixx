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
    public ProjectStatus ProjectStatus { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    

}