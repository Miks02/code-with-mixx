using Microsoft.EntityFrameworkCore;

namespace CodeWithMixx.Infrastructure.Persistence;

public static class PersistenceRegistration
{
    public static void AddPersistence(this IServiceCollection services, string connectionString)
    {
        
        var railwayDbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
        if (!string.IsNullOrEmpty(railwayDbUrl))
        {
            var databaseUri = new Uri(railwayDbUrl);
            var userInfo = databaseUri.UserInfo.Split(':');

            connectionString = $"Host={databaseUri.Host};" +
                               $"Port={databaseUri.Port};" +
                               $"Username={userInfo[0]};" +
                               $"Password={userInfo[1]};" +
                               $"Database={databaseUri.AbsolutePath.TrimStart('/')};" +
                               $"SSL Mode=Require;Trust Server Certificate=true;";
        }
        
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<DatabaseSeeder>();
    }
}