using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data
{
    public class IronBootsDbContext : IdentityDbContext
    {
        public IronBootsDbContext(DbContextOptions<IronBootsDbContext> options)
            : base(options)
        {
        }
    }
}