using NetBazaar.Infrastructure.Data;
using NetBazaar.Infrastructure.DependencyInjection;
using NetBazaar.Persistence.Data.Seed;
using NetBazaar.Web.EndPoint.Hubs;
using NetBazaar.Web.EndPoint.Utilities.Filters;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Core ASP.NET Core Services
builder.Services.AddAuthorization();
builder.Services.AddSignalR();

// Use AddInfrastructureServices instead of separate methods
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddDatabaseServices(builder.Configuration);

// Application Services
builder.Services.AddApplicationServices();
builder.Services.AddScoped<SaveVisitorFilter>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("SignalRPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5194") // آدرس پروژه Admin.Endpoint
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // برای SignalR ضروری است
    });
});

var app = builder.Build();

// اجرای Seed
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NetBazaarDbContext>();
    SeedData.SeedCatalogTypes(context);
}

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

app.UseCors("SignalRPolicy");

app.MapHub<OnlineVisitorsHub>("/Hubs/OnlineVisitorsHub");
app.Run();