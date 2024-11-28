using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronBoots.Data.Configuration
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasOne(c => c.Address)
                .WithOne(c => c.Client)
                .HasForeignKey<Client>(c => c.AddressId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
