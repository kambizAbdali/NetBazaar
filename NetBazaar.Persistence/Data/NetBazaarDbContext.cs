
using Microsoft.EntityFrameworkCore;
using NetBazaar.Application.Interfaces.DatabaseContext;
using NetBazaar.Domain.Entities;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace NetBazaar.Infrastructure.Data
{
    public class NetBazaarDbContext : DbContext, INetBazaarDbContext
    {
        public NetBazaarDbContext(DbContextOptions<NetBazaarDbContext> options)
            : base(options)
        {
        }

        // DbSets   
        public DbSet<User> Users { get; set; }
        //public override int SaveChanges(bool acceptAllChangesOnSuccess)
        //{

        //}
        //public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        //{

        //}
        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{

        //}
        //public override int SaveChanges()
        //{

        //}
        //public DbSet<Category> Categories => Set<Category>();
        //public DbSet<Order> Orders => Set<Order>();
        //public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        //public DbSet<Customer> Customers => Set<Customer>();
        //public DbSet<Inventory> Inventories => Set<Inventory>();

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    // اعمال کانفیگ‌ها از Assembly
        //    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        //    // یا به صورت دستی
        //    // modelBuilder.ApplyConfiguration(new ProductConfiguration());
        //    // modelBuilder.ApplyConfiguration(new OrderConfiguration());
        //}

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=.;Database=NetBazaar;Trusted_Connection=true;TrustServerCertificate=true;");
        //    }
        //}
    }
}