using Microsoft.EntityFrameworkCore;
using ProductConsoleAPI.Business;
using ProductConsoleAPI.Business.Contracts;
using ProductConsoleAPI.Data.Models;
using ProductConsoleAPI.DataAccess;
using System.ComponentModel.DataAnnotations;

namespace ProductConsoleAPI.IntegrationTests.NUnit
{
    public  class IntegrationTests
    {
        private TestProductsDbContext dbContext;
        private IProductsManager productsManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestProductsDbContext();
            this.productsManager = new ProductsManager(new ProductsRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //positive test
        [Test]
        public async Task AddProductAsync_ShouldAddNewProduct()
        {
            var newProduct = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProduct",
                ProductCode = "AB12C",
                Price = 1.25m,
                Quantity = 100,
                Description = "Anything for description"
            };

            await productsManager.AddAsync(newProduct);

            var dbProduct = await this.dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == newProduct.ProductCode);

            Assert.NotNull(dbProduct);
            Assert.AreEqual(newProduct.ProductName, dbProduct.ProductName);
            Assert.AreEqual(newProduct.Description, dbProduct.Description);
            Assert.AreEqual(newProduct.Price, dbProduct.Price);
            Assert.AreEqual(newProduct.Quantity, dbProduct.Quantity);
            Assert.AreEqual(newProduct.OriginCountry, dbProduct.OriginCountry);
            Assert.AreEqual(newProduct.ProductCode, dbProduct.ProductCode);
        }

        //Negative test
        [Test]
        public async Task AddProductAsync_TryToAddProductWithInvalidCredentials_ShouldThrowException()
        {
            var newProduct = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProduct",
                ProductCode = "AB12C",
                Price = -1m,
                Quantity = 100,
                Description = "Anything for description"
            };

            var ex = Assert.ThrowsAsync<ValidationException>(async () => await productsManager.AddAsync(newProduct));
            var actual = await dbContext.Products.FirstOrDefaultAsync(c => c.ProductCode == newProduct.ProductCode);

