using IronBoots.Controllers;
using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[TestFixture]
public class ProductTests
{
    private ApplicationDbContext _context;
    private ProductController _controller;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        _context = new ApplicationDbContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _context.Products.AddRange(
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                Price = 10.50m,
                IsDeleted = false,
                ImageUrl = "url1"
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product2",
                Price = 20.00m,
                IsDeleted = false,
                ImageUrl = "url2"
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "DeletedProduct",
                Price = 15.00m,
                IsDeleted = true,
                ImageUrl = "url3"
            }
        );

        _context.SaveChanges();
        _controller = new ProductController(_context);
    }


    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
        _controller?.Dispose();
    }

    [Test]
    public async Task Index_ReturnsViewWithActiveProducts()
    {
        var result = await _controller.Index();

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as List<ProductIndexViewModel>;
        Assert.IsNotNull(model);
        Assert.AreEqual(2, model.Count);
        Assert.IsTrue(model.Any(p => p.Name == "Product1"));
        Assert.IsTrue(model.Any(p => p.Name == "Product2"));
    }

    [Test]
    public async Task Details_ReturnsCorrectModel_WhenProductExists()
    {
        var productId = _context.Products.First(p => p.IsDeleted == false).Id;
        var result = await _controller.Details(productId);

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as ProductViewModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(productId, model.Id);
        Assert.AreEqual("Product1", model.Name);
        Assert.AreEqual("url1", model.ImageUrl);
    }

    [Test]
    public async Task Details_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var nonExistentId = Guid.NewGuid();
        var result = await _controller.Details(nonExistentId);
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Delete_SetsIsDeletedToTrueAndRedirects()
    {
        var productId = _context.Products.First(p => !p.IsDeleted).Id;

        var result = await _controller.Delete(productId);

        var product = _context.Products.Find(productId);
        Assert.NotNull(product);
        Assert.IsTrue(product.IsDeleted);

        var redirectResult = result as RedirectToActionResult;
        Assert.NotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);
    }

    [Test]
    public async Task Delete_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var nonExistentProductId = Guid.NewGuid();
        var result = await _controller.Delete(nonExistentProductId);

        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Add_Get_ReturnsViewWithModel()
    {
        _context.Materials.AddRange(
            new Material
            {
                Id = Guid.NewGuid(),
                Name = "Material1",
                IsDeleted = false,
                DistributorContact = "contact1@example.com"
            },
            new Material
            {
                Id = Guid.NewGuid(),
                Name = "Material2",
                IsDeleted = false,
                DistributorContact = "contact2@example.com"
            }
        );
        await _context.SaveChangesAsync();

        var result = await _controller.Add();

        var viewResult = result as ViewResult;
        Assert.NotNull(viewResult);

        var model = viewResult.Model as ProductViewModel;
        Assert.NotNull(model);
        Assert.NotNull(model.Materials);
        Assert.IsTrue(model.Materials.Count > 0);
    }

    [Test]
    public async Task Edit_Get_ReturnsViewWithModel_WhenProductExists()
    {
        var productId = _context.Products.First(p => !p.IsDeleted).Id;
        var result = await _controller.Edit(productId);

        var viewResult = result as ViewResult;
        Assert.NotNull(viewResult);

        var model = viewResult.Model as ProductViewModel;
        Assert.NotNull(model);
        Assert.AreEqual(productId, model.Id);
        Assert.AreEqual("Product1", model.Name);
    }

    [Test]
    public async Task Edit_Get_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var nonExistentProductId = Guid.NewGuid();
        var result = await _controller.Edit(nonExistentProductId);
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
    [Test]
    public async Task RemoveFromCart_RemovesProductAndRedirectsToCart()
    {
        var userId = Guid.NewGuid().ToString();
        var client = new Client
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            Name = "Test Client"
        };

        _context.Clients.Add(client);

        var productId = _context.Products.First(p => !p.IsDeleted).Id;
        _context.ClientsProducts.Add(new ClientProduct { ClientId = client.Id, ProductId = productId });
        await _context.SaveChangesAsync();

        var httpContext = new Mock<HttpContext>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));

        httpContext.Setup(x => x.User).Returns(user);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext.Object
        };

        var result = await _controller.RemoveFromCart(productId);

        var removedProduct = _context.ClientsProducts
            .FirstOrDefault(cp => cp.ClientId == client.Id && cp.ProductId == productId);
        Assert.Null(removedProduct);

        var redirectResult = result as RedirectToActionResult;
        Assert.NotNull(redirectResult);
        Assert.AreEqual("Cart", redirectResult.ActionName);
    }

    [Test]
    public async Task Cart_ReturnsViewWithUserCartProducts()
    {
        var userId = Guid.NewGuid().ToString();
        var client = new Client
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            Name = "Test Client"
        };

        _context.Clients.Add(client);

        var productId = _context.Products.First(p => !p.IsDeleted).Id;
        _context.ClientsProducts.Add(new ClientProduct { ClientId = client.Id, ProductId = productId });
        await _context.SaveChangesAsync();

        var httpContext = new Mock<HttpContext>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));

        httpContext.Setup(x => x.User).Returns(user);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext.Object
        };

        var result = await _controller.Cart();

        var viewResult = result as ViewResult;
        Assert.NotNull(viewResult);

        var model = viewResult.Model as List<ProductIndexViewModel>;
        Assert.NotNull(model);
        Assert.AreEqual(1, model.Count);
        Assert.IsTrue(model.Any(p => p.Id == productId));
    }

    [Test]
    public async Task AddToCart_AddsProductToUserCartAndRedirects()
    {
        var userId = Guid.NewGuid().ToString();
        var client = new Client
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            Name = "Test Client"
        };

        _context.Clients.Add(client);

        var productId = _context.Products.First(p => !p.IsDeleted).Id;
        await _context.SaveChangesAsync();

        var httpContext = new Mock<HttpContext>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));

        httpContext.Setup(x => x.User).Returns(user);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext.Object
        };

        var result = await _controller.AddToCart(productId);

        var clientProduct = _context.ClientsProducts
            .FirstOrDefault(cp => cp.ClientId == client.Id && cp.ProductId == productId);
        Assert.NotNull(clientProduct);

        var redirectResult = result as RedirectToActionResult;
        Assert.NotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);
    }

    [Test]
    public async Task Cart_ReturnsViewWithEmptyCart()
    {
        var userId = Guid.NewGuid().ToString();
        var client = new Client
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            Name = "Test Client"
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        var httpContext = new Mock<HttpContext>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));

        httpContext.Setup(x => x.User).Returns(user);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext.Object
        };

        var result = await _controller.Cart();

        var viewResult = result as ViewResult;
        Assert.NotNull(viewResult);

        var model = viewResult.Model as List<ProductIndexViewModel>;
        Assert.NotNull(model);
        Assert.AreEqual(0, model.Count);
    }

    [Test]
    public async Task AddToCart_ReturnsNotFound_ForInvalidProduct()
    {
        var userId = Guid.NewGuid().ToString();
        var client = new Client
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            Name = "Test Client"
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        var httpContext = new Mock<HttpContext>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));

        httpContext.Setup(x => x.User).Returns(user);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext.Object
        };

        var nonExistentProductId = Guid.NewGuid();

        var result = await _controller.AddToCart(nonExistentProductId);
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task AddToCart_DoesNotAddDuplicateProduct()
    {
        var userId = Guid.NewGuid().ToString();
        var client = new Client
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            Name = "Test Client"
        };

        _context.Clients.Add(client);

        var productId = _context.Products.First(p => !p.IsDeleted).Id;
        _context.ClientsProducts.Add(new ClientProduct { ClientId = client.Id, ProductId = productId });
        await _context.SaveChangesAsync();

        var httpContext = new Mock<HttpContext>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));

        httpContext.Setup(x => x.User).Returns(user);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext.Object
        };

        var result = await _controller.AddToCart(productId);

        var clientProducts = _context.ClientsProducts.Where(cp => cp.ClientId == client.Id && cp.ProductId == productId).ToList();
        Assert.AreEqual(1, clientProducts.Count);

        var redirectResult = result as RedirectToActionResult;
        Assert.NotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);
    }

    [Test]
    public async Task RemoveFromCart_ReturnsNotFound_WhenCartItemDoesNotExist()
    {
        var userId = Guid.NewGuid().ToString();
        var client = new Client
        {
            Id = Guid.NewGuid(),
            UserId = Guid.Parse(userId),
            Name = "Test Client"
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        var httpContext = new Mock<HttpContext>();
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
        new Claim(ClaimTypes.NameIdentifier, userId)
        }, "mock"));

        httpContext.Setup(x => x.User).Returns(user);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext.Object
        };

        var nonExistentProductId = Guid.NewGuid();

        var result = await _controller.RemoveFromCart(nonExistentProductId);
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
}