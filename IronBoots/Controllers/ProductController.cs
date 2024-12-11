using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static IronBoots.Common.ExtensionMethods;
using static IronBoots.Common.EntityValidationConstants.DateTimeValidation;
using System.Globalization;

namespace IronBoots.Controllers
{
    [Authorize]
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
                    PictureUrl = p.ImageUrl,
                    Price = p.Price.ToString("F2")
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
                Price = product.Price.ToString("F2"),
                ImageUrl = product.ImageUrl,
                Weight = product.Weight,
                Size = product.Size,
                ProductionCost = product.ProductionCost.ToString("F2"),
                ProductionTime = product.ProductionTime.ToString(),
                ProductMaterials = product.ProductMaterials,
                ProductOrders = product.ProductOrders,
                IsDeleted = product.IsDeleted
            };
            return View(model);
        }

        [Authorize(Roles = "Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
        //Add
        [HttpPost]
        public async Task<IActionResult> Add(ProductViewModel model)
        {
            if (!IsPriceValid(model.ProductionCost))
            {
                ModelState.AddModelError(nameof(model.ProductionCost), "Please enter a valid price.");
            }
            if (!IsPriceValid(model.Price))
            {
                ModelState.AddModelError(nameof(model.Price), "Please enter a valid price.");
            }
            if (!IsTimeValid(model.ProductionTime))
            {
                ModelState.AddModelError(nameof(model.ProductionTime), "Invalid time format! Please use hh:mm:ss.");
            }
            if (!ModelState.IsValid)
            {
                model.Materials = await context.Materials.Where(m => m.IsDeleted == false).ToListAsync();
                return View(model);
            }

            TimeSpan parsedTime;
            TimeSpan.TryParseExact(model.ProductionTime, TimeFormat, CultureInfo.InvariantCulture, TimeSpanStyles.None, out parsedTime);
            decimal parsedPrice;
            decimal.TryParse(model.Price, out parsedPrice);
            decimal parsedCost;
            decimal.TryParse(model.Price, out parsedCost);

            Product product = new()
            {
                Name = model.Name,
                ImageUrl = model.ImageUrl,
                Price = parsedPrice,
                Weight = model.Weight,
                Size = model.Size,
                ProductionCost = parsedPrice,
                ProductionTime = parsedTime,
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
        [Authorize(Roles = "Admin, Manager")]
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
        [Authorize(Roles = "Admin, Manager")]
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
                Price = current.Price.ToString("F2"),
                ImageUrl = current.ImageUrl,
                Weight = current.Weight,
                Size = current.Size,
                ProductionCost = current.ProductionCost.ToString("F2"),
                ProductionTime = current.ProductionTime.ToString(),
                ProductMaterials = current.ProductMaterials,
                ProductOrders = current.ProductOrders,
                IsDeleted = current.IsDeleted,
                Materials = await context.Materials.AsNoTracking().ToListAsync()
            };
            model.SelectedMaterialsIds = model.ProductMaterials.Select(mp => mp.MaterialId).ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin, Manager")]
        //Edit
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ProductViewModel model)
        {
            if (!IsPriceValid(model.Price))
            {
                ModelState.AddModelError(nameof(model.Price), "Please enter a valid price.");
            }
            if (!IsPriceValid(model.ProductionCost))
            {
                ModelState.AddModelError(nameof(model.ProductionCost), "Please enter a valid cost.");
            }
            if (!IsTimeValid(model.ProductionTime))
            {
                ModelState.AddModelError(nameof(model.ProductionTime), "Invalid time format! Please use hh:mm:ss.");
            }
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
            
            TimeSpan parsedTime;
            TimeSpan.TryParseExact(model.ProductionTime, TimeFormat, CultureInfo.InvariantCulture, TimeSpanStyles.None, out parsedTime);
            decimal parsedPrice;
            decimal.TryParse(model.Price, out parsedPrice);
            decimal parsedCost;
            decimal.TryParse(model.ProductionCost, out parsedCost);


            toEdit.Name = model.Name;
            toEdit.ImageUrl = model.ImageUrl;
            toEdit.Weight = model.Weight;
            toEdit.Size = model.Size;
            toEdit.ProductionCost = parsedCost;
            toEdit.ProductionTime = parsedTime;
            toEdit.Price = parsedPrice;

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


        //Cart
        [Authorize(Roles = "Client, Admin")]
        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            string? currentUser = GetCurrentUserId();
            if (currentUser == null) 
            {
                return RedirectToAction(nameof(Index));
            }
            List<ProductIndexViewModel> model = await context.Products
                .Where(p => p.IsDeleted == false)
                .Where(p => p.ProductsClients.Any(pc => pc.Client.UserId.ToString() == currentUser))
                .Select(p => new ProductIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price.ToString(),
                    PictureUrl = p.ImageUrl
                })
                .AsNoTracking()
                .ToListAsync();
            return View(model);
        }

        //Add to cart
        [Authorize(Roles = "Admin, Client")]
        [HttpPost]
        public async Task<IActionResult> AddToCart(Guid id)
        {
            string? userId = GetCurrentUserId();
            Product? product = await context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            ClientProduct? clientProduct = await context.ClientsProducts
                .FirstOrDefaultAsync(pc => pc.ProductId == id && pc.Client.UserId.ToString() == userId);

            if (clientProduct != null)
            {
                return RedirectToAction(nameof(Index));
            }

            ClientProduct addedProduct = new ClientProduct
            {
                ClientId = context.Clients.First(c => c.UserId.ToString() == userId).Id,
                ProductId = product.Id
            };

            await context.ClientsProducts.AddAsync(addedProduct);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(Guid id)
        {
            string? userId = GetCurrentUserId();
            if (userId == null)
            {
                return NotFound();
            }
            ClientProduct? toRemove = await context.ClientsProducts 
                .Include(cp => cp.Client)
                .FirstOrDefaultAsync(pc => pc.ProductId == id && pc.Client.UserId.ToString() == userId);

            if (toRemove == null) 
            {
                return NotFound();
            }

            context.ClientsProducts.Remove(toRemove);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Cart));
        }


            //Common
            private string? GetCurrentUserId()
        {
            string? userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
    }
}
