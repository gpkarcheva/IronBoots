using System.ComponentModel.DataAnnotations;

namespace IronBoots.Data.Models
{
    public class Vehicle
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public double WeightCapacity { get; set; }
        [Required]
        public double SizeCapacity { get; set; }
        [Required]
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
