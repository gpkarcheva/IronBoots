using IronBoots.Data;
using IronBoots.Models.Shipments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly ApplicationDbContext context;

        public ShipmentController(ApplicationDbContext _context)
        {
            context = _context;
        }

        //Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<ShipmentIndexViewModel> model = await context.Shipments
                .Where(s => s.ShipmentStatus != Data.Models.Shipment.Status.Delivered)
                .Select(s => new ShipmentIndexViewModel()
                {
                    Id = s.Id,
                    Vehicle = s.Vehicle,
                    ShipmentStatus = (ShipmentIndexViewModel.Status)s.ShipmentStatus
                })
                .AsNoTracking()
                .ToListAsync();

            return View(model);
        }
    }
}
