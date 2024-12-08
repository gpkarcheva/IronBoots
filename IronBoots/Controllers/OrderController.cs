using IronBoots.Data;
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
    }
}
