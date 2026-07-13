using CodeWithMixx.Domain.Entities.AppUsers;
using CodeWithMixx.Domain.Entities.Reservations;
using CodeWithMixx.Domain.Entities.Subjects;

namespace CodeWithMixx.Domain.Entities.Admins;

public class Admin
{
    public AppUser AppUser { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
    
    public ICollection<Subject> Subjects { get; set; } = [];
    public ICollection<Reservation> Reservations { get; set; } = [];
}