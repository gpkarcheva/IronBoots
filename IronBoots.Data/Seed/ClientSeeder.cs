using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data.Seed
{
    public class ClientSeeder
    {
        public async Task SeedClientsAsync(ApplicationDbContext context)
        {
            var addressTownId = context.AddressesTowns.First().Id;
            var addressTown = await context.AddressesTowns
                .Where(a => a.Id == addressTownId)
                .FirstOrDefaultAsync();

            var correctUser = await context.Users
                .Where(u => u.Id == Guid.Parse("9CD2BA8E-BE18-41DD-BB95-7C3477EDFDDC"))
                .FirstOrDefaultAsync();
            if (addressTown != null && correctUser != null)
            {
                Client client = new Client()
                {
                    Name = "client",
                    AddressTownId = addressTownId,
                    AddressTown = addressTown,
                    UserId = Guid.Parse("9CD2BA8E-BE18-41DD-BB95-7C3477EDFDDC"),
                    User = correctUser,
                    IsDeleted = false
                };
                if (await context.Clients.FirstOrDefaultAsync(c => c.UserId == client.UserId) == null!)
                {
                    await context.Clients.AddAsync(client);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
