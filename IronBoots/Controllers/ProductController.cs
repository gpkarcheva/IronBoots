using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Materials;
using IronBoots.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext context;

        public ProductController(ApplicationDbContext _context)
        {
            context = _context;
        }

        //Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = await context.Products
                .Where(p => p.IsDeleted == false)
                .Select(p => new ProductIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    PictureUrl = p.ImageUrl
                })
                .AsNoTracking()
                .ToListAsync();

            return View(model);
        }

        //Details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Product? product = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null || product.IsDeleted == true) 
            {
                return View(nameof(Index));
            }

            List<ProductMaterial> productMaterials = await context.ProductsMaterials
                .Where(pm => pm.ProductId == id)
                .ToListAsync();

            foreach (var pm in productMaterials)
            {
                pm.Material = await context.Materials.FirstOrDefaultAsync(m => m.Id == pm.MaterialId);
            }

            List<OrderProduct> orderProducts = await context.OrdersProducts
                .Where(op => op.ProductId  == id)
                .ToListAsync();

            foreach (var op in orderProducts)
            {
                op.Order = await context.Orders.FirstOrDefaultAsync(o => o.Id == op.OrderId);
            }

            ProductViewModel model = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Weight = product.Weight,
                Size = product.Size,
                ProductionCost = product.ProductionCost,
                ProductionTime = product.ProductionTime,
                ProductMaterials = productMaterials,
                ProductOrders = orderProducts,
                IsDeleted = product.IsDeleted
            };
            return View(model);
        }

        //Add
        [HttpGet]
        public async Task<IActionResult> Add()
        {
			var materials = await context.Materials.ToListAsync();
			var model = new ProductViewModel();
			model.Materials = materials;
			return View(model);
		}
    }
}
