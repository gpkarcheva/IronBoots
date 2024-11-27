using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronBoots.Data.Models
{
    public class OrderProduct
    {
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; } = null!;
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }
}
