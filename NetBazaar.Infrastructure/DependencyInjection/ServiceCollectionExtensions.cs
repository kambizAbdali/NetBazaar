using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBazaar.Application.Common.Configuration; // From Application layer
using NetBazaar.Application.Interfaces.Visitor;
using NetBazaar.Infrastructure.Configuration;
using NetBazaar.Infrastructure.Data;
using NetBazaar.Infrastructure.Services.Visitor;
using NetBazaar.Infrastructure.Services.Visitors;
using NetBazaar.Persistence.Data;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
using System;

namespace NetBazaar.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // SQL Server Database Services
            var connectionString = configuration.GetConnectionString("SqlServer");
            services.AddDbContext<NetBazaarDbContext>(options =>
                options.UseSqlServer(connectionString));


            // MongoDB Configuration - THIS WAS MISSING!
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDB"));
            // MongoDB Services
            services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));

            // Identity Services
            services.AddIdentityService(configuration);


            // Application Cookie Configuration
            services.ConfigureApplicationCookie(option =>
            {
                option.ExpireTimeSpan = TimeSpan.FromMinutes(12);
                option.LoginPath = "/account/login";
                option.AccessDeniedPath = "/account/AccessDenied";
                option.SlidingExpiration = true;
            });


            return services;
        }

        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            services.AddTransient<ISaveVisitorInfoService, SaveVisitorInfoService>();
            services.AddTransient<IVisitorStatisticsService, VisitorStatisticsService>();
            return services;
        }

        // If you're using separate methods, make sure they call the main one
        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddInfrastructureServices(configuration);
        }

        public static IServiceCollection AddCustomIdentityServices(
            this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddInfrastructureServices(configuration);
        }
    }
}