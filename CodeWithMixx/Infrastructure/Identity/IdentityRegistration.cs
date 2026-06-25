using CodeWithMixx.Domain.AppUsers;
using CodeWithMixx.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace CodeWithMixx.Infrastructure.Identity;

public static class IdentityRegistration
{
    public static void AddIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
    }
}