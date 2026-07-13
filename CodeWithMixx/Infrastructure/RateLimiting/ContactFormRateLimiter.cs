using System.Threading.RateLimiting;

namespace CodeWithMixx.Infrastructure.RateLimiting;

public static class ContactFormRateLimiter
{
    public static void AddContactFormRateLimiter(this IServiceCollection services)
    {
        var contactLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
            RateLimitPartition.GetSlidingWindowLimiter(
                partitionKey: ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                factory: _ => new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = 2,
                    Window = TimeSpan.FromMinutes(1),
                    SegmentsPerWindow = 5,
                    QueueLimit = 0
                }
            )
        );

        services.AddSingleton(contactLimiter);
    }
}