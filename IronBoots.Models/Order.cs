using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Data.Models
{
    public class Order
    {
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
        [Comment("Id of the address")]
        public Guid AddressId { get; set; }
        [Required]
        [ForeignKey(nameof(AddressId))]
        [Comment("Address object")]
        public Address Address { get; set; } = null!;
        [Required]
        [Comment("When is the order supposed to be assigned to a shipment")]
        public DateOnly PlannedAssignedDate { get; set; }
        [Required]
        [Comment("When is the order actually assigned to a shipment")]
        public DateOnly ActualAssignedDate { get; set; }
        [Required]
        [Comment("Id of the shipment the order belongs to")]
        public Guid ShipmentId { get; set; }
        [Required]
        [ForeignKey(nameof(ShipmentId))]
        [Comment("Shipment object")]
        public Shipment Shipment { get; set; } = null!;
        [Required]
        [Comment("Total price of the order")]
        public decimal TotalPrice { get; set; }
        [Required]
        [Comment("Which products are required for the order")]
        public ICollection<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();
        [Required]
        [Comment("Active orders flag")]
        public bool IsActive { get; set; }
    }
}
