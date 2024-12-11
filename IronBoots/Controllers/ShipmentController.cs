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
            current.Orders = await context.Orders.Where(o => o.ShipmentId == id).ToListAsync();
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
                Id = current.Id,
                VehicleId = current.VehicleId,
                Vehicle = current.Vehicle,
                ShipmentDate = current.ShipmentDate?.ToString("yyyy-MM-dd"),
                ShipmentStatus = current.ShipmentStatus,
                AllOrders = await context.Orders.ToListAsync(),
                SelectedOrdersIds = current.Orders.Select(o => o.Id).ToList(),
                VehicleList = await context.Vehicles.Where(v => v.IsDeleted == false && v.IsAvailable == true).ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ShipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Shipment? shipment = await context.Shipments
                .Include(s => s.Orders)
                .Include(s => s.Vehicle)
                .FirstOrDefaultAsync(s => s.Id == model.Id);

            if (shipment == null)
            {
                return NotFound();
            }

            // Update shipment properties
            shipment.VehicleId = model.VehicleId;
            shipment.ShipmentDate = string.IsNullOrWhiteSpace(model.ShipmentDate) ? null : DateTime.Parse(model.ShipmentDate);
            shipment.ShipmentStatus = model.ShipmentStatus;

            // Update selected orders
            var selectedOrders = await context.Orders
                .Where(o => model.SelectedOrdersIds.Contains(o.Id))
                .ToListAsync();

            shipment.Orders.Clear(); // Clear existing orders in the shipment
            shipment.Orders = selectedOrders; // Add the newly selected orders

            // Logic for updating shipment, vehicle, and orders
            if (shipment.ShipmentDate.HasValue)
            {
                foreach (var order in shipment.Orders)
                {
                    order.AssignedDate = shipment.ShipmentDate;
                }

                if (shipment.ShipmentDate.Value.Date == DateTime.Today)
                {
                    shipment.ShipmentStatus = Status.InTransit;
                    shipment.Vehicle.IsAvailable = false;
                }

                if (shipment.ShipmentDate.Value.AddDays(1).Date <= DateTime.Today)
                {
                    shipment.ShipmentStatus = Status.Delivered;
                    shipment.Vehicle.IsAvailable = true;

                    foreach (var order in shipment.Orders)
                    {
                        order.IsActive = false;
                    }
                }
            }

            await context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = shipment.Id });
        }


        //Add
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ShipmentViewModel model = new()
            {
                AllOrders = await context.Orders.Where(o => o.IsActive == true).ToListAsync(),
                VehicleList = await context.Vehicles.Where(v => v.IsAvailable == true && v.IsDeleted == false).ToListAsync()
            };
            return View(model);
        }
    }
}
