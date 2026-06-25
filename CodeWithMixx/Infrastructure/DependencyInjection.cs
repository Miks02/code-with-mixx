using System.Reflection;
using CodeWithMixx.Common.Interfaces;
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
        services.AddHandlers();
    }

    private static void AddHandlers(this IServiceCollection services)
    {
        var handlers = typeof(Program).Assembly
            .GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsAssignableTo(typeof(IHandler)))
            .ToList();

        foreach (var handler in handlers)
            services.AddScoped(handler);
    }

    public static async Task MapSeeders(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        
        await seeder.SeedRolesAsync();
        await seeder.SeedAdminAsync();
    }
}