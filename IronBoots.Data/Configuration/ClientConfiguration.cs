﻿using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronBoots.Data.Configuration
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasMany(c => c.AddressTown)
                .WithOne(c => c.Client)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder
                .HasOne(c => c.User)
                .WithMany()          // No navigation property on ApplicationUser
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
