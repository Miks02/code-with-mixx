using CodeWithMixx.Domain.Entities.Admins;
using CodeWithMixx.Domain.Entities.Students;
using Microsoft.AspNetCore.Identity;

namespace CodeWithMixx.Domain.Entities.AppUsers;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    
    public Admin? Admin { get; set; }
    
    public Student? Student { get; set; }

    public AccountStatus AccountStatus { get; set; } = AccountStatus.Active;
    
    public DateTime AccountStatusUpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLoginAt { get; set; }
}