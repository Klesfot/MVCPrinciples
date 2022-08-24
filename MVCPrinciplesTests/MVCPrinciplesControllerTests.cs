using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.EntityFrameworkCore;
using MVCPrinciples.Controllers;
using MVCPrinciples.Data;
using MVCPrinciples.Models;
using Options = MVCPrinciples.Data.Options;

namespace MVCPrinciples.Tests
{
    public class MvcPrinciplesControllerTests
    {
        private Mock<CategoryContext> categoryContextMock;
        private Mock<ProductContext> productContextMock;
        private Mock<SupplierContext> supplierContextMock;

        [SetUp]
        public void Setup()
        {
            categoryContextMock = new Mock<CategoryContext>(new DbContextOptions<CategoryContext>());
            productContextMock = new Mock<ProductContext>(new DbContextOptions<ProductContext>());
            supplierContextMock = new Mock<SupplierContext>(new DbContextOptions<SupplierContext>());
        }

        [Test]
        public async Task CategoriesControllerIndex_Always_ReturnsView()
        {
            var categories = SetupCategories();
            categoryContextMock.Setup(c => c.Categories)
                .ReturnsDbSet(categories);
            var sut = new CategoriesController(categoryContextMock.Object);

            var result = await sut.Index();

            Assert.That(result, Is.TypeOf(typeof(ViewResult)));
        }

        [Test]
        public async Task ProductsControllerIndex_ProductDisplayQuantityNotConfigured_ReturnsListOfCategories()
        {
            var categories = SetupCategories();
            var products = SetupProducts();
            var suppliers = SetupSuppliers();
            var options = new Options{ProductsDisplayQuantity = 0};
            var optionsMock = new Mock<IOptions<Options>>();
            optionsMock.Setup(o => o.Value).Returns(options);
            categoryContextMock.Setup(c => c.Categories)
                .ReturnsDbSet(categories);
            productContextMock.Setup(p => p.Products)
                .ReturnsDbSet(products);
            supplierContextMock.Setup(s => s.Suppliers)
                .ReturnsDbSet(suppliers);
            var sut = new ProductsController(
                productContextMock.Object,
                categoryContextMock.Object,
                supplierContextMock.Object,
                optionsMock.Object);

            var result = await sut.Index();

            Assert.That(result, Is.TypeOf(typeof(ViewResult)));
        }

        [Test]
        public void ProductsControllerCreate_Always_ReturnsView()
        {
            var categories = SetupCategories();
            var products = SetupProducts();
            var suppliers = SetupSuppliers();
            var options = new Options { ProductsDisplayQuantity = 0 };
            var optionsMock = new Mock<IOptions<Options>>();
            optionsMock.Setup(o => o.Value).Returns(options);
            categoryContextMock.Setup(c => c.Categories)
                .ReturnsDbSet(categories);
            productContextMock.Setup(p => p.Products)
                .ReturnsDbSet(products);
            supplierContextMock.Setup(s => s.Suppliers)
                .ReturnsDbSet(suppliers);
            var sut = new ProductsController(
                productContextMock.Object,
                categoryContextMock.Object,
                supplierContextMock.Object,
                optionsMock.Object);

            var result = sut.Create();

            Assert.That(result, Is.TypeOf(typeof(ViewResult)));
        }

