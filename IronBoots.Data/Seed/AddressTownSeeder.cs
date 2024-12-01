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
            var aT = new List<AddressTown>();
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
                        IsDeleted = false
                    });
                }
            }
            foreach (var at in aT)
            {
                if (await context.AddressesTowns.FirstOrDefaultAsync(addT => addT.TownId == at.TownId) != null
                    && await context.AddressesTowns.FirstOrDefaultAsync(addT => addT.AddressId == at.AddressId) != null)
                {
                    continue;
                }
                await context.AddressesTowns.AddAsync(at);
            }
            await context.SaveChangesAsync();
        }
    }
}
