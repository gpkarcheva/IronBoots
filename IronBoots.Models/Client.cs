using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static IronBoots.Common.EntityValidationConstants.ClientValidation;

namespace IronBoots.Data.Models
{
    public class Client
    {
        public Client()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }


        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        [Comment("Company name of the client")]
        public string Name { get; set; } = null!;


        [Required]
        [Comment("ID of the address")]
        public Guid AddressTownId { get; set; }


        [Required]
        [ForeignKey(nameof(AddressTownId))]
        [Comment("Address object")]
        public AddressTown AddressTown { get; set; } = null!;


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
