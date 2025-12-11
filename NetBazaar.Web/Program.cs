using FluentValidation.AspNetCore;
using NetBazaar.Application.Interfaces.Catalog;
using NetBazaar.Infrastructure.Data;
using NetBazaar.Infrastructure.DependencyInjection;
using NetBazaar.Infrastructure.Services;
using NetBazaar.Persistence.Data.Seed;
using NetBazaar.Web.EndPoint.Hubs;
using NetBazaar.Web.EndPoint.Utilities.Filters;
using NetBazaar.Web.EndPoint.Validators;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<CheckoutViewModelValidator>();
    });

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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.UseCors("SignalRPolicy");

app.MapHub<OnlineVisitorsHub>("/Hubs/OnlineVisitorsHub");
app.Run();