using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UrlAttribute = System.ComponentModel.DataAnnotations.UrlAttribute;
using static IronBoots.Common.EntityValidationConstants.MaterialValidation;

namespace IronBoots.Data.Models
{
    public class Material
    {
        public Material()
        {
            Id = Guid.NewGuid();
			MaterialProducts = new List<ProductMaterial>();
		}

        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }


        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        [Comment("Name/Code of the material")]
        public string Name { get; set; } = null!;


        [Required]
        [Range(typeof(decimal), nameof(PriceMin), nameof(PriceMax)), Precision(18, 2)]
        [Comment("Purchase price of the material")]
        public decimal Price { get; set; }


        [Comment("Url of product image")]
        public string? PictureUrl { get; set; }


        [Required, Url]
        [Comment("Contact page of the distributor")]
        public string DistributorContact { get; set; } = null!;


        [Required]
        [Comment("A list of products that require the current material")]
        public IList<ProductMaterial> MaterialProducts { get; set; }


        [Required]
        [Comment("Soft deletion flag")]
        public bool IsDeleted { get; set; }
    }
}
