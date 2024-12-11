using IronBoots.Common;
using IronBoots.Controllers;
using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Shipments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IronBoots.Common.Status;

[TestFixture]
public class ShipmentControllerTests
{
    private ApplicationDbContext _context;
    private ShipmentController _controller;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new ApplicationDbContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _controller = new ShipmentController(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
        _controller?.Dispose();
    }

    [Test]
    public async Task Index_ReturnsAllShipments_NotDelivered()
    {
        _context.Shipments.AddRange(
            new Shipment { Id = Guid.NewGuid(), ShipmentStatus = Status.InTransit, Vehicle = new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle 1" } },
            new Shipment { Id = Guid.NewGuid(), ShipmentStatus = Status.PendingShipment, Vehicle = new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle 2" } },
            new Shipment { Id = Guid.NewGuid(), ShipmentStatus = Status.Delivered, Vehicle = new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle 3" } }
        );
        await _context.SaveChangesAsync();

        var result = await _controller.Index();

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as List<ShipmentIndexViewModel>;
        Assert.IsNotNull(model);
        Assert.AreEqual(2, model.Count);
    }

    [Test]
    public async Task Details_ReturnsShipmentDetails_WhenExists()
    {
        var vehicle = new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle 1" };
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            Vehicle = vehicle,
            ShipmentStatus = Status.PendingShipment
        };
        _context.Vehicles.Add(vehicle);
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        var result = await _controller.Details(shipment.Id);

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as ShipmentViewModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(shipment.Id, model.Id);
        Assert.AreEqual("Vehicle 1", model.Vehicle?.Name);
    }

    [Test]
    public async Task Edit_Get_ReturnsShipmentEditView_WhenExists()
    {
        var vehicle = new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle 1" };
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            Vehicle = vehicle,
            ShipmentStatus = Status.PendingShipment,
            Orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), Address = "Address 1" },
                new Order { Id = Guid.NewGuid(), Address = "Address 2" }
            }
        };
        _context.Vehicles.Add(vehicle);
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        var result = await _controller.Edit(shipment.Id);

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as ShipmentViewModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(shipment.Id, model.Id);
        Assert.AreEqual("Vehicle 1", model.Vehicle?.Name);
    }

    [Test]
    public async Task Edit_Get_ReturnsNotFound_WhenShipmentDoesNotExist()
    {
        var result = await _controller.Edit(Guid.NewGuid());
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Edit_Post_UpdatesShipmentAndRedirects()
    {
        var vehicle1 = new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle 1", IsAvailable = true };
        var vehicle2 = new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle 2", IsAvailable = true };
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            Vehicle = vehicle1,
            ShipmentStatus = Status.PendingShipment,
            ShipmentDate = DateTime.Now.AddDays(-2),
            Orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), Address = "Address 1" },
                new Order { Id = Guid.NewGuid(), Address = "Address 2" }
            }
        };

        _context.Vehicles.AddRange(vehicle1, vehicle2);
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        var model = new ShipmentViewModel
        {
            Id = shipment.Id,
            VehicleId = vehicle2.Id,
            ShipmentDate = DateTime.Now.ToString("yyyy-MM-dd"),
            ShipmentStatus = Status.InTransit,
            SelectedOrdersIds = new List<Guid> { shipment.Orders.First().Id }
        };

        var result = await _controller.Edit(model);

        var updatedShipment = _context.Shipments.Include(s => s.Vehicle).FirstOrDefault(s => s.Id == shipment.Id);
        Assert.IsNotNull(updatedShipment);
        Assert.AreEqual(vehicle2.Id, updatedShipment.VehicleId);
        Assert.AreEqual(Status.InTransit, updatedShipment.ShipmentStatus);

        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual("Details", redirectResult.ActionName);
    }

    [Test]
    public async Task Edit_Post_ReturnsView_WhenModelStateIsInvalid()
    {
        var shipment = new Shipment { Id = Guid.NewGuid(), Vehicle = new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle 1" } };
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        var model = new ShipmentViewModel
        {
            Id = shipment.Id,
            VehicleId = shipment.VehicleId,
            ShipmentDate = "Invalid Date"
        };

        _controller.ModelState.AddModelError("ShipmentDate", "Invalid date format");

        var result = await _controller.Edit(model);

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);
        Assert.AreEqual(model, viewResult.Model);
    }

    [Test]
    public async Task Index_ReturnsEmptyList_WhenNoShipmentsExist()
    {
        var result = await _controller.Index();

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as List<ShipmentIndexViewModel>;
        Assert.IsNotNull(model);
        Assert.AreEqual(0, model.Count);
    }

    [Test]
    public async Task Details_ReturnsShipmentWithMultipleOrders_WhenExists()
    {
        var vehicle = new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle 1" };
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            Vehicle = vehicle,
            ShipmentStatus = Status.PendingShipment
        };
        var order1 = new Order { Id = Guid.NewGuid(), Address = "Address 1", ShipmentId = shipment.Id };
        var order2 = new Order { Id = Guid.NewGuid(), Address = "Address 2", ShipmentId = shipment.Id };
        _context.Vehicles.Add(vehicle);
        _context.Shipments.Add(shipment);
        _context.Orders.AddRange(order1, order2);
        await _context.SaveChangesAsync();

        var result = await _controller.Details(shipment.Id);

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as ShipmentViewModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(2, model.AllOrders.Count);
    }

    [Test]
    public async Task Edit_Post_ReturnsView_WhenInvalidDateFormatIsGiven()
    {
        var vehicle = new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle 1" };
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            Vehicle = vehicle,
            ShipmentStatus = Status.PendingShipment
        };
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        var model = new ShipmentViewModel
        {
            Id = shipment.Id,
            VehicleId = shipment.VehicleId,
            ShipmentDate = "Invalid Date"
        };

        _controller.ModelState.AddModelError("ShipmentDate", "Invalid date format");

        var result = await _controller.Edit(model);

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);
        Assert.AreEqual(model, viewResult.Model);
    }

    [Test]
    public async Task Edit_Post_ChangesShipmentStatus_WhenShipmentDateIsToday()
    {
        var vehicle = new Vehicle { Id = Guid.NewGuid(), Name = "Vehicle 1", IsAvailable = true };
        var shipment = new Shipment
        {
            Id = Guid.NewGuid(),
            Vehicle = vehicle,
            ShipmentStatus = Status.PendingShipment,
            ShipmentDate = DateTime.Now,
            Orders = new List<Order> { new Order { Id = Guid.NewGuid(), Address = "Address 1" } }
        };

        _context.Vehicles.Add(vehicle);
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();

        var model = new ShipmentViewModel
        {
            Id = shipment.Id,
            VehicleId = vehicle.Id,
            ShipmentDate = DateTime.Now.ToString("yyyy-MM-dd"),
            ShipmentStatus = Status.InTransit,
            SelectedOrdersIds = new List<Guid> { shipment.Orders.First().Id }
        };

        var result = await _controller.Edit(model);

        var updatedShipment = await _context.Shipments.Include(s => s.Vehicle).FirstOrDefaultAsync(s => s.Id == shipment.Id);
        Assert.IsNotNull(updatedShipment);
        Assert.AreEqual(Status.InTransit, updatedShipment.ShipmentStatus);
        Assert.IsFalse(updatedShipment.Vehicle.IsAvailable);
    }

    [Test]
    public async Task Edit_Post_ReturnsNotFound_WhenShipmentDoesNotExist()
    {
        var model = new ShipmentViewModel
        {
            Id = Guid.NewGuid(),
            VehicleId = Guid.NewGuid(),
            ShipmentDate = DateTime.Now.ToString("yyyy-MM-dd"),
            ShipmentStatus = Status.PendingShipment
        };

        var result = await _controller.Edit(model);

        Assert.IsInstanceOf<NotFoundResult>(result);
    }
}