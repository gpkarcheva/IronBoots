using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronBoots.Data.Configuration
{
    public class AddressConfiguration : IEntityTypeConfiguration<AddressTown>
    {
        public void Configure(EntityTypeBuilder<AddressTown> builder)
        {
            builder.HasKey(pk => new
            {
                pk.AddressId,
                pk.TownId
            });

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
