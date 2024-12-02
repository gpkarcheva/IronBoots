using static IronBoots.Common.EntityValidationConstants.MaterialValidation;
using IronBoots.Data.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace IronBoots.Models
{
    public class MaterialIndexViewModel
    {
        [Required]
        public Guid Id { get; set; }
        
        
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
        public string DistributorContact { get; set; } = null!;


        [Required]
        public ICollection<ProductMaterial> MaterialProducts { get; set; } = new HashSet<ProductMaterial>();


        [Required]
        public bool IsDeleted { get; set; }
    }
}
