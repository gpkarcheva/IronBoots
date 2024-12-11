using IronBoots.Controllers;
using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Vehicles;
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
public class VehicleControllerTests
{
    private ApplicationDbContext _context;
    private VehicleController _controller;

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
            new Claim(ClaimTypes.Role, "Admin")
        }, "mock"));

        mockHttpContext.Setup(x => x.User).Returns(mockUser);

        _controller = new VehicleController(_context)
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
    public async Task Index_ReturnsViewWithAllVehicles()
    {
        _context.Vehicles.AddRange(
            new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle1", IsDeleted = false, IsAvailable = true },
            new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle2", IsDeleted = false, IsAvailable = false },
            new Vehicle { Id = Guid.NewGuid(), Name = "DeletedVehicle", IsDeleted = true, IsAvailable = false }
        );
        await _context.SaveChangesAsync();

        var result = await _controller.Index();

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as List<VehicleIndexViewModel>;
        Assert.IsNotNull(model);
        Assert.AreEqual(2, model.Count);
        Assert.IsTrue(model.Any(v => v.Name == "Vehicle1"));
        Assert.IsTrue(model.Any(v => v.Name == "Vehicle2"));
    }

    [Test]
    public async Task Details_ReturnsNotFound_WhenVehicleDoesNotExist()
    {
        var result = await _controller.Details(Guid.NewGuid());
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Add_Post_AddsVehicleAndRedirects()
    {
        var model = new VehicleViewModel
        {
            Name = "New Vehicle",
            WeightCapacity = 150,
            SizeCapacity = 300
        };

        var result = await _controller.Add(model);

        var addedVehicle = _context.Vehicles.FirstOrDefault(v => v.Name == "New Vehicle");
        Assert.IsNotNull(addedVehicle);
        Assert.AreEqual(150, addedVehicle.WeightCapacity);
        Assert.AreEqual(300, addedVehicle.SizeCapacity);

        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);
    }

    [Test]
    public async Task Delete_MarksVehicleAsDeletedAndRedirects()
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Name = "Vehicle1",
            IsDeleted = false,
            IsAvailable = true
        };
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        var result = await _controller.Delete(vehicle.Id);

        var deletedVehicle = _context.Vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
        Assert.IsNotNull(deletedVehicle);
        Assert.IsTrue(deletedVehicle.IsDeleted);

        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);
    }

    [Test]
    public async Task Edit_Post_UpdatesVehicleAndRedirects()
    {
        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            Name = "Vehicle1",
            WeightCapacity = 100,
            SizeCapacity = 200
        };
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        var model = new VehicleViewModel
        {
            Id = vehicle.Id,
            Name = "Updated Vehicle",
            WeightCapacity = 150,
            SizeCapacity = 250
        };

        var result = await _controller.Edit(model);

        var updatedVehicle = _context.Vehicles.FirstOrDefault(v => v.Id == vehicle.Id);
        Assert.IsNotNull(updatedVehicle);
        Assert.AreEqual("Updated Vehicle", updatedVehicle.Name);
        Assert.AreEqual(150, updatedVehicle.WeightCapacity);
        Assert.AreEqual(250, updatedVehicle.SizeCapacity);

        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual("Details", redirectResult.ActionName);
    }
}