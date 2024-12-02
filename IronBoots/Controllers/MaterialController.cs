using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        //Remove
        //Edit
    }
}
