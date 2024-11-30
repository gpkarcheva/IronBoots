using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static IronBoots.Common.EntityValidationConstants.TownValidation;

namespace IronBoots.Data.Models
{
    public class Town
    {
        public Town()
        {
            Id = Guid.NewGuid();
        }

        [Key]
        [Comment("Identifier")]
        public Guid Id { get; set; }


        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        [Comment("Town name")]
        public string Name { get; set; } = null!;


        [Required]
        [Comment("Collection of addresses for each town")]
        public IList<AddressTown> TownsAddresses { get; set; } = new List<AddressTown>();
    }
}
