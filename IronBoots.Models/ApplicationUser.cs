using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace IronBoots.Data.Models
{

    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }
        [Required]
        [Comment("First name of the user")]
        public string FirstName { get; set; } = null!;

        [Required]
        [Comment("Last name of the user")]
        public string LastName { get; set; } = null!;
    }
}