        [Test]
        public async Task ProductsControllerCreate_ModelStateIsValid_SaveChangesAsyncCalledOnProductContext()
        {
            var categories = SetupCategories();
            var products = SetupProducts();
            var suppliers = SetupSuppliers();
            
            var options = new Options { ProductsDisplayQuantity = 0 };
            var optionsMock = new Mock<IOptions<Options>>();
            optionsMock.Setup(o => o.Value).Returns(options);
            categoryContextMock.Setup(c => c.Categories)
                .ReturnsDbSet(categories);
            productContextMock.Setup(p => p.Products)
                .ReturnsDbSet(products);
            supplierContextMock.Setup(s => s.Suppliers)
                .ReturnsDbSet(suppliers);
            var sut = new ProductsController(
                productContextMock.Object,
                categoryContextMock.Object,
                supplierContextMock.Object,
                optionsMock.Object);

            await sut.Create(products[0]);

            productContextMock.Verify(c => c.
                SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task ProductsControllerEdit_ProvidedIdIsNull_SaveAsyncNeverCalled()
        {
            var categories = SetupCategories();
            var products = SetupProducts();
            var suppliers = SetupSuppliers();

            var options = new Options { ProductsDisplayQuantity = 0 };
            var optionsMock = new Mock<IOptions<Options>>();
            optionsMock.Setup(o => o.Value).Returns(options);
            categoryContextMock.Setup(c => c.Categories)
                .ReturnsDbSet(categories);
            productContextMock.Setup(p => p.Products)
                .ReturnsDbSet(products);
            supplierContextMock.Setup(s => s.Suppliers)
                .ReturnsDbSet(suppliers);
            var sut = new ProductsController(
                productContextMock.Object,
                categoryContextMock.Object,
                supplierContextMock.Object,
                optionsMock.Object);

            await sut.Edit(null);

            productContextMock.Verify(c => c.
                SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test]
        public async Task ProductsControllerEdit_ModelStateIsValid_SaveChangesAsyncCalledOnProductContext()
        {
            var categories = SetupCategories();
            var products = SetupProducts();
            var suppliers = SetupSuppliers();

            var options = new Options { ProductsDisplayQuantity = 0 };
            var optionsMock = new Mock<IOptions<Options>>();
            optionsMock.Setup(o => o.Value).Returns(options);
            categoryContextMock.Setup(c => c.Categories)
                .ReturnsDbSet(categories);
            productContextMock.Setup(p => p.Products)
                .ReturnsDbSet(products);
            supplierContextMock.Setup(s => s.Suppliers)
                .ReturnsDbSet(suppliers);
            var sut = new ProductsController(
                productContextMock.Object,
                categoryContextMock.Object,
                supplierContextMock.Object,
                optionsMock.Object);

            await sut.Edit(0, products[0]);

            productContextMock.Verify(c => c.
                SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void ConfigureControllerIndex_Always_ReturnsView()
        {
            var options = new Options { ProductsDisplayQuantity = 0, DbConnectionString = string.Empty };
            var optionsMock = new Mock<IOptions<Options>>();
            optionsMock.Setup(o => o.Value).Returns(options);
            var sut = new ConfigureController(optionsMock.Object);

            var result = sut.Index();

            Assert.That(result, Is.TypeOf(typeof(ViewResult)));
        }

        [Test]
        public void ConfigureControllerSave_Always_OptionsValuesChanged()
        {
            var updatedOptions = new Options { ProductsDisplayQuantity = 1, DbConnectionString = "string"};
            var optionsMock = new Mock<IOptions<Options>>();
            optionsMock.SetupSet(c => c.Value.ProductsDisplayQuantity = It.IsAny<int>()).Verifiable();
            optionsMock.SetupSet(c => c.Value.DbConnectionString = It.IsAny<string>()).Verifiable();
            var sut = new ConfigureController(optionsMock.Object);

            sut.Save(Convert.ToString(updatedOptions.ProductsDisplayQuantity), updatedOptions.DbConnectionString);
            
            optionsMock.VerifySet(c => c.Value.ProductsDisplayQuantity = It.IsAny<int>(), Times.Once);
            optionsMock.VerifySet(c => c.Value.DbConnectionString = It.IsAny<string>(), Times.Once);
        }

        [Test]
        public void HomeControllerIndex_Always_ReturnsView()
        {
            var loggerMock = new Mock<ILogger<HomeController>>();
            var sut = new HomeController(loggerMock.Object);

            var result = sut.Index();

            Assert.That(result, Is.TypeOf(typeof(ViewResult)));
        }

        [Test]
        public void HomeControllerError_Always_ReturnsView()
        {
            var loggerMock = new Mock<ILogger<HomeController>>();
            var sut = new HomeController(loggerMock.Object);

            var result = sut.Index();

            Assert.That(result, Is.TypeOf(typeof(ViewResult)));
        }

        private List<Category> SetupCategories()
        {
            var categories = new List<Category>();
            categories.Add(new Category
            {
                CategoryID = 0,
                CategoryName = string.Empty,
                Description = string.Empty
            });
            return categories;
        }

        private List<Product> SetupProducts()
        {
            var products = new List<Product>();
            products.Add(new Product
            {
                ProductID = 0,
                ProductName = string.Empty,
                SupplierId = 0,
                CategoryId = 0,
                QuantityPerUnit = string.Empty,
                UnitPrice = 0,
                UnitsInStock = 0,
                UnitsOnOrder = 0,
                ReorderLevel = 0,
                Discontinued = false
            });
            return products;
        }

        private List<Supplier> SetupSuppliers()
        {
            var suppliers = new List<Supplier>();
            suppliers.Add(new Supplier
            {
                SupplierID = 0,
                CompanyName = string.Empty
            });
            return suppliers;
        }
    }
}