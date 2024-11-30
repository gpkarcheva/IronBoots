using IronBoots.Data.Models;
using IronBoots.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore;

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
            if (!await context.Clients.ContainsAsync(client))
            {
                context.Clients.Add(client);
            }
        }
        await context.SaveChangesAsync();

        //seed materials

    }
}
