using NetBazaar.Admin.EndPoint.Configuration;
using NetBazaar.Admin.EndPoint.Filters;
using NetBazaar.Infrastructure.DependencyInjection;
using NetBazaar.Infrastructure.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<PreserveModelFilter>(); //To fully utilize GlobalExceptionFilter, we may need another Action Filter to preserve the model"
});


// Use AddInfrastructureServices instead of separate methods
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddDatabaseServices(builder.Configuration);

// Application Services
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(typeof(ViewModelMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

// روتینگ MVC به جای Razor Pages
app.UseRouting();

app.UseAuthorization();

// اگر بخواهید فایل‌های استاتیک را هم نگه دارید (مثل CSS/JS) می‌توانید از این دو خط استفاده کنید
app.UseStaticFiles(); // برای استاتیک‌ها

// نقشهٔ روتینگ MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");


// اگر نیاز به روت‌های اضافی دارید:
// app.MapControllerRoute(
//     name: "areas",
//     pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.Run();