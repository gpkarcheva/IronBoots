using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Data.Models
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(80)]
        public string AddressText { get; set; } = null!;
        [Required]
        public Guid TownId { get; set; }
        [Required]
        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; } = null!;

    }
}
