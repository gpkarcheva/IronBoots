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
            foreach (var product in currentProducts)
            {
                foreach (var material in currentMaterials)
                {
                    Random random = new Random();
                    int quantity = random.Next(0, 10);
                    productsMaterials.Add(new ProductMaterial()
                    {
                        ProductId = product.Id,
                        Product = product,
                        MaterialId = material.Id,
                        Material = material,
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
                    var material = await context.Materials.FirstOrDefaultAsync(m => m.Id == pm.MaterialId);
                    var product = await context.Products.FirstOrDefaultAsync(p => p.Id == pm.ProductId);
                    if (material != null && product != null)
                    {
                        material.MaterialProducts.Add(pm);
                        product.ProductMaterials.Add(pm);
                        await context.ProductsMaterials.AddAsync(pm);
                    }
                }
            }
            await context.SaveChangesAsync();

            foreach (var product in productsMaterials)
            {
                context.Products.FirstOrDefault(p => p.Id == product.ProductId).ProductMaterials.Add(product);
                context.Materials.FirstOrDefault(m => m.Id == product.MaterialId).MaterialProducts.Add(product);
            }
        }
    }
}
