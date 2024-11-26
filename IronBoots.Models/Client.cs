using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IronBoots.Data.Models
{
    public class Client
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required]
        public Guid AddressId { get; set; }
        [Required]
        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; } = null!;

    }
}
