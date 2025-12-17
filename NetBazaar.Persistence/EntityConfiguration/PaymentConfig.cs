using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Entities.Payments;

namespace NetBazaar.Persistence.EntityConfiguration
{
    public class PaymentConfig : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("Payments");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.CreatedAt).IsRequired();
            builder.Property(p => p.Status).IsRequired();

            builder.Property(p => p.Authority).HasMaxLength(100);
            builder.Property(p => p.RefId).HasMaxLength(100);

            builder.HasIndex(p => p.OrderId);
        }
    }
}
