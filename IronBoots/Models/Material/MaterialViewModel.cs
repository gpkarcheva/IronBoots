using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static IronBoots.Common.EntityValidationConstants.MaterialValidation;
namespace IronBoots.Models.Material
{
    public class MaterialViewModel
    {
        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), nameof(PriceMin), nameof(PriceMax))]
        [Precision(18, 2)]
        public decimal Price { get; set; }

        public string? PictureUrl { get; set; }

        [Required]
        [Url]
        public string DistrubutorContact { get; set; } = null!;

        [Required]
        public ICollection<ProductMaterial> MaterialProducts { get; set; } = new HashSet<ProductMaterial>();
    }
}
