using IronBoots.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IronBoots.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Address> Adresses { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<OrderProduct> OrdersProducts { get; set; }
        public DbSet<ProductMaterial> ProductsMaterials { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderProduct>()
                .HasKey(op => new
                {
                    op.OrderId,
                    op.ProductId,
                });
            builder.Entity<ProductMaterial>()
                .HasKey(pm => new
                {
                    pm.ProductId,
                    pm.MaterialId
                });
        }
    }
}