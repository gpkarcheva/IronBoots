using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data.Seed
{
    public class ProductSeeder
    {
        public async Task SeedProductsAsync(ApplicationDbContext context)
        {
            Product product = new Product()
            {
                Name = "product",
                Weight = 1.23,
                Size = 1.56,
                ProductionCost = 6.78m,
                ProductionTime = TimeSpan.Parse("0.02:54:18"),
                IsDeleted = false
            };

            if (await context.Products.FirstOrDefaultAsync(p => p.Name == product.Name) == null)
            {
                await context.Products.AddAsync(product);
            }
        }
    }
}
