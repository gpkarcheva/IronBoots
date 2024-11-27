using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Data.Models
{
    public class Vehicle
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [Min(1)]
        public double WeightCapacity { get; set; }
        [Required]
        [Min(1)]
        public double SizeCapacity { get; set; }
    }
}
