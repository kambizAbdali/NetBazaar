using Microsoft.EntityFrameworkCore;
using NetBazaar.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register EF Core DbContext with connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<NetBazaarDbContext>(options =>
    options.UseSqlServer(connectionString));

// If you also want to enable a DbContext factory (optional, useful for Blazor/W3C scenarios):
// builder.Services.AddDbContextFactory<MyDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
  
app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();