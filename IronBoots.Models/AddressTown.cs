using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IronBoots.Data.Models
{
    public class AddressTown
    {
        public AddressTown()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }


        [Required]
        [Comment("Id of the address")]
        public Guid AddressId { get; set; }


        [Required]
        [ForeignKey(nameof(AddressId))]
        [Comment("Address object")]
        public Address Address { get; set; } = null!;


        [Required]
        [Comment("Id of the Town")]
        public Guid TownId { get; set; }


        [Required]
        [ForeignKey(nameof(TownId))]
        [Comment("Town object")]
        public Town Town { get; set; } = null!;


        [Comment("Id of the client that has this combination")]
        public Guid ClientId { get; set; }


        [ForeignKey(nameof(ClientId))]
        [Comment("Client object")]
        public Client? Client { get; set; }


        [Required]
        public bool IsDeleted { get; set; }
    }
}
