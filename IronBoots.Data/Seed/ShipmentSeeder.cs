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
                .FirstAsync();
            if (currentVehicle != null && currentOrder != null)
            {
                var currentShipment = new Shipment()
                {
                    VehicleId = currentVehicle.Id,
                    Vehicle = currentVehicle,
                    Orders = new List<Order>()
                    {
                        currentOrder
                    },
                    ShipmentStatus = Shipment.Status.PendingShipment
                };
                if (await context.Shipments.FirstOrDefaultAsync(s => s.VehicleId == currentVehicle.Id) == null)
                {
                    currentOrder.ShipmentId = currentShipment.Id;
                    currentOrder.Shipment = currentShipment;
                    await context.Shipments.AddAsync(currentShipment);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
