using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Domain.Entities.Reservations;

namespace CodeWithMixx.Domain.Entities.Students;

public class Student
{
    public AppUser AppUser { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
    
    public string? University { get; set; }
    
    public ICollection<Reservation> Reservations { get; set; } = [];
    
}