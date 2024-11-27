using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace IronBoots.Data.Models
{
    public class Address
    {
        [Key]
        [Comment("Identifier - GUID")]
        public Guid Id { get; set; }
        [Comment("Address details")]
        [Required]
        [MinLength(3)]
        [MaxLength(80)]
        public string AddressText { get; set; } = null!;
        [Required]
        [Comment("Town Id for easy tracking of orders/shipments")]
        public Guid TownId { get; set; }
        [Required]
        [ForeignKey(nameof(TownId))]
        [Comment("Town object")]
        public Town Town { get; set; } = null!;

    }
}
