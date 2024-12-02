using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Material;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
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
                    Id = model.Id,
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
        }

        //Delete
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                Material? toDelete = await context.Materials.FirstOrDefaultAsync(p => p.Id == id);
                if (toDelete == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                toDelete.IsDeleted = true;
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        //Edit
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Material? currentMaterial = await context.Materials.FirstOrDefaultAsync(m => m.Id == id);
            if (currentMaterial == null)
            {
                return View("NotFound"); //TODO IMPLEMENT NOT FOUND
            }
            currentMaterial.MaterialProducts = await context.ProductsMaterials.Where(pm => pm.MaterialId == currentMaterial.Id).ToListAsync();
            MaterialViewModel model = new MaterialViewModel()
            {
                Id = id,
                Name = currentMaterial.Name,
                Price = currentMaterial.Price,
                PictureUrl = currentMaterial.PictureUrl,
                DistributorContact = currentMaterial.DistrubutorContact,
                MaterialProducts = currentMaterial.MaterialProducts,
                SelectedProductIds = new List<Guid>(),
                Products = new List<Product>()
            };
            foreach (var materialProduct in model.MaterialProducts)
            {
                model.SelectedProductIds.Add(materialProduct.ProductId);
            }
            model.Products = await context.Products.ToListAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, MaterialViewModel model)
        {
            if (ModelState.IsValid)
            {
                Material? toEdit = await context.Materials.FirstOrDefaultAsync(m => m.Id == id);
                if (toEdit == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                toEdit.Name = model.Name;
                toEdit.Price = model.Price;
                toEdit.PictureUrl = model.PictureUrl;
                toEdit.DistrubutorContact = model.DistributorContact;
                List<ProductMaterial> editProductMaterial = new List<ProductMaterial>();
                List<ProductMaterial> productMaterials = await context.ProductsMaterials.Where(pm => pm.MaterialId == id).ToListAsync();

                foreach (var prodId in model.SelectedProductIds)
                {
                    Product? currentProd = context.Products.FirstOrDefault(p => p.Id == prodId);
                    if (currentProd != null)
                    {
                        ProductMaterial pm = new()
                        {
                            MaterialId = id,
                            Material = toEdit,
                            ProductId = prodId,
                            Product = currentProd
                        };
                        model.MaterialProducts.Add(pm);
                        if (currentProd.ProductMaterials.FirstOrDefault(p => p.ProductId == prodId) != null)
                        {
                            currentProd.ProductMaterials.Add(pm);
                        }
                    }
                }
                toEdit.MaterialProducts = model.MaterialProducts;
                if (!productMaterials.Any())
                {
                    await context.AddRangeAsync(editProductMaterial);
                }
                await context.SaveChangesAsync();
                return RedirectToAction("Details", "Material", new
                {
                    id = toEdit.Id
                });
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
