using System.Reflection;
using System.Threading.RateLimiting;
using CodeWithMixx.Infrastructure.BackgroundJobs;
using CodeWithMixx.Pages;
using Discord;
using Discord.WebSocket;
using FluentValidation;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages()
    .AddMvcOptions(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));


builder.Services
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
    .AddHealthChecks();

builder.Services.AddScoped<ContactHandler>();

var discordConfig = new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
};
builder.Services.AddSingleton(new DiscordSocketClient(discordConfig));
builder.Services.AddHostedService<DiscordBotService>();

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

builder.Services.AddSingleton(contactLimiter);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

var forwardedOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardedOptions.KnownIPNetworks.Clear();
forwardedOptions.KnownProxies.Clear();

app.UseForwardedHeaders(forwardedOptions);


app.UseRouting();

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();