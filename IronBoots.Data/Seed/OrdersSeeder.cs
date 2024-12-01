using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data.Seed
{
    public class OrdersSeeder
    {
        public async Task SeedOrdersAsync(ApplicationDbContext context)
        {
            var currentOrder = new Order()
            {
                ClientId = Guid.Parse("0A511CD3-889D-484D-8428-060FF4CA3850"),
                Client = await context.Clients.FirstAsync(),
                PlannedAssignedDate = DateTime.UtcNow,
                TotalPrice = 123.45m,
                IsActive = true
            };
            if (await context.Orders.FirstOrDefaultAsync(o => o.ClientId == currentOrder.ClientId) == null)
            {
                await context.Orders.AddAsync(currentOrder);
                await context.SaveChangesAsync();
            }
        }
    }
}
