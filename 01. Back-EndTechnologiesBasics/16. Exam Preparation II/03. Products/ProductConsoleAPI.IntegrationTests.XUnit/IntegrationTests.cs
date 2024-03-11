using Microsoft.EntityFrameworkCore;
using ProductConsoleAPI.Business.Contracts;
using ProductConsoleAPI.Business;
using ProductConsoleAPI.Data.Models;
using ProductConsoleAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductConsoleAPI.IntegrationTests.XUnit
{
    public class IntegrationTests : IClassFixture<ProductsManagerFixture>
    {
        private TestProductsDbContext dbContext;
        private IProductsManager productsManager;

        public IntegrationTests(ProductsManagerFixture fixture)
        {
            this.productsManager = fixture.ProductsManager;
            dbContext = fixture.DbContext;
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }


        //positive test
        [Fact] 
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
            Assert.Equal(newProduct.ProductName, dbProduct.ProductName);
            Assert.Equal(newProduct.Description, dbProduct.Description);
            Assert.Equal(newProduct.Price, dbProduct.Price);
            Assert.Equal(newProduct.Quantity, dbProduct.Quantity);
            Assert.Equal(newProduct.OriginCountry, dbProduct.OriginCountry);
            Assert.Equal(newProduct.ProductCode, dbProduct.ProductCode);
        }

        //Negative test
        [Fact]
        public async Task AddProductAsync_TryToAddProductWithInvalidCredentials_ShouldThrowException()
        {
            var newProduct = new Product()
            {
                OriginCountry = "Bulgaria",
                ProductName = "TestProduct",
                ProductCode = "AB12C",
                Price = -10m,
                Quantity = 100,
                Description = "Anything for description"
            };

            var ex = await Assert.ThrowsAsync<ValidationException>(async () => await productsManager.AddAsync(newProduct));
            var actual = await dbContext.Products.FirstOrDefaultAsync(c => c.ProductCode == newProduct.ProductCode);

            Assert.Null(actual);
            Assert.Equal("Invalid product!", ex?.Message);

        }

        [Fact(Skip = "Not implemented yet")] //delete the part "(Skip = "Not implemented yet")" before implement the test
        public async Task DeleteProductAsync_WithValidProductCode_ShouldRemoveProductFromDb()
        {
            // Arrange

            // Act

            // Assert
            
        }

        [Fact(Skip = "Not implemented yet")] //delete the part "(Skip = "Not implemented yet")" before implement the test
        public async Task DeleteProductAsync_TryToDeleteWithNullOrWhiteSpaceProductCode_ShouldThrowException()
        {
            // Arrange

            // Act

            // Assert
            
        }

        [Fact(Skip = "Not implemented yet")] //delete the part "(Skip = "Not implemented yet")" before implement the test
        public async Task GetAllAsync_WhenProductsExist_ShouldReturnAllProducts()
        {
            // Arrange

            // Act

            // Assert
            
        }

        [Fact(Skip = "Not implemented yet")] //delete the part "(Skip = "Not implemented yet")" before implement the test
        public async Task GetAllAsync_WhenNoProductsExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act

            // Assert
            
        }

        [Fact(Skip = "Not implemented yet")] //delete the part "(Skip = "Not implemented yet")" before implement the test
        public async Task SearchByOriginCountry_WithExistingOriginCountry_ShouldReturnMatchingProducts()
        {
            // Arrange

            // Act

            // Assert
            
        }

        [Fact(Skip = "Not implemented yet")] //delete the part "(Skip = "Not implemented yet")" before implement the test
        public async Task SearchByOriginCountryAsync_WithNonExistingOriginCountry_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act

            // Assert
            
        }

        [Fact(Skip = "Not implemented yet")] //delete the part "(Skip = "Not implemented yet")" before implement the test
        public async Task GetSpecificAsync_WithValidProductCode_ShouldReturnProduct()
        {
            // Arrange

            // Act

            // Assert
            
        }

        [Fact(Skip = "Not implemented yet")] //delete the part "(Skip = "Not implemented yet")" before implement the test
        public async Task GetSpecificAsync_WithInvalidProductCode_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act

            // Assert
            
        }

        [Fact(Skip = "Not implemented yet")] //delete the part "(Skip = "Not implemented yet")" before implement the test
        public async Task UpdateAsync_WithValidProduct_ShouldUpdateProduct()
        {
            // Arrange

            // Act

            // Assert
            
        }

        [Fact(Skip = "Not implemented yet")] //delete the part "(Skip = "Not implemented yet")" before implement the test
        public async Task UpdateAsync_WithInvalidProduct_ShouldThrowValidationException()
        {
            // Arrange

            // Act

            // Assert
            
        }
    }
}
