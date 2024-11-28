using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronBoots.Data.Configuration
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasOne(v => v.Shipment)
                .WithOne(v => v.Vehicle)
                .HasForeignKey<Vehicle>(v => v.ShipmentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
