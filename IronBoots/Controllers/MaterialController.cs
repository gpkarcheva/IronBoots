using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Materials;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Controllers
{
    public class MaterialController : Controller
    {
        private readonly ApplicationDbContext context;

        public MaterialController(ApplicationDbContext _context)
        {
            context = _context;
        }

        //Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<MaterialIndexViewModel> model = await context.Materials
                .Where(m => m.IsDeleted == false)
                .Select(m => new MaterialIndexViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    PictureUrl = m.PictureUrl
                })
                .AsNoTracking()
                .ToListAsync();

            return View(model);
        }

        //Details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Material? material = await context.Materials
            .AsNoTracking()
            .Include(m => m.MaterialProducts)
            .ThenInclude(pm => pm.Product)
            .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            if (material == null)
            {
                return RedirectToAction(nameof(Index));
            }

            MaterialViewModel model = new()
            {
                Id = material.Id,
                Name = material.Name,
                Price = material.Price,
                PictureUrl = material.PictureUrl,
                DistributorContact = material.DistributorContact,
                MaterialProducts = material.MaterialProducts
            };

            return View(model);
        }

        //Add
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            MaterialViewModel model = new()
            {
                Products = await context.Products.Where(p => p.IsDeleted == false).AsNoTracking().ToListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MaterialViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Material material = new()
            {
                Name = model.Name,
                Price = model.Price,
                PictureUrl = model.PictureUrl,
                DistributorContact = model.DistributorContact
            };

            List<ProductMaterial> productMaterials = model.SelectedProductIds.Select(productId => new ProductMaterial
            {
                ProductId = productId,
                MaterialId = material.Id
            }).ToList();

            material.MaterialProducts = productMaterials;

            await context.Materials.AddAsync(material);
            await context.ProductsMaterials.AddRangeAsync(productMaterials);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Delete
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            Material? toDelete = await context.Materials.FirstOrDefaultAsync(p => p.Id == id);
            if (toDelete == null)
            {
                return RedirectToAction(nameof(Index));
            }
            toDelete.IsDeleted = true;
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Edit
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Material? currentMaterial = await context.Materials
                .Include(m => m.MaterialProducts)
                .ThenInclude(pm => pm.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (currentMaterial == null)
            {
                return View("NotFound"); //TODO IMPLEMENT NOT FOUND
            }
            MaterialViewModel model = new MaterialViewModel()
            {
                Id = id,
                Name = currentMaterial.Name,
                Price = currentMaterial.Price,
                PictureUrl = currentMaterial.PictureUrl,
                DistributorContact = currentMaterial.DistributorContact,
                MaterialProducts = currentMaterial.MaterialProducts.Where(mp => mp.Product.IsDeleted == false).ToList(),
                Products = await context.Products.AsNoTracking().ToListAsync()
            };

            model.SelectedProductIds = model.MaterialProducts.Select(mp => mp.ProductId).ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, MaterialViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Material? toEdit = await context.Materials
                .Include(m => m.MaterialProducts)
                .ThenInclude(pm => pm.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toEdit == null)
            {
                return RedirectToAction(nameof(Index));
            }

            toEdit.Name = model.Name;
            toEdit.Price = model.Price;
            toEdit.PictureUrl = model.PictureUrl;
            toEdit.DistributorContact = model.DistributorContact;

            foreach (var prodId in model.SelectedProductIds)
            {
                Product? currentProd = context.Products.FirstOrDefault(p => p.Id == prodId);
                if (currentProd != null)
                {
                    ProductMaterial pm = new()
                    {
                        MaterialId = id,
                        ProductId = prodId,
                    };
                    model.MaterialProducts.Add(pm);
                    if (currentProd.ProductMaterials.FirstOrDefault(p => p.ProductId == prodId) != null)
                    {
                        currentProd.ProductMaterials.Add(pm);
                    }
                }
            }

            toEdit.MaterialProducts = model.MaterialProducts;
            await context.SaveChangesAsync();
            return RedirectToAction("Details", "Material", new
            {
                id = toEdit.Id
            });
        }
    }
}
