using IronBoots.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace IronBoots.Data.Seed
{
    public class UserRoleSeeder
    {
        public async Task SeedUserRolesAsync(IServiceProvider serviceProvider, ApplicationDbContext context)
        {
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
                Id = Guid.Parse("9CD2BA8E-BE18-41DD-BB95-7C3477EDFDDC"),
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
                await userManager.AddToRoleAsync(adminUser, "Manager");
            }

            await context.SaveChangesAsync();
        }
    }
}
