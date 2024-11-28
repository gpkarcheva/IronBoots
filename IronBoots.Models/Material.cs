using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UrlAttribute = System.ComponentModel.DataAnnotations.UrlAttribute;

namespace IronBoots.Data.Models
{
    public class Material
    {
        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(20)]
        [Comment("Name/Code of the material")]
        public string Name { get; set; } = null!;
        [Required]
        [Comment("Purchase price of the material")]
        public decimal Price { get; set; }
        [Required]
        [Url]
        [Comment("Contact page of the distributor")]
        public string DistrubutorContact { get; set; } = null!;
        [Required]
        [Comment("A list of products that require the current material")]
        public ICollection<ProductMaterial> MaterialProducts { get; set; } = new HashSet<ProductMaterial>();
        [Required]
        [Comment("Soft deletion flag")]
        public bool IsDeleted { get; set; }
    }
}
