using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static IronBoots.Common.ExtensionMethods;

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
                        AssignedDate = o.AssignedDate.ToString()
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
                    AssignedDate = o.AssignedDate.ToString()
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
                Address = current.Address,
                AssignedDate = current.AssignedDate.ToString(),
                ShipmentId = current.ShipmentId,
                Shipment = current.Shipment,
                OrdersProducts = current.OrderProducts,
                IsActive = current.IsActive
            };

            if (model.ShipmentId != null)
            {
                model.AssignedDate = DateTime.Now.ToString();
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
            toCancel.AssignedDate = null;
            if (toCancel.ShipmentId != null)
            {
                toCancel.Shipment?.Orders.Remove(toCancel);
            }
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Checkout
        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            List<decimal> productPrices = await context.ClientsProducts.Select(cp => cp.Product.Price).ToListAsync();
            decimal totalPrice = productPrices.Sum();

            string? userId = GetCurrentUserId();

            OrderViewModel model = new OrderViewModel()
            {
                Id = Guid.NewGuid(),
                ClientId = context.Clients
                .First(c => c.UserId.ToString() == userId).Id,
                Client = context.Clients
                .First(c => c.UserId.ToString() == userId),
                TotalPrice = totalPrice.ToString(),
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            List<Product> products = await context.ClientsProducts.Where(cp => cp.ClientId == model.ClientId)
                .Select(cp => cp.Product)
                .ToListAsync();
            if(!products.Any())
            {
                return RedirectToAction("Cart", "Product");
            }
            decimal totalPrice;
            decimal.TryParse(model.TotalPrice, out totalPrice);
            Order order = new()
            {
                ClientId = model.ClientId,
                Address = model.Address,
                TotalPrice = totalPrice,
                IsActive = true
            };
            foreach (var product in products)
            {
                await context.OrdersProducts.AddAsync(new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = product.Id
                });
            }
            List<ClientProduct> cps = await context.ClientsProducts.Where(cp => cp.ClientId == model.ClientId).ToListAsync();
            context.ClientsProducts.RemoveRange(cps);
            await context.Orders.AddAsync(order);
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
