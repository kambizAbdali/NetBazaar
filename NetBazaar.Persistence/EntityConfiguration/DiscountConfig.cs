using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Discounts;

namespace NetBazaar.Persistence.EntityConfiguration
{
    public class DiscountConfig : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.ToTable("Discounts");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(d => d.UsePercentage).IsRequired();
            builder.Property(d => d.DiscountPercentage);
            builder.Property(d => d.DiscountAmount);

            builder.Property(d => d.StartDate).IsRequired();
            builder.Property(d => d.EndDate).IsRequired();

            builder.Property(d => d.RequiresCouponCode).IsRequired();
            builder.Property(d => d.CouponCode).HasMaxLength(100);

            builder.Property(d => d.DiscountType).IsRequired();
            builder.Property(d => d.LimitationType).IsRequired();
            builder.Property(d => d.LimitationTimes);

            // اگر رابطه با CatalogItem وجود دارد (many-to-many) آن را کانفیگ کن
            // در این نسخه ساده فرض می‌کنیم رابطه‌ی many-to-many با جدول میانی خودکار EF Core برقرار می‌شود
            // اگر نیاز به جدول میانی سفارشی داری، آن را تعریف کن.
        }
    }
}
