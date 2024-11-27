using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Data.Models
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        public Guid AddressId { get; set; }
        [Required]
        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; } = null!;
        [Required]
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        [Required]
        public ApplicationUser User { get; set; } = null!;

    }
}
