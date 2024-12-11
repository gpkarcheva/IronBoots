namespace IronBoots.Data.Seed
{
    using IronBoots.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class OrdersProductsSeeder
    {
        public async Task SeedOrdersProductsAsync(ApplicationDbContext context)
        {
            var orders = context.Orders;
            var products = context.Products;
            var orderProducts = new List<OrderProduct>();
            var product = context.Products.First();
            foreach (var order in orders)
            {
                    Random random = new Random();
                    OrderProduct orderProduct = new OrderProduct()
                    {
                        OrderId = order.Id,
                        Order = order,
                        ProductId = product.Id,
                        Product = product,
                    };
                    orderProducts.Add(orderProduct);
            }
            foreach (var orderProduct in orderProducts)
            {
                var currentProduct = orderProduct.Product;
                var currentOrder = orderProduct.Order;
                if (await context.OrdersProducts.FirstOrDefaultAsync(op => op.ProductId == currentProduct.Id) != null
                    && await context.OrdersProducts.FirstOrDefaultAsync(po => po.OrderId == currentOrder.Id) != null)
                {
                    continue;
                }
                currentOrder.OrderProducts.Add(orderProduct);
                currentProduct.ProductOrders.Add(orderProduct);
                await context.OrdersProducts.AddAsync(orderProduct);
            }
            await context.SaveChangesAsync();
        }
    }
}
