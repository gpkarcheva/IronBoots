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
            Client? currentClient = await context.Clients.FirstOrDefaultAsync(c => c.Id == current.ClientId);
            if (currentClient == null)
            {
                return NotFound();
            }

            List<OrderProduct> orderProducts = await context.OrdersProducts
                .Where(op => op.OrderId == id)
                .ToListAsync();

            foreach (var op in orderProducts)
            {
                op.Order = await context.Orders.FirstOrDefaultAsync(o => o.Id == op.OrderId);
                op.Product = await context.Products.FirstOrDefaultAsync(p => p.Id == op.ProductId);
            }

            OrderViewModel model = new OrderViewModel()
            {
                Id = id,
                ClientId = current.ClientId,  
                Client = currentClient,
                TotalPrice = current.TotalPrice,
                PlannedAssignedDate = current.PlannedAssignedDate,
                ActualAssignedDate = current.ActualAssignedDate.ToString(),
                ShipmentId = current.ShipmentId,
                Shipment = current.Shipment,
                OrdersProducts = orderProducts,
                IsActive = current.IsActive
            };

            if (model.ShipmentId != null)
            {
                model.ActualAssignedDate = DateTime.Now.ToString();
            }
            return View(model);
        }
    }
}
