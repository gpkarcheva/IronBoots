using IronBoots.Common;
using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Shipments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
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
                .Where(s => s.ShipmentStatus != Status.Delivered)
                .Select(s => new ShipmentIndexViewModel()
                {
                    Id = s.Id,
                    Vehicle = s.Vehicle,
                    ShipmentStatus = s.ShipmentStatus
                })
                .AsNoTracking()
                .ToListAsync();

            return View(model);
        }

        //Details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Shipment? current = await context.Shipments
                .Include(s => s.Vehicle)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (current == null)
            {
                return NotFound();
            }
            ShipmentViewModel model = new()
            {
                Id = id,
                VehicleId = current.VehicleId,
                Vehicle = current.Vehicle,
                AllOrders = current.Orders,
                ShipmentDate = current.ShipmentDate.ToString(),
                DeliveryDate = current.DeliveryDate.ToString(),
                ShipmentStatus = current.ShipmentStatus
            };
            return View(model);
        }

        //Edit
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Shipment? current = await context.Shipments
                .Include(s => s.Vehicle)
                .Include(s => s.Orders)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (current == null)
            {
                return NotFound();
            }
            ShipmentViewModel model = new()
            {
                Id = id,
                VehicleId = current.VehicleId,
                Vehicle = current.Vehicle,
                ShipmentDate = current.ShipmentDate.ToString(),
                DeliveryDate = current.DeliveryDate.ToString(),
                ShipmentStatus = current.ShipmentStatus,
                AllOrders = context.Orders.ToList()
            };
            model.SelectedOrdersIds = current.Orders.Select(o => o.Id).ToList();

            return View(model);
        }
    }
}
