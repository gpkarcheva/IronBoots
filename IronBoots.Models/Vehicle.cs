using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
