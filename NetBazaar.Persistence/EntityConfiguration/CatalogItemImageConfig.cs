using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetBazaar.Persistence.EntityConfiguration
{
    public class CatalogItemImageConfig : IEntityTypeConfiguration<CatalogItemImage>
    {
        public void Configure(EntityTypeBuilder<CatalogItemImage> builder)
        {
            builder.ToTable("CatalogItemImages");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Src)
                   .IsRequired()
                   .HasMaxLength(500);
        }
    }
}
