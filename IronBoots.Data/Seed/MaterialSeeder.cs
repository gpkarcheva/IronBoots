using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronBoots.Data.Seed
{
    public class MaterialSeeder
    {
        public async Task SeedMaterialsAsync(ApplicationDbContext context) 
        {
            Material material = new Material()
            {
                Name = "material1",
                Price = 123.5m,
                DistrubutorContact = "distr.com/contact",
                IsDeleted = false
            };

            if (await context.Materials.FirstOrDefaultAsync(mt => mt.Name == material.Name) == null)
            {
                await context.Materials.AddAsync(material);
                await context.SaveChangesAsync();
            }
        }
    }
}
