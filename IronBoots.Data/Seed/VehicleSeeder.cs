using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data.Seed
{
    public class VehicleSeeder
    {
        public async Task SeedVehiclesAsync(ApplicationDbContext context)
        {
            Vehicle currentVehicle = new()
            {
                Name = "vehicle",
                WeightCapacity = 123.5,
                SizeCapacity = 100.05,
                IsDeleted = false
            };
            if (await context.Vehicles.FirstOrDefaultAsync(v => v.Name == currentVehicle.Name) == null)
            {
                await context.Vehicles.AddAsync(currentVehicle);
                await context.SaveChangesAsync();
            }
        }
    }
}
