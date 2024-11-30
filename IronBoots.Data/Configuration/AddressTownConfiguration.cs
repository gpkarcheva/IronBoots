using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronBoots.Data.Configuration
{
    public class AddressTownConfiguration : IEntityTypeConfiguration<AddressTown>
    {
        public void Configure(EntityTypeBuilder<AddressTown> builder)
        {
            builder.HasIndex(at => new
            {
                at.AddressId,
                at.TownId
            })
                .IsUnique();

            builder.HasOne(at => at.Address)
                .WithMany(at => at.AddressesTowns)
                .HasForeignKey(at => at.AddressId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(ta => ta.Town)
                .WithMany(ta => ta.TownsAddresses)
                .HasForeignKey(ta => ta.TownId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
