using System.Text.Json;

namespace IronBoots.Data.Seed
{
    public class Seeder
    {
        private readonly ApplicationDbContext context;

        public Seeder(ApplicationDbContext _context)
        {
            context = _context;
        }

        public void Seed()
        {
            if (context.Towns.Any() ||
                context.Clients.Any() ||
                context.Addresses.Any() ||
                context.Materials.Any() ||
                context.Products.Any() ||
                context.ProductsMaterials.Any() ||
                context.Orders.Any() ||
                context.OrdersProducts.Any() ||
                context.Shipments.Any() ||
                context.Vehicles.Any())
            {
                Console.WriteLine("Database already contains data. Seeding skipped.");
                return;
            }

            //ToDo - add it to secrets.json
            string seedPath = "../IronBoots.Data/Seed/seed_data.json";
            var jsonData = File.ReadAllText(seedPath);
            var seedData = JsonSerializer.Deserialize<SeedData>(jsonData);

            if (seedData != null)
            {
                context.Users.AddRange(seedData.Users);
                context.Towns.AddRange(seedData.Towns);
                context.Addresses.AddRange(seedData.Addresses);
                context.AddressesTowns.AddRange(seedData.AddressesTowns);
                context.Clients.AddRange(seedData.Clients);
                context.Materials.AddRange(seedData.Materials);
                context.Products.AddRange(seedData.Products);
                context.ProductsMaterials.AddRange(seedData.ProductMaterials);
                context.Orders.AddRange(seedData.Orders);
                context.OrdersProducts.AddRange(seedData.OrderProducts);
                context.Shipments.AddRange(seedData.Shipments);
                context.Vehicles.AddRange(seedData.Vehicles);

                context.SaveChanges();
                Console.WriteLine("Database seeding completed successfully!");
            }
            else
            {
                Console.WriteLine("Failed to deserialize seed data.");
            }
        }
    }
}
