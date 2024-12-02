using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data.Seed
{
    public class AddressTownSeeder
    {
        public async Task SeedAddressTownAsync(ApplicationDbContext context)
        {
            var currentA = await context.Addresses.ToListAsync();
            var currentT = await context.Towns.ToListAsync();
            var currentClient = await context.Clients.FirstOrDefaultAsync();
            var aT = new List<AddressTown>();
            if (currentClient != null)
            {
                foreach (var address in currentA)
                {
                    foreach (var town in currentT)
                    {
                        aT.Add(new AddressTown
                        {
                            TownId = town.Id,
                            Town = town,
                            AddressId = address.Id,
                            Address = address,
                            IsDeleted = false,
                            ClientId = currentClient.Id,
                            Client = currentClient
                        });
                    }
                }

                foreach (var at in aT)
                {
                    var town = await context.Towns.FirstOrDefaultAsync(t => t.Id == at.TownId);
                    var address = await context.Addresses.FirstOrDefaultAsync(a => a.Id == at.AddressId);
                    if (town != null && address != null)
                    {
                        if (context.AddressesTowns.Count() == 0)
                        {
                            town.TownsAddresses.Add(at);
                            address.AddressesTowns.Add(at);
                            await context.AddressesTowns.AddAsync(at);
                            await context.SaveChangesAsync();
                            continue;
                        }
                        else if (await context.AddressesTowns.FirstOrDefaultAsync(addT => addT.TownId == at.TownId) != null
                            && await context.AddressesTowns.FirstOrDefaultAsync(addT => addT.AddressId == at.AddressId) != null)
                        {
                            continue;
                        }
                        town.TownsAddresses.Add(at);
                        address.AddressesTowns.Add(at);
                        currentClient.AddressTown.Add(at);
                        await context.AddressesTowns.AddAsync(at);
                    }
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
