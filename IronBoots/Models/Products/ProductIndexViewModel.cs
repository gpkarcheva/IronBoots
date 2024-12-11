using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static IronBoots.Common.EntityValidationConstants.ProductValidation;

namespace IronBoots.Models.Products
{
    public class ProductIndexViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(typeof(decimal), "0.01", "100000000.00")] //TODO FIX MAGIC NUMBERS
        public decimal Price { get; set; }

        [Url]
        public string? PictureUrl { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
