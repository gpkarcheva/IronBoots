using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronBoots.Data.Models
{
    public class Town
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MinLength(3)]
        [MaxLength(60)]
        public string Name { get; set; } = null!;
    }
}
