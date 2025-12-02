using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace NetBazaar.Persistence.EntityConfiguration
{
    public class BasketItemConfig : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.ToTable("BasketItems");

            builder.HasKey(bi => bi.Id);

            //builder.HasQueryFilter(item => !item.IsRemoved); // فیلتر سراسری برای حذف‌شده‌ها

            builder.Property(bi => bi.Quantity)
                .IsRequired();

            builder.Property(bi => bi.UnitPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // ارتباط با CatalogItem
            builder.HasOne(bi => bi.CatalogItem)
                .WithMany()
                .HasForeignKey(bi => bi.CatalogItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // جلوگیری از آیتم‌های تکراری در یک سبد
            builder.HasIndex(bi => new { bi.BasketId, bi.CatalogItemId })
                .IsUnique();
        }
    }
}
