using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Persistence.EntityConfiguration
{
    public class BasketConfig : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.ToTable("Baskets");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.BuyerId)
                .IsRequired()
                .HasMaxLength(256);

            // ارتباط یک به چند با BasketItem
            builder.HasMany(b => b.Items)
                .WithOne(i => i.Basket)
                .HasForeignKey(i => i.BasketId)
                .OnDelete(DeleteBehavior.Cascade);

            // ایندکس برای جستجوی سریع
            builder.HasIndex(b => b.BuyerId)
                .IsUnique();
        }
    }
}
