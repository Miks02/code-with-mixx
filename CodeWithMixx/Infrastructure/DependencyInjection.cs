using System.Reflection;
using CodeWithMixx.Infrastructure.Discord;
using CodeWithMixx.Infrastructure.Identity;
using CodeWithMixx.Infrastructure.Persistence;
using CodeWithMixx.Infrastructure.RateLimiting;
using FluentValidation;

namespace CodeWithMixx.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration.GetConnectionString("PostgresConnection")!);
        services.AddIdentity();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddDiscord();
        services.AddRateLimiters();
    }

    public static async Task MapSeeders(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        
        await seeder.SeedRolesAsync();
        await seeder.SeedAdminAsync();
    }
}