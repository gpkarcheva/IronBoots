using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Data.Models
{
    public class Client
    {
        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }


        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        [Comment("Company name of the client")]
        public string Name { get; set; } = null!;


        [Required]
        [Comment("ID of the address")]
        public Guid AddressId { get; set; }


        [Required]
        [ForeignKey(nameof(AddressId))]
        [Comment("Address object")]
        public Address Address { get; set; } = null!;


        [Required]
        [Comment("Orders the client currently has open")]
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();


        [Required]
        [Comment("Current user")]
        public ApplicationUser User { get; set; } = null!;


        [Required]
        [Comment("Soft deletion flag")]
        public bool IsDeleted { get; set; }

    }
}
