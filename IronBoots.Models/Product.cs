using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static IronBoots.Common.EntityValidationConstants.ProductValidation;

namespace IronBoots.Data.Models
{
    public class Product
    {
        public Product()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }


        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        [Comment("Name/Code of the product")]
        public string Name { get; set; } = null!;


        [Comment("Url of product image")]
        public string? ImageUrl { get; set; }


        [Required]
        [Range(WeightMin, WeightMax)]
        [Comment("Net weight of the product in kg")]
        public double Weight { get; set; }


        [Required]
        [Range(SizeMin, SizeMax)]
        [Comment("Net size of the product in cm2")]
        public double Size { get; set; }


        [Required]
        [Range(typeof(decimal), nameof(CostMin), nameof(CostMax))]
        [Precision(18, 2)]
        [Comment("Cost to produce the product")]
        public decimal ProductionCost { get; set; }


        [Required]
        [Comment("Time required to produce the product")]
        public TimeSpan ProductionTime { get; set; }


        [Required]
        [Comment("Materials required to produce")]
        public ICollection<ProductMaterial> ProductMaterials = new HashSet<ProductMaterial>();


        [Required]
        [Comment("Orders in which the product is required")]
        public ICollection<OrderProduct> ProductOrders = new HashSet<OrderProduct>();


        [Required]
        [Comment("Soft deletion flag")]
        public bool IsDeleted { get; set; }
    }
}
