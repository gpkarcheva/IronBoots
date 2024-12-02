using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data.Seed
{
    public class ClientSeeder
    {
        public async Task SeedClientsAsync(ApplicationDbContext context)
        {
            var correctUser = await context.Users
                .Where(u => u.Id == Guid.Parse("9CD2BA8E-BE18-41DD-BB95-7C3477EDFDDC"))
                .FirstOrDefaultAsync();
            
            if (correctUser != null)
            {
                Client client = new Client()
                {
                    Name = "client",
                    UserId = correctUser.Id,
                    User = correctUser,
                    IsDeleted = false
                };

                if (await context.Clients.FirstOrDefaultAsync(c => c.UserId == client.UserId) == null)
                {
                    await context.Clients.AddAsync(client);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
