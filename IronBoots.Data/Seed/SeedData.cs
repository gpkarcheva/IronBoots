using IronBoots.Data.Models;

namespace IronBoots.Data.Seed
{
    public class SeedData
    {
        public List<ApplicationUser> Users { get; set; } = new();
        public List<Town> Towns { get; set; } = new();
        public List<Address> Addresses { get; set; } = new();
        public List<Client> Clients { get; set; } = new();
        public List<Material> Materials { get; set; } = new();
        public List<Product> Products { get; set; } = new();
        public List<ProductMaterial> ProductMaterials { get; set; } = new();
        public List<OrderProduct> OrderProducts { get; set; } = new();
        public List<Order> Orders { get; set; } = new();
        public List<Shipment> Shipments { get; set; } = new();
        public List<Vehicle> Vehicles { get; set; } = new();
        public List<AddressTown> AddressesTowns { get; set; } = new();
    }
}
