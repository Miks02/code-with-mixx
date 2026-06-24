using CodeWithMixx.Domain.AppUsers;

namespace CodeWithMixx.Domain.Admins;

public class Admin
{
    public AppUser AppUser { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
}