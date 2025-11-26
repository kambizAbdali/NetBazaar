using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetBazaar.Domain.Entities.Catalog;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace NetBazaar.Persistence.EntityConfiguration
{
    public class CatalogTypeConfig : IEntityTypeConfiguration<CatalogType>
    {
        public void Configure(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CatalogTypes");
            builder.HasKey(ct => ct.Id);
            builder.Property(ct => ct.Type).IsRequired().HasMaxLength(100);

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
