using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Persistence.EntityConfiguration
{
    public class CatalogItemFeatureConfig : IEntityTypeConfiguration<CatalogItemFeature>
    {
        public void Configure(EntityTypeBuilder<CatalogItemFeature> builder)
        {
            builder.ToTable("CatalogItemFeatures");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Group)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(f => f.Key)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(f => f.Value)
                   .IsRequired()
                   .HasMaxLength(300);
        }
    }
}
