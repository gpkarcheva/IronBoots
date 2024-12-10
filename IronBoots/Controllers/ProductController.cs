using IronBoots.Data;
using IronBoots.Data.Models;
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
            List<ProductIndexViewModel> model = await context.Products
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
            Product? product = await context.Products
                .Include(p => p.ProductMaterials)
                .ThenInclude(pm => pm.Material)
                .Include(p => p.ProductOrders)
                .ThenInclude(po => po.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null || product.IsDeleted == true)
            {
                return NotFound();
            }

            ProductViewModel model = new()
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Weight = product.Weight,
                Size = product.Size,
                ProductionCost = product.ProductionCost,
                ProductionTime = product.ProductionTime,
                ProductMaterials = product.ProductMaterials,
                ProductOrders = product.ProductOrders,
                IsDeleted = product.IsDeleted
            };
            return View(model);
        }

        //Add
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ProductViewModel model = new()
            {
                Materials = await context.Materials.Where(m => m.IsDeleted == false).ToListAsync()
            };
            return View(model);
        }
        //Add
        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Materials = await context.Materials.Where(m => m.IsDeleted == false).ToListAsync();
                return View(model);
            }
            Product product = new()
            {
                Name = model.Name,
                ImageUrl = model.ImageUrl,
                Weight = model.Weight,
                Size = model.Size,
                ProductionCost = model.ProductionCost,
                ProductionTime = model.ProductionTime,
            };

            List<ProductMaterial> productMaterials = model.SelectedMaterialsIds.Select(materialId => new ProductMaterial
            {
                ProductId = product.Id,
                MaterialId = materialId
            }).ToList();

            product.ProductMaterials = productMaterials;

            await context.Products.AddAsync(product);
            await context.ProductsMaterials.AddRangeAsync(productMaterials);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Delete
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            Product? toDelete = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (toDelete == null)
            {
                return NotFound();
            }
            toDelete.IsDeleted = true;
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Edit
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Product? current = await context.Products
                .Include(p => p.ProductMaterials)
                .ThenInclude(pm => pm.Material)
                .Include(p => p.ProductOrders)
                .ThenInclude(po => po.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (current == null)
            {
                return NotFound();
            }

            ProductViewModel model = new ProductViewModel()
            {
                Id = id,
                Name = current.Name,
                ImageUrl = current.ImageUrl,
                Weight = current.Weight,
                Size = current.Size,
                ProductionCost = current.ProductionCost,
                ProductionTime = current.ProductionTime,
                ProductMaterials = current.ProductMaterials,
                ProductOrders = current.ProductOrders,
                IsDeleted = current.IsDeleted,
                Materials = await context.Materials.AsNoTracking().ToListAsync()
            };
            model.SelectedMaterialsIds = model.ProductMaterials.Select(mp => mp.MaterialId).ToList();
            return View(model);
        }
        //Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Materials = await context.Materials.Where(m => m.IsDeleted == false).ToListAsync();
                return View(model);
            }
            Product? toEdit = await context.Products
                .Include(p => p.ProductMaterials)
                .ThenInclude(pm => pm.Material)
                .Include(p => p.ProductOrders)
                .ThenInclude(pm => pm.Order)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (toEdit == null)
            {
                return NotFound();
            }

            toEdit.Name = model.Name;
            toEdit.ImageUrl = model.ImageUrl;
            toEdit.Weight = model.Weight;
            toEdit.Size = model.Size;
            toEdit.ProductionCost = model.ProductionCost;
            toEdit.ProductionTime = model.ProductionTime;

            List<ProductMaterial> editProductMaterials = new List<ProductMaterial>();
            foreach (var materialId in model.SelectedMaterialsIds)
            {
                Material? currentMat = context.Materials.FirstOrDefault(m => m.Id == materialId);
                if (currentMat != null)
                {
                    ProductMaterial pm = new()
                    {
                        MaterialId = materialId,
                        ProductId = id,
                    };
                    model.ProductMaterials.Add(pm);
                    if (currentMat.MaterialProducts.FirstOrDefault(p => p.MaterialId == materialId) != null)
                    {
                        currentMat.MaterialProducts.Add(pm);
                    }
                }
            }
            toEdit.ProductMaterials = editProductMaterials;

            await context.SaveChangesAsync();
            return RedirectToAction("Details", "Product", new
            {
                id = toEdit.Id
            });
        }
    }
}
