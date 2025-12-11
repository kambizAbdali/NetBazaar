using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NetBazaar.Domain.Entities;
using NetBazaar.Domain.Entities.Basket;
using NetBazaar.Domain.Entities.Catalog;
using NetBazaar.Domain.Entities.Orders;
using NetBazaar.Domain.Entities.Users;
using System.Threading;
using System.Threading.Tasks;

namespace NetBazaar.Persistence.Interfaces.DatabaseContext
{
    public interface INetBazaarDbContext
    {
        DatabaseFacade Database { get; }

        // Existing properties
        DbSet<CatalogType> CatalogTypes { get; }
        DbSet<CatalogBrand> CatalogBrands { get; }
        DbSet<CatalogItem> Catalogs { get; }

        // Add these to match implementation
        DbSet<CatalogItemFeature> CatalogItemFeatures { get; }
        DbSet<CatalogItemImage> CatalogItemImages { get; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        // Save methods remain the same
        public int SaveChanges();
        public int SaveChanges(bool acceptAllChangesOnSuccess);
        public Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
