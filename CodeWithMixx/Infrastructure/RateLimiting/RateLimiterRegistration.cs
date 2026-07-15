namespace CodeWithMixx.Infrastructure.RateLimiting;

public static class RateLimiterRegistration
{
    public static void AddRateLimiters(this IServiceCollection services)
    {
        services.AddGlobalRateLimiter();
        services.AddContactFormRateLimiter();
    }
}