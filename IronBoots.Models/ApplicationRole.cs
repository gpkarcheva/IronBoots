using Microsoft.AspNetCore.Identity;

namespace IronBoots.Data
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole()
        {
            Id = Guid.NewGuid();
        }
    }
}