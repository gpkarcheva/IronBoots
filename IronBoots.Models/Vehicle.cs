using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Data.Models
{
    public class Vehicle
    {
        public Vehicle()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }


        [Required]
        [MinLength(1)]
        [MaxLength(30)]
        public string Name { get; set; } = null!;


        [Required]
        [Comment("Max weight the vehicle can carry in kg")]
        public double WeightCapacity { get; set; }


        [Required]
        [Comment("Max size the vehicle can carry in cm2")]
        public double SizeCapacity { get; set; }


        [Comment("Id of the shipment")]
        public Guid? ShipmentId { get; set; }


        [ForeignKey(nameof(ShipmentId))]
        [Comment("Shipment the order belongs to")]
        public Shipment? Shipment { get; set; }


        [Required]
        [Comment("Soft deletion flag")]
        public bool IsDeleted { get; set; }
    }
}
