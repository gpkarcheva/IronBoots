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
    public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
    {
        public void Configure(EntityTypeBuilder<Shipment> builder)
        {
            builder.HasOne(s => s.Vehicle)
                .WithOne(s => s.Shipment)
                .HasForeignKey<Shipment>(s => s.VehicleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(s => s.Orders)
                .WithOne(s => s.Shipment)
                .HasForeignKey(s => s.ShipmentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
