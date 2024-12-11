using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Controllers
{
    [Authorize]
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
            if (User.IsInRole("Client"))
            {
                string? userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                List<OrderIndexViewModel> model = await context.Orders
                    .Where(o => o.IsActive == true && o.Client.UserId.ToString() == userId)
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
            else
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
        }

        //Details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Order? current = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            Client? currentClient = await context.Clients.FirstOrDefaultAsync(c => c.Id == current.ClientId);
            if (current == null || currentClient == null)
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


        //Cancel order
        [HttpPost]
        public async Task<IActionResult> Cancel(Guid id)
        {
            Order? toCancel = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if (toCancel == null)
            {
                return NotFound();
            }
            toCancel.IsActive = false;
            if (toCancel.ShipmentId != null)
            {
                Shipment? currentShipment = await context.Shipments.FirstOrDefaultAsync(s => s.Id == toCancel.ShipmentId);
                if (currentShipment != null)
                {
                    currentShipment.Orders.Remove(toCancel);
                }
            }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
