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
			if (TempData["ErrorMessage"] != null)
			{
				ViewData["ErrorMessage"] = TempData["ErrorMessage"];
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

        //Add
		[HttpGet]
		public IActionResult Add()
		{
			VehicleViewModel model = new VehicleViewModel();
			return View(model);
		}

        [HttpPost]
        public async Task<IActionResult> Add(VehicleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            Vehicle toAdd = new Vehicle()
            {
                Name = model.Name,
                WeightCapacity = model.WeightCapacity,
                SizeCapacity = model.SizeCapacity,
                ShipmentId = Guid.Empty,
                IsAvailable = true
            };

            await context.Vehicles.AddAsync(toAdd);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		//Delete
		[HttpPost]
		public async Task<IActionResult> Delete(Guid id)
		{
			Vehicle? toDelete = await context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
			if (toDelete == null)
			{
				return NotFound();
			}
			if (toDelete.IsAvailable == false)
			{
				TempData["ErrorMessage"] = "Please complete the shipment before deleting the vehicle.";
				return RedirectToAction(nameof(Details), new {id = id});
			}
			toDelete.IsDeleted = true;
			await context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}


        //Edit
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Vehicle? toEdit = await context.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
            if (toEdit == null)
            {
                return NotFound();
            }
            VehicleViewModel model = new VehicleViewModel()
            {
                Id = id,
                Name = toEdit.Name,
                WeightCapacity = toEdit.WeightCapacity,
                SizeCapacity = toEdit.SizeCapacity
            };
            return View(model);
        }
	}
}
