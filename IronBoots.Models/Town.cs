using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Data.Models
{
    public class Town
    {
        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        [Comment("Town name")]
        public string Name { get; set; } = null!;
    }
}
