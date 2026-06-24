using CodeWithMixx.Domain.Admins;
using CodeWithMixx.Domain.Students;
using Microsoft.AspNetCore.Identity;

namespace CodeWithMixx.Domain.AppUsers;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    
    public Admin? Admin { get; set; }
    
    public Student? Student { get; set; }
}