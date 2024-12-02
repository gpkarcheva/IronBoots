using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Material;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using System.Runtime.CompilerServices;

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
            var model = await context.Materials
                .Where(m => m.IsDeleted == false)
                .Select(m => new MaterialIndexViewModel
                {
                    Id = m.Id,
                    Name = m.Name,
                    Price = m.Price,
                    DistributorContact = m.DistrubutorContact,
                    MaterialProducts = m.MaterialProducts,
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
            var productMaterials = context.ProductsMaterials.Where(pm => pm.MaterialId == id).ToList();
            foreach (var pm in productMaterials)
            {
                pm.Product = context.Products.FirstOrDefault(p => p.Id == pm.ProductId);
            }
            Material? current = await context.Materials.FirstOrDefaultAsync(m => m.Id == id);
            if (current == null)
            {
                return RedirectToAction(nameof(Index));
            }
            if (current.IsDeleted == true)
            {
                return View(nameof(Index));
            }
            MaterialIndexViewModel model = new MaterialIndexViewModel()
            {
                Id = current.Id,
                Name = current.Name,
                Price = current.Price,
                PictureUrl = current.PictureUrl,
                DistributorContact = current.DistrubutorContact,
                MaterialProducts = productMaterials
            };
            return View(model);
        }

        //Add
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var products = await context.Products.ToListAsync();
            var model = new MaterialViewModel();
            model.Products = products;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MaterialViewModel model)
        {
            if (ModelState.IsValid)
            {
                Material material = new Material()
                {
                    Name = model.Name,
                    Price = model.Price,
                    PictureUrl = model.PictureUrl,
                    DistrubutorContact = model.DistributorContact,
                    MaterialProducts = new List<ProductMaterial>()
                };
                List<Guid> ids = model.SelectedProductIds.ToList();
                List<ProductMaterial> productMaterials = new List<ProductMaterial>();
                foreach (var id in ids)
                {
                    Product? currentProduct = await context.Products.FirstOrDefaultAsync(p => p.Id == id);
                    if (currentProduct != null)
                    {
                        ProductMaterial pm = new ProductMaterial()
                        {
                            ProductId = id,
                            Product = currentProduct,
                            MaterialId = material.Id,
                            Material = material
                        };
                        productMaterials.Add(pm);
                        currentProduct.ProductMaterials.Add(pm);
                    }
                }
                material.MaterialProducts = productMaterials;
                await context.ProductsMaterials.AddRangeAsync(productMaterials);
                await context.Materials.AddAsync(material);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
            //Remove
            //Edit
        }
    }
}
