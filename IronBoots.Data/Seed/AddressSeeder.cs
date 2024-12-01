using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data.Seed
{
    public class AddressSeeder
    {
        public async Task SeedAddressAsync(ApplicationDbContext context)
        {
            List<Address> addresses = new List<Address>()
        {
            new Address
            {
                AddressText = "addresline 123"
            },
            new Address
            {
                AddressText = "addressline 456"
            }
        };
            foreach (var address in addresses)
            {
                if (await context.Addresses.FirstOrDefaultAsync(a => a.AddressText == address.AddressText) != null)
                {
                    continue;
                }
                await context.Addresses.AddAsync(address);
            }
            await context.SaveChangesAsync();
        }
    }
}
