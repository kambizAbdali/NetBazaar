var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();



// تعریف Policy برای CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5194") // آدرس پروژه Admin.EndPoint
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();
app.UseStaticFiles(); // برای فعال‌سازی wwwroot
app.UseStaticFiles(); // برای دسترسی مستقیم به فایل‌ها در wwwroot/uploads


// فعال‌سازی CORS
app.UseCors("AllowFrontend");
app.MapControllers();

app.Run();
