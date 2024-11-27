using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronBoots.Data.Configuration
{
    public class ProductMaterialConfiguration : IEntityTypeConfiguration<ProductMaterial>
    {
        public void Configure(EntityTypeBuilder<ProductMaterial> builder)
        {
            builder.HasKey(pm => new
            {
                pm.ProductId,
                pm.MaterialId
            });

            builder.HasOne(mp => mp.Material)
                .WithMany(mp => mp.MaterialProducts)
                .HasForeignKey(mp => mp.MaterialId);

            builder.HasOne(pm => pm.Product)
                .WithMany(pm => pm.ProductMaterials)
                .HasForeignKey(pm => pm.ProductId);
        }
    }
}
