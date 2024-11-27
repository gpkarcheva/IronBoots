using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Data.Models
{
    public class Vehicle
    {
        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }
        [Required]
        [Comment("Max weight the vehicle can carry")]
        public double WeightCapacity { get; set; }
        [Required]
        [Comment("Max size the vehicle can carry")]
        public double SizeCapacity { get; set; }
        [Required]
        [Comment("A list of orders to be shipped with this vehicle")]
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
