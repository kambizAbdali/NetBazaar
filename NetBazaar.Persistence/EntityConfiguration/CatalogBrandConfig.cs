using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Persistence.EntityConfiguration
{
    public class CatalogBrandConfig : IEntityTypeConfiguration<CatalogBrand>
    {
        public void Configure(EntityTypeBuilder<CatalogBrand> builder)
        {
            builder.ToTable("CatalogBrands");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Brand)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(b => b.Catalogs)
                   .WithOne(i => i.CatalogBrand)
                   .HasForeignKey(i => i.CatalogBrandId);
        }
    }
}