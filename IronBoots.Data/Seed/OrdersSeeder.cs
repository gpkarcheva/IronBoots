using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data.Seed
{
    public class OrdersSeeder
    {
        public async Task SeedOrdersAsync(ApplicationDbContext context)
        {
            var client = await context.Clients.FirstOrDefaultAsync();
            var orders = context.Orders;
            if (client != null)
            {
                if (orders.Any())
                {
                    return;
                }
                var currentOrder = new Order()
                {
                    ClientId = client.Id,
                    Client = client,
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
}
