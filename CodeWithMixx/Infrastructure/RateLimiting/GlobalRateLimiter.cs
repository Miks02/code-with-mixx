using System.Security.Claims;
using System.Threading.RateLimiting;
using CodeWithMixx.Infrastructure.Web;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace CodeWithMixx.Infrastructure.RateLimiting;

public static class GlobalRateLimiter 
{
    public static void AddGlobalRateLimiter(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {
                var path = context.Request.Path.Value ?? "";

                var isStatic = path switch
                {
                    _ when path.StartsWith("/css", StringComparison.OrdinalIgnoreCase) => true,
                    _ when path.StartsWith("/js", StringComparison.OrdinalIgnoreCase) => true,
                    _ when path.StartsWith("/lib", StringComparison.OrdinalIgnoreCase) => true,
                    _ when path.StartsWith("/images", StringComparison.OrdinalIgnoreCase) => true,
                    _ when path.StartsWith("/_framework", StringComparison.OrdinalIgnoreCase) => true,
                    _ => false
                };

                if (isStatic)
                    return RateLimitPartition.GetNoLimiter("static");
                

                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var partitionKey = userId 
                                   ?? context.Connection.RemoteIpAddress?.ToString() 
                                   ?? "unknown";

                return RateLimitPartition.GetTokenBucketLimiter(partitionKey, _ => new TokenBucketRateLimiterOptions
                {
                    TokenLimit = 100,
                    ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                    TokensPerPeriod = 10,
                    QueueLimit = 0
                });
            });

            options.OnRejected = async (context, ct) =>
            {
                var acceptHeader = context.HttpContext.Request.Headers["Accept"].ToString();
                var httpContext = context.HttpContext;

                if (!acceptHeader.Contains("text/html"))
                {
                    httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    httpContext.Response.ShowToast("Polako, poslao/la si previše zahteva, sačekaj malo pa pokušaj ponovo.", "error");
                    return;
                }
                
                var tempDataFactory = httpContext.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
                var tempData = tempDataFactory.GetTempData(httpContext);
                tempData["TooManyRequests"] = true;
                tempData.Save();
                httpContext.Response.Redirect("/Errors/429");
                
                
                await Task.CompletedTask;
            };
        });
    }
}