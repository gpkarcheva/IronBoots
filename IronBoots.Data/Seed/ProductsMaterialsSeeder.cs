using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data.Seed
{
    public class ProductsMaterialsSeeder
    {
        public async Task SeedProductsMaterialsAsync(ApplicationDbContext context)
        {
            var currentProducts = await context.Products.ToListAsync();
            var currentMaterials = await context.Materials.ToListAsync();
            var productsMaterials = new List<ProductMaterial>();
            foreach (var item in currentProducts)
            {
                foreach (var materialItem in currentMaterials)
                {
                    Random random = new Random();
                    int quantity = random.Next(0, 10);
                    productsMaterials.Add(new ProductMaterial()
                    {
                        ProductId = item.Id,
                        Product = item,
                        MaterialId = materialItem.Id,
                        Material = materialItem,
                        MaterialQuantity = quantity
                    });
                }
            }
            foreach (var pm in productsMaterials)
            {
                if (await context.ProductsMaterials
                    .FirstOrDefaultAsync(prodM => prodM.ProductId == pm.ProductId) == null
                    && await context.ProductsMaterials
                    .FirstOrDefaultAsync(prodM => prodM.MaterialId == pm.MaterialId) == null)
                {
                    await context.ProductsMaterials.AddAsync(pm);
                }
            }
            await context.SaveChangesAsync();
        }
    }
}
