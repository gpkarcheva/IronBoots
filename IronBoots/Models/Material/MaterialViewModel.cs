using IronBoots.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static IronBoots.Common.EntityValidationConstants.MaterialValidation;
namespace IronBoots.Models.Materials
{
    public class MaterialViewModel
    {
        public MaterialViewModel()
        {
            MaterialProducts = new List<ProductMaterial>();
            SelectedProductIds = new List<Guid>();
            Products = new List<Product>();
        }

        [Required]
        public Guid Id { get; set; }

        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        public string Name { get; set; } = null!;

        [Required]
        public string Price { get; set; } = null!;

        public string? PictureUrl { get; set; }

        [Required]
        [Url]
        public string DistributorContact { get; set; } = null!;

        [Required]
        public IList<ProductMaterial> MaterialProducts { get; set; }

        public IList<Guid> SelectedProductIds { get; set; }

        public IList<Product> Products { get; set; }
    }
}
