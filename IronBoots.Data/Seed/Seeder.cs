using IronBoots.Data;
using IronBoots.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class Seeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        //seed users and roles
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await context.Database.EnsureCreatedAsync();

        var password = "PisnaMi123!";

        var roles = new[] { "Admin", "Manager", "Client" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = role, NormalizedName = role.ToUpper() });
            }
        }

        var clientUser = new ApplicationUser
        {
            UserName = "client1@abv.bg",
            Email = "client1@abv.bg",
            FirstName = "Client",
            LastName = "User",
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(clientUser, password);
        if (result.Succeeded)
        {
            await context.SaveChangesAsync();
            await userManager.AddToRoleAsync(clientUser, "Client");
        }

        var adminUser = new ApplicationUser
        {
            UserName = "admin@abv.bg",
            Email = "admin@abv.bg",
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true
        };

        result = await userManager.CreateAsync(adminUser, password);
        if (result.Succeeded)
        {
            await context.SaveChangesAsync();
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        var managerUser = new ApplicationUser
        {
            UserName = "manager@abv.bg",
            Email = "manager@abv.bg",
            FirstName = "Manager",
            LastName = "User",
            EmailConfirmed = true
        };
        result = await userManager.CreateAsync(managerUser, password);

        if (result.Succeeded)
        {
            await context.SaveChangesAsync();
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        await context.SaveChangesAsync();

        //seed towns
        List<Town> towns = new List<Town>()
        {
            new Town
            {
                Name = "town1"
            },

            new Town
            {
                Name = "town2"
            }
        };
        foreach (var town in towns)
        {
            if (context.Towns.Any(t => t.Name == town.Name))
            {
                continue;
            }
            await context.Towns.AddAsync(town);
        }
        await context.SaveChangesAsync();

        //seed addresses
        List<Address> addresses = new List<Address>()
        {
            new Address
            {
                AddressText = "addresline 123"
            },
            new Address
            {
                AddressText = "addressline 456"
            }
        };
        foreach (var address in addresses)
        {
            if (context.Addresses.Any(a => a.AddressText == address.AddressText))
            {
                continue;
            }
            await context.Addresses.AddAsync(address);
        }
        await context.SaveChangesAsync();

        //seed addressesTowns
        var currentA = context.Addresses.ToList();
        var currentT = context.Towns.ToList();
        var aT = new List<AddressTown>();
        foreach (var address in currentA)
        {
            foreach (var town in currentT)
            {
                aT.Add(new AddressTown
                {
                    TownId = town.Id,
                    Town = town,
                    AddressId = address.Id,
                    Address = address,
                    IsDeleted = false
                });
            }
        }
        foreach (var at in aT)
        {
            if (context.AddressesTowns.Any(addT => addT.TownId == at.TownId)
                && context.AddressesTowns.Any(addT => addT.AddressId == at.AddressId))
            {
                continue;
            }
            await context.AddressesTowns.AddAsync(at);
        }
        await context.SaveChangesAsync();

        //seed Client
        var addressTownId = context.AddressesTowns.First().Id;
        var addressTown = await context.AddressesTowns
            .Where(a => a.Id == addressTownId)
            .FirstOrDefaultAsync();

        var correctUser = await context.Users
            .Where(u => u.Id == Guid.Parse("9CD2BA8E-BE18-41DD-BB95-7C3477EDFDDC"))
            .FirstOrDefaultAsync();
        if (addressTown != null)
        {
            Client client = new Client()
            {
                Name = "client",
                AddressTownId = addressTownId,
                AddressTown = addressTown,
                UserId = Guid.Parse("9CD2BA8E-BE18-41DD-BB95-7C3477EDFDDC"),
                User = correctUser,
                IsDeleted = false
            };
            if (await context.Clients.FirstOrDefaultAsync(c => c.UserId == client.UserId) == null!)
            {
                context.Clients.Add(client);
            }
        }
        await context.SaveChangesAsync();

        //seed materials
        Material material = new Material()
        {
            Name = "material1",
            Price = 123.5m,
            DistrubutorContact = "distr.com/contact",
            IsDeleted = false
        };

        if (await context.Materials.FirstOrDefaultAsync(mt => mt.Name == material.Name) == null)
        {
            await context.Materials.AddAsync(material);
            await context.SaveChangesAsync();
        }

        //seed products
        Product product = new Product()
        {
            Name = "product",
            Weight = 1.23,
            Size = 1.56,
            ProductionCost = 6.78m,
            ProductionTime = TimeSpan.Parse("0.02:54:18"),
            IsDeleted = false
        };

        if (await context.Products.FirstOrDefaultAsync(p => p.Name == product.Name) == null)
        {
            await context.Products.AddAsync(product);
        }

        //seed productsMaterials
        var currentProducts = await context.Products.ToListAsync();
        var currentMaterials = await context.Materials.ToListAsync();
        var productsMaterials = new List<ProductMaterial>();
        foreach (var item in currentProducts)
        {
            foreach (var materialItem in currentMaterials)
            {
                Random random = new Random();
                int quantity = random.Next(0, 10);
                productsMaterials.Add(new ProductMaterial()
                {
                    ProductId = item.Id,
                    Product = item,
                    MaterialId = materialItem.Id,
                    Material = materialItem,
                    MaterialQuantity = quantity
                });
            }
        }
        foreach (var pm in productsMaterials)
        {
            if (await context.ProductsMaterials
                .FirstOrDefaultAsync(prodM => prodM.ProductId == pm.ProductId) == null
                && await context.ProductsMaterials
                .FirstOrDefaultAsync(prodM => prodM.MaterialId == pm.MaterialId) == null)
            {
                await context.ProductsMaterials.AddAsync(pm);
            }
        }
        await context.SaveChangesAsync();

        //seed vehicles
        Vehicle currentVehicle = new()
        {
            Name = "vehicle",
            WeightCapacity = 123.5,
            SizeCapacity = 100.05,
            IsDeleted = false
        };
        if (await context.Vehicles.FirstOrDefaultAsync(v => v.Name == currentVehicle.Name) == null)
        {
            await context.Vehicles.AddAsync(currentVehicle);
            await context.SaveChangesAsync();
        }

        //seed orders
        var currentOrder = new Order()
        {
            ClientId = Guid.Parse("0A511CD3-889D-484D-8428-060FF4CA3850"),
            Client = await context.Clients.FirstAsync(),
            PlannedAssignedDate = DateTime.UtcNow,
            TotalPrice = 123.45m,
            IsActive = true
        };
        if (await context.Orders.FirstOrDefaultAsync(o => o.ClientId == currentOrder.ClientId) == null)
        {
            await context.Orders.AddAsync(currentOrder);
            await context.SaveChangesAsync();
        }

        //seed shipments
        var currentShipment = new Shipment()
        {
            VehicleId = Guid.Parse("EA7B32B8-F776-4E39-ACC7-2977899670BD"),
            Vehicle = currentVehicle,
            Orders = new List<Order>()
            {
                currentOrder
            },
            ShipmentStatus = Shipment.Status.PendingShipment
        };
        if (await context.Shipments.FirstOrDefaultAsync(s => s.VehicleId == currentVehicle.Id) == null)
        {
            await context.Shipments.AddAsync(currentShipment);
            await context.SaveChangesAsync();
        }
    }
}
