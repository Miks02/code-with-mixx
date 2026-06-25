using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Infrastructure.Persistence;

public static class PersistenceRegistration
{
    public static void AddPersistence(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
    }
}