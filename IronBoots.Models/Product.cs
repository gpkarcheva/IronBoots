using System.ComponentModel.DataAnnotations;

namespace IronBoots.Data.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        public double Weight { get; set; }
        [Required]
        public double Size { get; set; }
        [Required]
        public decimal ProductionCost { get; set; }
        [Required]
        public TimeSpan ProductionTime { get; set; }
        [Required]
        ICollection<ProductMaterial> ProductMaterials = new HashSet<ProductMaterial>();
        [Required]
        public ICollection<OrderProduct> ProductOrders = new HashSet<OrderProduct>();
    }
}