            Assert.IsNull(actual);
            Assert.That(ex?.Message, Is.EqualTo("Invalid product!"));

        }

        [Test]
        public async Task DeleteProductAsync_WithValidProductCode_ShouldRemoveProductFromDb()
        {
            var newProduct = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProduct",
                ProductCode = "AB12C",
                Price = 1.25m,
                Quantity = 100,
                Description = "Anything for description"
            };

            await productsManager.AddAsync(newProduct);

            // Act
            await productsManager.DeleteAsync(newProduct.ProductCode);

            // Assert
            var dbProduct = await this.dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == newProduct.ProductCode);
            Assert.IsNull(dbProduct);
        }

        [Test]
        public async Task DeleteProductAsync_TryToDeleteWithNullOrWhiteSpaceProductCode_ShouldThrowException()
        {
            // Arrange
            string productCode = "";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await productsManager.DeleteAsync(productCode));
            Assert.That(ex?.Message, Is.EqualTo("Product code cannot be empty."));
        }

        [Test]
        public async Task GetAllAsync_WhenProductsExist_ShouldReturnAllProducts()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product { OriginCountry = "Bulgaria", ProductName = "TestProduct1", ProductCode = "AB12C", Price = 1.25m, Quantity = 100, Description = "Description 1" },
                new Product { OriginCountry = "USA", ProductName = "TestProduct2", ProductCode = "CD34E", Price = 2.50m, Quantity = 200, Description = "Description 2" }
            };
            await this.dbContext.Products.AddRangeAsync(productList);
            await this.dbContext.SaveChangesAsync();

            // Act
            var products = await productsManager.GetAllAsync();

            // Assert
            Assert.AreEqual(productList.Count, products.Count());
            foreach (var product in productList)
            {
                Assert.IsTrue(products.Any(p => p.ProductCode == product.ProductCode));
            }
        }

        [Test]
        public async Task GetAllAsync_WhenNoProductsExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await productsManager.GetAllAsync());
            Assert.That(ex?.Message, Is.EqualTo("No product found."));
        }

        [Test]
        public async Task SearchByOriginCountry_WithExistingOriginCountry_ShouldReturnMatchingProducts()
        {
            // Arrange
            var productList = new List<Product>
            {
                new Product 
                {
                    OriginCountry = "Bulgaria", 
                    ProductName = "TestProduct1", 
                    ProductCode = "AB12C", 
                    Price = 1.25m, 
                    Quantity = 100, 
                    Description = "Description 1" 
                },
                new Product 
                { 
                    OriginCountry = "USA",
                    ProductName = "TestProduct2",
                    ProductCode = "CD34E", 
                    Price = 2.50m, 
                    Quantity = 200,
                    Description = "Description 2"
                }
            };
            await this.dbContext.Products.AddRangeAsync(productList);
            await this.dbContext.SaveChangesAsync();

            string originCountry = "Bulgaria";

            // Act
            var products = await productsManager.SearchByOriginCountry(originCountry);

            // Assert
            Assert.AreEqual(1, products.Count());
            Assert.IsTrue(products.All(p => p.OriginCountry == originCountry));
        }

        [Test]
        public async Task SearchByOriginCountryAsync_WithNonExistingOriginCountry_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            string nonExistingOriginCountry = "NonExistingCountry";

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await productsManager.SearchByOriginCountry(nonExistingOriginCountry));
            Assert.That(ex?.Message, Is.EqualTo($"No product found with the given first name."));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidProductCode_ShouldReturnProduct()
        {
            // Arrange
            var productToAdd = new Product
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProduct",
                ProductCode = "AB12C",
                Price = 1.25m,
                Quantity = 100,
                Description = "Anything for description"
            };

            await productsManager.AddAsync(productToAdd);

            // Act
            var retrievedProduct = await productsManager.GetSpecificAsync(productToAdd.ProductCode);

            // Assert
            Assert.NotNull(retrievedProduct);
            Assert.AreEqual(productToAdd.ProductCode, retrievedProduct.ProductCode);
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidProductCode_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            string invalidProductCode = "InvalidProductCode";

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await productsManager.GetSpecificAsync(invalidProductCode));
            Assert.That(ex?.Message, Is.EqualTo($"No product found with product code: {invalidProductCode}"));
        }

        [Test]
        public async Task UpdateAsync_WithValidProduct_ShouldUpdateProduct()
        {
            // Arrange
            var updatedProduct = new Product
            {
                ProductCode = "01HNGXES30KY10TA6A3S3A0MRJ",
                ProductName = "UpdatedTestProduct",
                Quantity = 200,
                Price = 2.50m,
                OriginCountry = "Germany",
                Description = "Updated description"
            };

            // Act
            await productsManager.UpdateAsync(updatedProduct);

            // Assert
            var retrievedProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == updatedProduct.ProductCode);
            Assert.NotNull(retrievedProduct);
            Assert.AreEqual(updatedProduct.ProductName, retrievedProduct.ProductName);
            Assert.AreEqual(updatedProduct.Description, retrievedProduct.Description);
            Assert.AreEqual(updatedProduct.Price, retrievedProduct.Price);
            Assert.AreEqual(updatedProduct.Quantity, retrievedProduct.Quantity);
            Assert.AreEqual(updatedProduct.OriginCountry, retrievedProduct.OriginCountry);
        }

        [Test]
        public async Task UpdateAsync_WithInvalidProduct_ShouldThrowValidationException()
        {
            // Arrange
            var updatedProduct = new Product
            {
                ProductCode = "01HNGXES2V1GNTV6C7S30F84RE",
                ProductName = "UpdatedTestProduct",
                Quantity = 200,
                Price = 2.50m,
                OriginCountry = "Germany",
                Description = "Updated description"
            };

            // Act
            await productsManager.UpdateAsync(updatedProduct);

            // Assert
            var retrievedProduct = await dbContext.Products.FirstOrDefaultAsync(p => p.ProductCode == updatedProduct.ProductCode);
            Assert.NotNull(retrievedProduct);
            Assert.AreEqual(updatedProduct.ProductCode, retrievedProduct.ProductCode);
            Assert.AreEqual(updatedProduct.ProductName, retrievedProduct.ProductName);
            Assert.AreEqual(updatedProduct.Quantity, retrievedProduct.Quantity);
            Assert.AreEqual(updatedProduct.Price, retrievedProduct.Price);
            Assert.AreEqual(updatedProduct.OriginCountry, retrievedProduct.OriginCountry);
            Assert.AreEqual(updatedProduct.Description, retrievedProduct.Description);
        }
    }
}
