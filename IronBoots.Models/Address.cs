using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static IronBoots.Common.EntityValidationConstants.AddressValidation;

namespace IronBoots.Data.Models
{
    public class Address
    {
        public Address()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Comment("Identifier - GUID")]
        public Guid Id { get; set; }


        [Comment("Address details")]
        [Required]
        [MinLength(AddressMin)]
        [MaxLength(AddressMax)]
        public string AddressText { get; set; } = null!;


        [Required]
        [Comment("All towns that contain the address")]
        public IList<AddressTown> AddressesTowns { get; set; } = new List<AddressTown>();
    }
}
