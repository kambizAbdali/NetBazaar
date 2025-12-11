using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Entities.Orders;
using NetBazaar.Domain.ValueObjects;

namespace NetBazaar.Persistence.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.Property(o => o.BuyerId)
                   .IsRequired()
                   .HasMaxLength(450); // چون معمولاً UserId از Identity می‌آید

            builder.Property(o => o.OrderDate)
                   .IsRequired();

            builder.Property(o => o.PaymentMethod)
                   .IsRequired();

            builder.Property(o => o.PaymentStatus)
                   .IsRequired();

            builder.Property(o => o.OrderStatus)
                   .IsRequired();

            // Address به عنوان Owned Entity
            // Address به عنوان Owned Entity
            builder.OwnsOne(o => o.Address, a =>
            {
                a.Property(p => p.ReceiverName)
                    .IsRequired()
                    .HasMaxLength(100);

                a.Property(p => p.City)
                    .IsRequired()
                    .HasMaxLength(100);

                a.Property(p => p.PostalCode)
                    .IsRequired()
                    .HasMaxLength(10);

                a.Property(p => p.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                a.Property(p => p.AddressText)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            // رابطه با OrderItem
            builder.HasMany(o => o.OrderItems)
                   .WithOne()
                   .HasForeignKey("OrderId")
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Orders");
        }
    }
}
