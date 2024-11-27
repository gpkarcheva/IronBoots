using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronBoots.Data.Models
{
    public class ProductMaterial
    {
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
        [Required]
        public Guid MaterialId { get; set; }
        [Required]
        [ForeignKey(nameof(MaterialId))]
        public Material Material { get; set; } = null!;
        [Required]
        public int MaterialQuantity { get; set; }
    }
}
