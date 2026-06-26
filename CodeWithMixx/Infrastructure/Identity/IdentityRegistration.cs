using CodeWithMixx.Domain.Entities.AppUsers;
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
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        services.AddAuthorizationBuilder()
            .AddPolicy("AdminPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Admin");
            })
            .AddPolicy("StudentPolicy", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Student");
            });
    }
}