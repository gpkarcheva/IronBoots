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

        [Url]
        public string? PictureUrl { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
