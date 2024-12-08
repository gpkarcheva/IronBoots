using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Models.Shipments
{
    public class ShipmentIndexViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Vehicle Vehicle { get; set; } = null!;

        [Required]
        public Status ShipmentStatus { get; set; }

        public enum Status
        {
            PendingShipment,
            Shipped,
            InTransit,
            Delivered
        }
    }
}
