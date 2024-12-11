using Moq;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using IronBoots.Controllers;
using IronBoots.Data;
using IronBoots.Data.Models;
using IronBoots.Models.Materials;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[TestFixture]
public class MaterialTests
{
    private ApplicationDbContext _context;
    private MaterialController _controller;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .EnableSensitiveDataLogging()
            .Options;

        _context = new ApplicationDbContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _context.Materials.AddRange(
            new Material
            {
                Id = Guid.NewGuid(),
                Name = "Material1",
                IsDeleted = false,
                PictureUrl = "url1",
                Price = 10.5m,
                DistributorContact = "contact1@example.com"
            },
            new Material
            {
                Id = Guid.NewGuid(),
                Name = "Material2",
                IsDeleted = false,
                PictureUrl = "url2",
                Price = 15.0m,
                DistributorContact = "contact2@example.com"
            },
            new Material
            {
                Id = Guid.NewGuid(),
                Name = "Material3",
                IsDeleted = true,
                PictureUrl = "url3",
                Price = 20.0m,
                DistributorContact = "contact3@example.com"
            }
        );
        _context.SaveChanges();

        _controller = new MaterialController(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context?.Dispose();
        _controller?.Dispose();
    }

    [Test]
    public async Task Index_ReturnsViewWithMaterials()
    {
        var result = await _controller.Index();

        var viewResult = result as ViewResult;
        Assert.NotNull(viewResult);
        var model = viewResult.Model as List<MaterialIndexViewModel>;
        Assert.NotNull(model);
        Assert.AreEqual(2, model.Count);
        Assert.IsTrue(model.Any(m => m.Name == "Material1"));
        Assert.IsTrue(model.Any(m => m.Name == "Material2"));
    }

    [Test]
    public async Task Details_ReturnsCorrectModel_WhenMaterialExists()
    {
        var materialId = _context.Materials.First(m => m.IsDeleted == false).Id;

        var result = await _controller.Details(materialId);

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as MaterialViewModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(materialId, model.Id);
        Assert.AreEqual("Material1", model.Name);
        Assert.AreEqual("url1", model.PictureUrl);
    }

    [Test]
    public async Task Delete_MaterialSetsIsDeletedToTrue()
    {
        var materialId = _context.Materials.First(m => m.IsDeleted == false).Id;
        var result = await _controller.Delete(materialId);

        var material = _context.Materials.Find(materialId);
        Assert.IsNotNull(material);
        Assert.IsTrue(material.IsDeleted);

        var redirectResult = result as RedirectToActionResult;
        Assert.IsNotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);
    }

    [Test]
    public async Task Add_Get_ReturnsViewWithModel()
    {
        _context.Products.AddRange(
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product1",
                IsDeleted = false
            },
            new Product
            {
                Id = Guid.NewGuid(),
                Name = "Product2",
                IsDeleted = false
            }
        );
        await _context.SaveChangesAsync();

        var result = await _controller.Add();

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as MaterialViewModel;
        Assert.IsNotNull(model);
        Assert.IsNotNull(model.Products);
        Assert.IsTrue(model.Products.Count > 0);
    }

    [Test]
    public async Task Edit_Get_ReturnsViewWithModel_WhenMaterialExists()
    {
        var materialId = _context.Materials.First(m => m.IsDeleted == false).Id;

        var result = await _controller.Edit(materialId);

        var viewResult = result as ViewResult;
        Assert.IsNotNull(viewResult);

        var model = viewResult.Model as MaterialViewModel;
        Assert.IsNotNull(model);
        Assert.AreEqual(materialId, model.Id);
        Assert.AreEqual("Material1", model.Name);
    }

    [Test]
    public async Task Edit_Get_ReturnsNotFound_WhenMaterialDoesNotExist()
    {
        var nonExistentMaterialId = Guid.NewGuid();

        var result = await _controller.Edit(nonExistentMaterialId);

        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Edit_Post_ReturnsViewWithModel_WhenModelStateIsInvalid()
    {
        var materialId = _context.Materials.First(m => m.IsDeleted == false).Id;

        var model = new MaterialViewModel
        {
            Id = materialId,
            Name = "",
            Price = "invalid-price",
            PictureUrl = "updated-url",
            DistributorContact = "updated-contact@example.com",
            SelectedProductIds = new List<Guid>()
        };

        _controller.ModelState.AddModelError("Name", "The Name field is required.");
        _controller.ModelState.AddModelError("Price", "The Price field must be a valid decimal.");
        var result = await _controller.Edit(materialId, model);

        var viewResult = result as ViewResult;
        Assert.NotNull(viewResult);
        Assert.AreEqual(model, viewResult.Model);
    }

    [Test]
    public async Task Delete_MaterialIsDeletedAndRedirects()
    {
        var materialId = _context.Materials.First(m => m.IsDeleted == false).Id;

        var result = await _controller.Delete(materialId);

        var material = _context.Materials.Find(materialId);
        Assert.NotNull(material);
        Assert.IsTrue(material.IsDeleted);

        var redirectResult = result as RedirectToActionResult;
        Assert.NotNull(redirectResult);
        Assert.AreEqual("Index", redirectResult.ActionName);
    }


    [Test]
    public async Task Delete_ReturnsNotFound_WhenMaterialDoesNotExist()
    {
        var nonExistentMaterialId = Guid.NewGuid();
        var result = await _controller.Delete(nonExistentMaterialId);
        Assert.IsInstanceOf<NotFoundResult>(result);
    }

    [Test]
    public async Task Details_ReturnsNotFound_WhenMaterialDoesNotExist()
    {
        var nonExistentMaterialId = Guid.NewGuid();
        var result = await _controller.Details(nonExistentMaterialId);
        Assert.IsInstanceOf<NotFoundResult>(result);
    }
}