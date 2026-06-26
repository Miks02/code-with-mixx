using CodeWithMixx.Infrastructure;
using CodeWithMixx.Pages;
using Microsoft.AspNetCore.HttpOverrides;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/Student", "StudentPolicy");

}).AddMvcOptions(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);



builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));


builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ContactHandler>();

var app = builder.Build();

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

app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();