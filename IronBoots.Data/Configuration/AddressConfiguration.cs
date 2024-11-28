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
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasOne(a => a.Town)
                .WithMany(a => a.Addresses)
                .HasForeignKey(a => a.TownId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(a => a.Client)
                .WithOne(a => a.Address)
                .HasForeignKey<Address>(a => a.ClientId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
