using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Data.Models
{
    public class Product
    {
        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }


        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        [Comment("Name/Code of the product")]
        public string Name { get; set; } = null!;


        [Required]
        [Comment("Net weight of the product")]
        public double Weight { get; set; }


        [Required]
        [Comment("Net size of the product")]
        public double Size { get; set; }


        [Required]
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
