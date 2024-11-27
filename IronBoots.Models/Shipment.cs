using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Data.Models
{
    public class Shipment
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid VehicleId { get; set; }
        [Required]
        [ForeignKey(nameof(VehicleId))]
        public Vehicle Vehicle { get; set; } = null!;
        [Required]
        public DateTime ShipmentDate { get; set; }
        [Required]
        public DateTime DeliveryDate { get; set; }
        public enum Status
        {
            PendingShipment,
            Shipped,
            InTransit,
            Delivered
        }
    }
}
