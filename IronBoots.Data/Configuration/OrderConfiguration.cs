﻿using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronBoots.Data.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.Client)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.ClientId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.Address)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.AddressId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.Shipment)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.ShipmentId)
                .OnDelete(DeleteBehavior.NoAction);


        }
    }
}
