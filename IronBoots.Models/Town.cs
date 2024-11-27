using System.ComponentModel.DataAnnotations;

namespace IronBoots.Data.Models
{
    public class Town
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string Name { get; set; } = null!;
    }
}
