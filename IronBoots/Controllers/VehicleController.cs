using IronBoots.Data;
using IronBoots.Models.Vehicles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Controllers
{
    public class VehicleController : Controller
    {
        private readonly ApplicationDbContext context;

        public VehicleController(ApplicationDbContext _context)
        {
            context = _context;
        }

        //Index
        public async Task<IActionResult> Index()
        {
            List<VehicleIndexViewModel> model = await context.Vehicles
                .Where(v => v.IsDeleted == false)
                .Select(v => new VehicleIndexViewModel()
                {
                    Id = v.Id,
                    Name = v.Name,
                    IsAvailable = v.IsAvailable
                })
                .AsNoTracking()
                .ToListAsync();
            return View(model);
        }
    }
}
