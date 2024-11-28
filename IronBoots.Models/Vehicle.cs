using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Data.Models
{
    public class Vehicle
    {
        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }
        [Required]
        [Comment("Max weight the vehicle can carry")]
        public double WeightCapacity { get; set; }
        [Required]
        [Comment("Max size the vehicle can carry")]
        public double SizeCapacity { get; set; }
        [Required]
        [Comment("Id of the shipment")]
        public Guid ShipmentId { get; set; }
        [Required]
        [Comment("Shipment the order belongs to")]
        public Shipment Shipment { get; set; } = null!;
        [Required]
        [Comment("Soft deletion flag")]
        public bool IsDeleted { get; set; }
    }
}
