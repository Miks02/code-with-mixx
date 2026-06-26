using CodeWithMixx.Domain.Entities.AppUsers;

namespace CodeWithMixx.Domain.Entities.Students;

public class Student
{
    public AppUser AppUser { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
}