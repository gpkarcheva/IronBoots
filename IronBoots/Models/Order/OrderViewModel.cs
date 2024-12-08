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

        [Required]
        public Client Client { get; set; }

        [Required]
        public DateTime PlannedAssignedDate { get; set; }

        public string? ActualAssignedDate { get; set; }

        public Guid? ShipmentId { get; set; }

        public Shipment? Shipment { get; set; }

        [Required]
        [Range(typeof(decimal), "0.01", "10000000.00")] //TODO FIX MAGIC NUMBERS
        [Precision(18, 2)]
        public decimal TotalPrice { get; set; }

        [Required]
        public List<OrderProduct> OrdersProducts { get; set; } = new List<OrderProduct>();

        [Required]
        public bool IsActive { get; set; }
    }
}
