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
    public class AddressTownConfiguration : IEntityTypeConfiguration<AddressTown>
    {
        public void Configure(EntityTypeBuilder<AddressTown> builder)
        {
            builder.HasOne(a => a.Address)
                .WithMany(a => a.To)
        }
    }
}
