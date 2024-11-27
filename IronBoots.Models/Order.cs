using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Data.Models
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid ClientId { get; set; }
        [Required]
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;
        [Required]
        public Guid AddressId { get; set; }
        [Required]
        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; } = null!;
        [Required]
        public DateOnly PlannedAssignedDate { get; set; }
        [Required]
        public DateOnly ActualAssignedDate { get; set; }
        [Required]
        public Guid ShipmentId { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
    }
}
