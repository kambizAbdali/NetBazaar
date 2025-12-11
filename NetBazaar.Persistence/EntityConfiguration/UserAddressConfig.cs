using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Entities.Users;

namespace NetBazaar.Persistence.EntityConfiguration
{
    public class UserAddressConfig : IEntityTypeConfiguration<UserAddress>
    {
        public void Configure(EntityTypeBuilder<UserAddress> builder)
        {
            builder.ToTable("UserAddresses");

            builder.HasKey(a => a.Id);

            // UserId الزامی و طول محدود
            builder.Property(a => a.UserId)
                .IsRequired()
                .HasMaxLength(256);

            // ReceiverName الزامی و طول محدود
            builder.Property(a => a.ReceiverName)
                .IsRequired()
                .HasMaxLength(100);

            // PhoneNumber الزامی و طول محدود
            builder.Property(a => a.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            // PostalCode الزامی و طول محدود
            builder.Property(a => a.PostalCode)
                .IsRequired()
                .HasMaxLength(10);

            // AddressText الزامی و طول محدود
            builder.Property(a => a.AddressText)
                .IsRequired()
                .HasMaxLength(500);

            // City الزامی و طول محدود
            builder.Property(a => a.City)
                .IsRequired()
                .HasMaxLength(100);

            // IsDefault مقدار پیش‌فرض false
            builder.Property(a => a.IsDefault)
                .HasDefaultValue(false)
                .IsRequired();

            // ایندکس برای جستجوی سریع بر اساس UserId
            builder.HasIndex(a => a.UserId);
        }
    }
}
