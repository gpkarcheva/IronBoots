using IronBoots.Controllers;
using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Orders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[TestFixture]
public class OrderTests
{
    private ApplicationDbContext _context;
    private OrderController _controller;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new ApplicationDbContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        var mockHttpContext = new Mock<HttpContext>();
        var userId = Guid.NewGuid();
        var mockUser = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        }, "mock"));

        mockHttpContext.Setup(x => x.User).Returns(mockUser);

        _controller = new OrderController(_context)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            }
        };
    }

    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
        _controller?.Dispose();
    }

    [Test]
    public async Task Index_ReturnsClientOrders_WhenUserIsClient()
    {
        var userId = Guid.Parse(_controller.GetCurrentUserId() ?? Guid.Empty.ToString());
        var client = new Client { Id = Guid.NewGuid(), UserId = userId, Name = "Test Client" };
        _context.Clients.Add(client);

        _context.Orders.Add(new Order
        {
            Id = Guid.NewGuid(),
            ClientId = client.Id,
            Client = client,
            TotalPrice = 100m,
            Address = "Test Address",
            IsActive = true,
            AssignedDate = DateTime.Now
        });

        await _context.SaveChangesAsync();

        var result = await _controller.Index();

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as List<OrderIndexViewModel>;
        Assert.IsNotNull(model);
        Assert.AreEqual(1, model.Count);
        Assert.AreEqual(100m.ToString("F2"), model.First().TotalPrice);
    }

    [Test]
    public async Task Cancel_MarksOrderAsInactiveAndRedirects()
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            Address = "Test Address",
            TotalPrice = 100m,
            IsActive = true
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var result = await _controller.Cancel(order.Id);

        var canceledOrder = _context.Orders.FirstOrDefault(o => o.Id == order.Id);
        Assert.IsNotNull(canceledOrder);
        Assert.IsFalse(canceledOrder.IsActive);
        Assert.IsNull(canceledOrder.AssignedDate);

        var redirectResult = result as RedirectToActionResult;
        Assert.NotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);
    }

    [Test]
    public async Task Checkout_Get_ReturnsCheckoutViewWithTotalPrice()
    {
        var userId = Guid.Parse(_controller.GetCurrentUserId() ?? Guid.Empty.ToString());
        var client = new Client { Id = Guid.NewGuid(), UserId = userId, Name = "Test Client" };
        _context.Clients.Add(client);

        var product1 = new Product { Id = Guid.NewGuid(), Name = "Product1", Price = 50m, IsDeleted = false };
        var product2 = new Product { Id = Guid.NewGuid(), Name = "Product2", Price = 75m, IsDeleted = false };

        _context.Products.AddRange(product1, product2);

        _context.ClientsProducts.AddRange(
            new ClientProduct { ClientId = client.Id, ProductId = product1.Id },
            new ClientProduct { ClientId = client.Id, ProductId = product2.Id }
        );

        await _context.SaveChangesAsync();

        var result = await _controller.Checkout();

        var viewResult = result as ViewResult;
        Assert.NotNull(viewResult);

        var model = viewResult.Model as OrderViewModel;
        Assert.NotNull(model);
        Assert.AreEqual("125", model.TotalPrice.ToString(CultureInfo.InvariantCulture));
    }

    [Test]
    public async Task Details_ReturnsOrderDetails_WhenAuthorized()
    {
        var userId = Guid.NewGuid().ToString();
        var client = new Client { Id = Guid.NewGuid(), UserId = Guid.Parse(userId), Name = "Client1" };
        _context.Clients.Add(client);
        var order = new Order
        {
            Id = Guid.NewGuid(),
            ClientId = client.Id,
            IsActive = true,
            TotalPrice = 50,
            Address = "Test Address"
        };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var httpContext = new Mock<HttpContext>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));
        httpContext.Setup(x => x.User).Returns(user);
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext.Object };

        var result = await _controller.Details(order.Id);

        var viewResult = result as ViewResult;
        Assert.NotNull(viewResult);

        var model = viewResult.Model as OrderViewModel;
        Assert.NotNull(model);
        Assert.AreEqual(order.Id, model.Id);
        Assert.AreEqual(order.TotalPrice.ToString("F2"), model.TotalPrice);
        Assert.AreEqual("Test Address", model.Address);
    }

    [Test]
    public async Task Details_ReturnsNotFound_WhenUnauthorized()
    {
        var client = new Client { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Name = "Client1" };
        _context.Clients.Add(client);
        var order = new Order
        {
            Id = Guid.NewGuid(),
            ClientId = client.Id,
            IsActive = true,
            TotalPrice = 50,
            Address = "Test Address"
        };
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        var httpContext = new Mock<HttpContext>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        }, "mock"));
        httpContext.Setup(x => x.User).Returns(user);
        _controller.ControllerContext = new ControllerContext { HttpContext = httpContext.Object };

        var result = await _controller.Details(order.Id);

        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Cancel_ReturnsNotFound_WhenOrderDoesNotExist()
    {
        var nonExistentOrderId = Guid.NewGuid();
        var result = await _controller.Cancel(nonExistentOrderId);
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
}