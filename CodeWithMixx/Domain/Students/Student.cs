using CodeWithMixx.Domain.AppUsers;

namespace CodeWithMixx.Domain.Students;

public class Student
{
    public AppUser AppUser { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
}