using IronBoots.Data;
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
                    DistrubutorContact = m.DistrubutorContact,
                    MaterialProducts = m.MaterialProducts
                })
                .AsNoTracking()
                .ToListAsync();

            return View(model);
        }

        //Add
        //Remove
        //Edit
        //Details
    }
}
