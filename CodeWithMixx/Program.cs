using CodeWithMixx.Infrastructure;
using CodeWithMixx.Infrastructure.Filters;
using CodeWithMixx.Infrastructure.Persistence;
using CodeWithMixx.Pages;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/Student", "StudentPolicy");
}).AddMvcOptions(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    options.Filters.Add(new ToastFilter());
});

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ContactHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>(); 
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        Log.Logger.Error($"An error happened during a database migration process: {ex.Message}");
    }
}

await app.MapSeeders();

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
app.UseRateLimiter();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();