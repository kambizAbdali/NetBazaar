using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Entities.Orders;

namespace NetBazaar.Persistence.EntityConfiguration
{
    public class OrderItemConfig : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(o => o.Id);

            // CatalogItemId الزامی
            builder.Property(o => o.CatalogItemId)
                .IsRequired();

            // ProductName الزامی و محدودیت طول
            builder.Property(o => o.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            // PictureUri اختیاری ولی محدودیت طول
            builder.Property(o => o.PictureUri)
                .HasMaxLength(500);

            // UnitPrice الزامی و نوع دقیق
            builder.Property(o => o.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Units الزامی
            builder.Property(o => o.Units)
                .IsRequired();

            // ایندکس برای جستجو سریع روی CatalogItemId
            builder.HasIndex(o => o.CatalogItemId);
        }
    }
}
