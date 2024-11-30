using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static IronBoots.Common.EntityValidationConstants.OrderValidation;

namespace IronBoots.Data.Models
{
    public class Order
    {
        public Order()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }


        [Required]
        [Comment("Id of the client")]
        public Guid ClientId { get; set; }


        [Required]
        [ForeignKey(nameof(ClientId))]
        [Comment("Client object")]
        public Client Client { get; set; } = null!;


        [Required]
        [Comment("When is the order supposed to be assigned to a shipment")]
        public DateTime PlannedAssignedDate { get; set; }


        [Required]
        [Comment("When is the order actually assigned to a shipment")]
        public DateTime ActualAssignedDate { get; set; }


        [Required]
        [Comment("Id of the shipment the order belongs to")]
        public Guid ShipmentId { get; set; }


        [Required]
        [ForeignKey(nameof(ShipmentId))]
        [Comment("Shipment object")]
        public Shipment Shipment { get; set; } = null!;


        [Required]
        [Range(typeof(decimal), nameof(PriceMin), nameof(PriceMax))]
        [Precision(18, 2)]
        [Comment("Total price of the order")]
        public decimal TotalPrice { get; set; }


        [Required]
        [Comment("Which products are required for the order")]
        public IList<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();


        [Required]
        [Comment("Active orders flag")]
        public bool IsActive { get; set; }
    }
}
