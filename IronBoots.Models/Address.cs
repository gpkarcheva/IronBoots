using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace IronBoots.Data.Models
{
    public class Address
    {
        [Key]
        [Comment("Identifier - GUID")]
        public Guid Id { get; set; }

        [Comment("Address details")]
        [Required]
        [MinLength(3)]
        [MaxLength(80)]
        public string AddressText { get; set; } = null!;

        [Required]
        [Comment("Town Id for easy tracking of orders/shipments")]
        public Guid TownId { get; set; }

        [Required]
        [Comment("All towns that contain the address")]
        public ICollection<AddressTown> AddressesTowns { get; set; } = null!;

        [Required]
        [Comment("Reference to the client the address belongs to")]
        public Guid ClientId { get; set; }

        [Required]
        [ForeignKey(nameof(ClientId))]
        [Comment("Client that has the address")]
        public Client Client { get; set; } = null!;

        [Required]
        [Comment("All orders for this address")]
        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
