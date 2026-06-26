using CodeWithMixx.Domain.Entities.AppUsers;

namespace CodeWithMixx.Domain.Entities.Admins;

public class Admin
{
    public AppUser AppUser { get; set; } = null!;
    public string AppUserId { get; set; } = null!;
}