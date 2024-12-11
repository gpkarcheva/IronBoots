using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Data.Models
{
    public class ClientProduct
    {
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; } = null!;

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        [ForeignKey(nameof(ClientId))]
        public Product Product { get; set; } = null!;
    }
}
