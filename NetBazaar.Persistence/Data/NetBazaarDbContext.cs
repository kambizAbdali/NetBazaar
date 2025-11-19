
using Microsoft.EntityFrameworkCore;
using NetBazaar.Domain.Attributes;
using NetBazaar.Domain.Entities;
using NetBazaar.Persistence.Interfaces.DatabaseContext;
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
        // دیگر موجودیت‌ها اینجا اضافه شوند
        // public DbSet<Product> Products { get; set; } = null!;
        // public DbSet<Order> Orders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            // اعمال Auditable برای موجودیت‌های دارای ویژگی AuditableAttribute
            ApplyAuditableConfiguration(modelBuilder);

            // اعمال کانفیگ‌ها از Assembly
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private static void ApplyAuditableConfiguration(ModelBuilder modelBuilder)
        {
            var auditableEntities = modelBuilder.Model.GetEntityTypes()
                .Where(entityType => entityType.ClrType
                    .GetCustomAttributes(typeof(AuditableAttribute), true)
                    .Any());

            foreach (var entityType in auditableEntities)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime>("UpdateTime");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime?>("InsertTime");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<DateTime?>("RemoveTime");

                modelBuilder.Entity(entityType.ClrType)
                    .Property<bool>("IsRemoved")
                    .HasDefaultValue(false);
            }
        }

        public override int SaveChanges()
        {
            UpdateAuditableEntities();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntities();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditableEntities()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(entry => entry.State is EntityState.Added
                             or EntityState.Modified
                             or EntityState.Deleted);

            foreach (var entityEntry in changedEntities)
            {
                if (!IsAuditableEntity(entityEntry.Entity.GetType()))
                    continue;

                UpdateAuditableProperties(entityEntry);
            }
        }

        private static bool IsAuditableEntity(Type entityType)
        {
            return entityType.GetCustomAttributes(typeof(AuditableAttribute), true)
                .Any();
        }

        private static void UpdateAuditableProperties(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entityEntry)
        {
            var currentTime = DateTime.UtcNow; // استفاده از UTC برای سازگاری زمانی

            switch (entityEntry.State)
            {
                case EntityState.Added:
                    SetPropertyValue(entityEntry, "InsertTime", currentTime);
                    SetPropertyValue(entityEntry, "UpdateTime", currentTime);
                    SetPropertyValue(entityEntry, "IsRemoved", false);
                    break;

                case EntityState.Modified:
                    SetPropertyValue(entityEntry, "UpdateTime", currentTime);
                    break;

                case EntityState.Deleted:
                    // Soft Delete Implementation
                    if (HasProperty(entityEntry, "IsRemoved") &&
                        HasProperty(entityEntry, "RemoveTime"))
                    {
                        entityEntry.State = EntityState.Modified; // تغییر به Modified برای Soft Delete
                        SetPropertyValue(entityEntry, "RemoveTime", currentTime);
                        SetPropertyValue(entityEntry, "IsRemoved", true);
                        SetPropertyValue(entityEntry, "UpdateTime", currentTime);
                    }
                    break;
            }
        }

        private static void SetPropertyValue(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entityEntry,
            string propertyName, object value)
        {
            if (HasProperty(entityEntry, propertyName))
            {
                entityEntry.Property(propertyName).CurrentValue = value;
            }
        }

        private static bool HasProperty(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entityEntry,
            string propertyName)
        {
            return entityEntry.Properties.Any(p => p.Metadata.Name == propertyName);
        }
    }
}
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseSqlServer("Server=.;Database=NetBazaar;Trusted_Connection=true;TrustServerCertificate=true;");
        //    }
        //}

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