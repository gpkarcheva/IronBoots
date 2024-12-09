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
				Products = await context.Products.AsNoTracking().ToListAsync()
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Add(MaterialViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

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
				DistributorContact = currentMaterial.DistributorContact,
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
				toEdit.DistributorContact = model.DistributorContact;
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
