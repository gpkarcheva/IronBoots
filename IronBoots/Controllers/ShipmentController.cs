﻿using IronBoots.Common;
using IronBoots.Data;
using IronBoots.Data.Models;
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
            Shipment? current = await context.Shipments.FirstOrDefaultAsync(s => s.Id == id);
            if (current == null)
            {
                return NotFound(); //jesus just implement it already
            }
            ShipmentViewModel model = new ShipmentViewModel()
            {
                Id = id,
                VehicleId = current.VehicleId,
                Vehicle = await context.Vehicles.FirstOrDefaultAsync(v => v.Id == current.VehicleId),
                Orders = current.Orders,
                ShipmentDate = current.ShipmentDate,
                DeliveryDate = current.DeliveryDate,
                ShipmentStatus = current.ShipmentStatus
            };
            return View(model);
        }
    }
}
