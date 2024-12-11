using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Models.Orders
{
    public class OrderViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid ClientId { get; set; }


        public Client? Client { get; set; }

        [Required]
        public string Address { get; set; } = null!;

        public string? AssignedDate { get; set; }

        public Guid? ShipmentId { get; set; }

        public Shipment? Shipment { get; set; }

        [Required]
        public string TotalPrice { get; set; } = null!;

        [Required]
        public IList<OrderProduct> OrdersProducts { get; set; } = new List<OrderProduct>();

        [Required]
        public bool IsActive { get; set; }
    }
}
