using NetBazaar.Infrastructure.DependencyInjection;
using NetBazaar.Web.EndPoint.Utilities.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Core ASP.NET Core Services
builder.Services.AddAuthorization();

// Use AddInfrastructureServices instead of separate methods
builder.Services.AddInfrastructureServices(builder.Configuration);

// Application Services
builder.Services.AddApplicationServices();
builder.Services.AddScoped<SaveVisitorFilter>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();