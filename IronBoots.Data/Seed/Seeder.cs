using IronBoots.Data;
using IronBoots.Data.Seed;
using Microsoft.Extensions.DependencyInjection;

public static class Seeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        ////seed users and roles
        //UserRoleSeeder userRoleSeeder = new UserRoleSeeder();
        //await userRoleSeeder.SeedUserRolesAsync(serviceProvider, context);

        ////seed towns
        //TownSeeder townSeeder = new TownSeeder();
        //await townSeeder.SeedTownsAsync(context);
        //await context.SaveChangesAsync();

        ////seed addresses
        //AddressSeeder addressSeeder = new AddressSeeder();
        //await addressSeeder.SeedAddressAsync(context);
        //await context.SaveChangesAsync();

        ////seed Client
        //ClientSeeder clientSeeder = new ClientSeeder();
        //await clientSeeder.SeedClientsAsync(context);

        ////seed addressesTowns
        //AddressTownSeeder addressTownSeeder = new AddressTownSeeder();
        //await addressTownSeeder.SeedAddressTownAsync(context);

        ////seed materials
        //MaterialSeeder materialSeeder = new MaterialSeeder();
        //await materialSeeder.SeedMaterialsAsync(context);

        ////seed products
        //ProductSeeder productSeeder = new ProductSeeder();
        //await productSeeder.SeedProductsAsync(context);

        //seed productsMaterials
        ProductsMaterialsSeeder productsMaterialsSeeder = new ProductsMaterialsSeeder();
        await productsMaterialsSeeder.SeedProductsMaterialsAsync(context);

        //    //seed orders
        //    OrdersSeeder ordersSeeder = new OrdersSeeder();
        //    await ordersSeeder.SeedOrdersAsync(context);

        //    //seed orderProducts
        //    OrdersProductsSeeder orderProductSeeder = new OrdersProductsSeeder();
        //    await orderProductSeeder.SeedOrdersProductsAsync(context);

        //    //seed vehicles
        //    VehicleSeeder vehicleSeeder = new VehicleSeeder();
        //    await vehicleSeeder.SeedVehiclesAsync(context);

        //    //seed shipments
        //    ShipmentSeeder shipmentSeeder = new ShipmentSeeder();
        //    await shipmentSeeder.SeedShipmentsAsync(context);
    }
}
