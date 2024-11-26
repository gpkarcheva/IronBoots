using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

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
        [Min(1)]
        public double Weight { get; set; }
        [Required]
        [Min(1)]
        public double Size { get; set; }
        [Required]
        [Min(1)]
        public decimal ProductionCost { get; set; }
        [Required]
        [Min(1)]
        public TimeSpan ProductionTime { get; set; }
    }
}
