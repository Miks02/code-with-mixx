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
}