using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data.Seed
{
    public class TownSeeder
    {
        public async Task SeedTownsAsync(ApplicationDbContext context)
        {
            List<Town> towns = new List<Town>()
            {
                new Town
                {
                    Name = "town1"
                },

                new Town
                {
                    Name = "town2"
                }
            };
            foreach (var town in towns)
            {
                if (await context.Towns.FirstOrDefaultAsync(t => t.Name == town.Name) != null)
                {
                    continue;
                }
                await context.Towns.AddAsync(town);
            }
            await context.SaveChangesAsync();
        }
    }
}
