using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Entities.Catalog;

namespace NetBazaar.Persistence.EntityConfiguration
{
    public class CatalogItemConfig : IEntityTypeConfiguration<CatalogItem>
    {
        public void Configure(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("CatalogItems");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(ci => ci.Description)
                   .IsRequired();

            builder.Property(ci => ci.Price)
                   .IsRequired();

            builder.Property(ci => ci.StockQuantity)
                   .IsRequired();

            builder.Property(ci => ci.ReorderThreshold)
                   .IsRequired();

            builder.Property(ci => ci.MaxStockThreshold)
                   .IsRequired();

            builder.HasOne(ci => ci.CatalogType)
                   .WithMany(ct => ct.CatalogItems)
                   .HasForeignKey(ci => ci.CatalogTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ci => ci.CatalogBrand)
                   .WithMany(cb => cb.Catalogs)
                   .HasForeignKey(ci => ci.CatalogBrandId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(ci => ci.Features)
                   .WithOne(f => f.CatalogItem)
                   .HasForeignKey(f => f.CatalogItemId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(ci => ci.Images)
                   .WithOne(i => i.CatalogItem)
                   .HasForeignKey(i => i.CatalogItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}