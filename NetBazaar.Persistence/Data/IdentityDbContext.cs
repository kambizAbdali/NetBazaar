using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetBazaar.Domain.Entities;
using System.Reflection;

namespace NetBazaar.Persistence.Data
{
    public class IdentityDatabaseContext : IdentityDbContext<User>
    {
        public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
                throw new ArgumentNullException(nameof(modelBuilder));

            // ✅ ابتدا base را فراخوانی کنید - این بسیار مهم است
            base.OnModelCreating(modelBuilder);

            // ✅ حذف موجودیت مشکل‌ساز Passkey قبل از هر کاری
            RemoveProblematicEntities(modelBuilder);

            // تغییر نام جداول
            modelBuilder.Entity<User>().ToTable("Users", "identity");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "identity");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "identity");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "identity");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "identity");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "identity");
            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "identity");
        }

        private static void RemoveProblematicEntities(ModelBuilder modelBuilder)
        {
            try
            {
                // پیدا کردن و حذف تمام موجودیت‌های مشکل‌ساز
                var entityTypes = modelBuilder.Model.GetEntityTypes().ToList();

                foreach (var entityType in entityTypes)
                {
                    var clrType = entityType.ClrType;
                    if (clrType.Name.Contains("Passkey") ||
                        clrType.Name.Contains("IdentityPasskeyData") ||
                        clrType.FullName?.Contains("Passkey") == true)
                    {
                        modelBuilder.Ignore(clrType);
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
    }
}