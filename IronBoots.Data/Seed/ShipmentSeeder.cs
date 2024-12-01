using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronBoots.Data.Seed
{
    public class ShipmentSeeder
    {
        public async Task SeedShipmentsAsync(ApplicationDbContext context)
        {
            var currentVehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.Name == "vehicle");
            var currentOrder = await context.Orders
                .FirstOrDefaultAsync(o => o.ClientId == Guid.Parse("9CD2BA8E-BE18-41DD-BB95-7C3477EDFDDC"));
            if (currentVehicle != null && currentOrder != null)
            {
                var currentShipment = new Shipment()
                {
                    VehicleId = Guid.Parse("EA7B32B8-F776-4E39-ACC7-2977899670BD"),
                    Vehicle = currentVehicle,
                    Orders = new List<Order>()
                    {
                        currentOrder
                    },
                    ShipmentStatus = Shipment.Status.PendingShipment
                };
                if (await context.Shipments.FirstOrDefaultAsync(s => s.VehicleId == currentVehicle.Id) == null)
                {
                    await context.Shipments.AddAsync(currentShipment);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
