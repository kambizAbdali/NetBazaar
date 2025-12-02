using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetBazaar.Application.Common.Configuration;
using NetBazaar.Application.Interfaces.Basket;
using NetBazaar.Application.Interfaces.Catalog;
using NetBazaar.Application.Interfaces.Visitor;
using NetBazaar.Infrastructure.Configuration;
using NetBazaar.Infrastructure.Data;
using NetBazaar.Infrastructure.MappingProfiles;
using NetBazaar.Infrastructure.Services.Basket;
using NetBazaar.Infrastructure.Services.Catalog;
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
            services
                .AddIdentityService(configuration)
                .AddAutoMapper(typeof(CatalogMappingProfile))
                .ConfigureApplicationCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(12);
                    options.LoginPath = "/account/login";
                    options.AccessDeniedPath = "/account/access-denied";
                    options.SlidingExpiration = true;
                });

            return services;
        }

        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services)
        {
            // Visitor Services
            services.AddTransient<ISaveVisitorInfoService, SaveVisitorInfoService>();
            services.AddTransient<IVisitorStatisticsService, VisitorStatisticsService>();
            services.AddScoped<IOnlineVisitorsService, OnlineVisitorsService>();

            // Catalog Services
            services.AddScoped<ICatalogTypeService, CatalogTypeService>();
            services.AddScoped<ICatalogBrandService, CatalogBrandService>();
            services.AddScoped<ICatalogItemService, CatalogItemService>();
            services.AddScoped<IGetMenuItemService, GetMenuItemService>();
            services.AddScoped<IGetCatalogItemDetailService, GetCatalogItemDetailService>();

            services.AddScoped<IImageUrlService, ImageUrlService>(); // ثبت با اینترفیس
            services.AddScoped<IGetCatalogItemPLPService, GetCatalogItemPLPService>();

            services.AddScoped<IBasketService, BasketService>();


            // Validation
            //services.AddFluentValidationAutoValidation();
            //services.AddValidatorsFromAssemblyContaining<AddCatalogItemDtoValidator>();

            return services;
        }

        public static IServiceCollection AddDatabaseServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServer");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("SQL Server connection string 'SqlServer' is not configured.");
            }

            // SQL Server Database Services
            services.AddDbContext<NetBazaarDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<INetBazaarDbContext>(provider =>
                provider.GetRequiredService<NetBazaarDbContext>());

            // MongoDB Services
            services.Configure<MongoDBSettings>(configuration.GetSection("MongoDB"));
            services.AddTransient(typeof(IMongoDbContext<>), typeof(MongoDbContext<>));

            return services;
        }

        public static IServiceCollection AddCustomIdentityServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services.AddInfrastructureServices(configuration);
        }
    }
}