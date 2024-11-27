using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Data.Models
{

    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;
    }
}
