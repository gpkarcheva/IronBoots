using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static IronBoots.Common.EntityValidationConstants.ProductMaterialValidation;

namespace IronBoots.Data.Models
{
    public class ProductMaterial
    {
        [Required]
        [Comment("Id of the product, PK")]
        public Guid ProductId { get; set; }


        [Required]
        [ForeignKey(nameof(ProductId))]
        [Comment("Product object")]
        public Product Product { get; set; } = null!;


        [Required]
        [Comment("Id of the material, PK")]
        public Guid MaterialId { get; set; }


        [Required]
        [ForeignKey(nameof(MaterialId))]
        [Comment("Material object")]
        public Material Material { get; set; } = null!;


        [Required]
        [Range(QuantityMin, QuantityMax)]
        [Comment("Quantity needed to make the product")]
        public int MaterialQuantity { get; set; }
    }
}
