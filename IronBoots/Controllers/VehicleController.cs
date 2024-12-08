using IronBoots.Data;
using IronBoots.Data.Models;
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


        //Details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Vehicle? currentVehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (currentVehicle == null)
            {
                return NotFound();
            }

            VehicleViewModel model = new VehicleViewModel()
            {
                Id = currentVehicle.Id,
                Name = currentVehicle.Name,
                WeightCapacity = currentVehicle.WeightCapacity,
                SizeCapacity = currentVehicle.SizeCapacity,
                ShipmentId = currentVehicle.ShipmentId,
                IsAvailable = currentVehicle.IsAvailable
            };
            Shipment? currentShipment = await context.Shipments.FirstOrDefaultAsync(s => s.Id == model.ShipmentId);
            if (currentShipment != null)
            {
                model.Shipment = currentShipment;
            }

            return View(model);
        }
    }
}
