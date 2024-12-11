using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronBoots.Data.Configuration
{
    public class ClientProductConfiguration : IEntityTypeConfiguration<ClientProduct>
    {
        public void Configure(EntityTypeBuilder<ClientProduct> builder)
        {
            builder.HasKey(cp => new
            {
                cp.ClientId,
                cp.ProductId
            });

            builder.HasOne(cp => cp.Client)
                .WithMany(cp => cp.ClientsProducts)
                .HasForeignKey(cp => cp.ProductId);

            builder.HasOne(pc => pc.Product)
                .WithMany(pc => pc.ProductsClients)
                .HasForeignKey(pc => pc.ClientId);
        }
    }
}
