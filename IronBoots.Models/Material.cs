using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UrlAttribute = System.ComponentModel.DataAnnotations.UrlAttribute;
using static IronBoots.Common.EntityValidationConstants.MaterialValidation;

namespace IronBoots.Data.Models
{
    public class Material
    {
        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }


        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        [Comment("Name/Code of the material")]
        public string Name { get; set; } = null!;


        [Required]
        [Range(typeof(decimal), nameof(PriceMin), nameof(PriceMax))]
        [Precision(18,2)]
        [Comment("Purchase price of the material")]
        public decimal Price { get; set; }


        [Comment("Url of product image")]
        public string? ImageUrl { get; set; }


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
