using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Data.Models
{
    public class OrderProduct
    {
        [Required]
        [Comment("Id of the order, PK")]
        public Guid OrderId { get; set; }


        [Required]
        [ForeignKey(nameof(OrderId))]
        [Comment("Order object")]
        public Order Order { get; set; } = null!;


        [Required]
        [Comment("Id of the product, PK")]
        public Guid ProductId { get; set; }


        [Required]
        [ForeignKey(nameof(ProductId))]
        [Comment("Product object")]
        public Product Product { get; set; } = null!;
    }
}
