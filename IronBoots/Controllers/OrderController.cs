using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
                string? userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value; //extract extension

                List<OrderIndexViewModel> model = await context.Orders
                    .Where(o => o.IsActive == true && o.Client.UserId.ToString() == userId)
                    .Select(o => new OrderIndexViewModel()
                    {
                        Id = o.Id,
                        Client = o.Client,
                        TotalPrice = o.TotalPrice.ToString(),
                        PlannedAssignedDate = o.PlannedAssignedDate.ToString(),
                        ActualAssignedDate = o.ActualAssignedDate.ToString()
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
                    TotalPrice = o.TotalPrice.ToString("F2"),
                    PlannedAssignedDate = o.PlannedAssignedDate.ToString(),
                    ActualAssignedDate = o.ActualAssignedDate.ToString()
                })
                .ToListAsync();
                return View(model);
            }
        }

        //Details
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            Order? current = await context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(o => o.Client)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (current == null
                || !User.IsInRole("Admin")
                && !User.IsInRole("Manager")
                && User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value != current.Client.UserId.ToString())
            {
                return NotFound();
            }

            OrderViewModel model = new()
            {
                Id = id,
                ClientId = current.ClientId,
                Client = current.Client,
                TotalPrice = current.TotalPrice.ToString("F2"),
                PlannedAssignedDate = current.PlannedAssignedDate.ToString(),
                ActualAssignedDate = current.ActualAssignedDate.ToString(),
                ShipmentId = current.ShipmentId,
                Shipment = current.Shipment,
                OrdersProducts = current.OrderProducts,
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
            Order? toCancel = await context.Orders
                .Include(o => o.Shipment)
                .FirstOrDefaultAsync(o => o.Id == id);
            if (toCancel == null)
            {
                return NotFound();
            }
            toCancel.IsActive = false;
            toCancel.PlannedAssignedDate = default;
            toCancel.ActualAssignedDate = null;
            if (toCancel.ShipmentId != null)
            {
                toCancel.Shipment?.Orders.Remove(toCancel);
            }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        //Common
        public string? GetCurrentUserId()
        {
            string? userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
    }
}
