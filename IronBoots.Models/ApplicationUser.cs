using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using static IronBoots.Common.EntityValidationConstants.ApplicationUserValidation;

namespace IronBoots.Data.Models
{

    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }

        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        [PersonalData]
        [Comment("First name of the user")]
        public string FirstName { get; set; } = null!;


        [Required]
        [MinLength(NameMin)]
        [MaxLength(NameMax)]
        [PersonalData]
        [Comment("Last name of the user")]
        public string LastName { get; set; } = null!;
    }
}
