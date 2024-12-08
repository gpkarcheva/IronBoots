using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext context;

        public OrderController(ApplicationDbContext _context)
        {
            context = _context;
        }

        //Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<OrderIndexViewModel> model = await context.Orders
                .Where(o => o.IsActive == true)
                .Select(o => new OrderIndexViewModel()
                {
                    Id = o.Id,
                    Client = o.Client,
                    TotalPrice = o.TotalPrice,
                    PlannedAssignedDate = o.PlannedAssignedDate,
                    ActualAssignedDate = o.ActualAssignedDate
                })
                .ToListAsync();
            return View(model);
        }

        //Details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Order? current = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (current == null)
            {
                return NotFound(); //implement pls ms
            }

            OrderViewModel model = new OrderViewModel()
            {
                Id = current.Id,
                ClientId = current.Client.Id,
                Client = current.Client,
                TotalPrice = current.TotalPrice,
                PlannedAssignedDate = current.PlannedAssignedDate,
                ActualAssignedDate = current.ActualAssignedDate,
                ShipmentId = current.ShipmentId,
                Shipment = current.Shipment,
                Products = await context.OrdersProducts
                .Where(op => op.OrderId == id)
                .Select(o => o.Product)
                .ToListAsync(),
                IsActive = current.IsActive
            };
            return View(model);
        }
    }
}
