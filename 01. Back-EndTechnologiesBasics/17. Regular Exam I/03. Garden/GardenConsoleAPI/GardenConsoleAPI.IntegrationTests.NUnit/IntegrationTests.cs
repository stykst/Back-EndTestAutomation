using GardenConsoleAPI.Business;
using GardenConsoleAPI.Business.Contracts;
using GardenConsoleAPI.Data.Models;
using GardenConsoleAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace GardenConsoleAPI.IntegrationTests.NUnit
{
    public class IntegrationTests
    {
        private TestPlantsDbContext dbContext;
        private IPlantsManager plantsManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestPlantsDbContext();
            this.plantsManager = new PlantsManager(new PlantsRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //positive test
        [Test]
        public async Task AddPlantAsync_ShouldAddNewPlant()
        {
            // Arrange
            var newPlant = new Plant
            {
                CatalogNumber = "01QW01PRFC6R",
                Name = "Rose",
                PlantType = "Flower",
                FoodType = "Nectar",
                Quantity = 5
            };

            // Act
            await this.plantsManager.AddAsync(newPlant);
            var dbPlant = await this.dbContext.Plants.FirstOrDefaultAsync(p => p.CatalogNumber == newPlant.CatalogNumber);

            // Assert
            Assert.IsNotNull(dbPlant);
            Assert.AreEqual(newPlant.Name, dbPlant.Name);
            Assert.AreEqual(newPlant.PlantType, dbPlant.PlantType);
            Assert.AreEqual(newPlant.FoodType, dbPlant.FoodType);
            Assert.AreEqual(newPlant.Quantity, dbPlant.Quantity);
        }

        //Negative test
        [Test]
        public async Task AddPlantAsync_TryToAddPlantWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var newPlant = new Plant
            {
                CatalogNumber = "01QW01PRFC6R",
                Name = "Rose",
                PlantType = "",
                FoodType = "Nectar",
                Quantity = 5
            };

            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await this.plantsManager.AddAsync(newPlant));
            var actual = await this.dbContext.Plants.FirstOrDefaultAsync(p => p.CatalogNumber == newPlant.CatalogNumber);

            // Assert
            Assert.IsNull(actual);
            Assert.That(ex?.Message, Is.EqualTo("Invalid plant!"));
        }

        [Test]
        public async Task DeletePlantAsync_WithValidCatalogNumber_ShouldRemovePlantFromDb()
        {
            // Arrange
            var newPlant = new Plant
            {
                CatalogNumber = "01QW01PRFC6S",
                Name = "Rose",
                PlantType = "Flower",
                FoodType = "Nectar",
                Quantity = 5
            };

            await this.plantsManager.AddAsync(newPlant);

            // Act
            await this.plantsManager.DeleteAsync(newPlant.CatalogNumber);
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await this.plantsManager.GetSpecificAsync(newPlant.CatalogNumber));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"No plant found with catalog number: {newPlant.CatalogNumber}"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        public async Task DeletePlantAsync_TryToDeleteWithNullOrWhiteSpaceCatalogNumber_ShouldThrowException(string catalogNumber)
        {
            // Act
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await this.plantsManager.DeleteAsync(catalogNumber));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Catalog number cannot be empty."));
        }

        [Test]
        public async Task GetAllAsync_WhenPlantsExist_ShouldReturnAllPlants()
        {
            // Arrange
            var plantsToAdd = new List<Plant>
            {
                new Plant { CatalogNumber = "01QW01PRFC6A", Name = "Rose", PlantType = "Flower", FoodType = "Nectar", Quantity = 5 },
                new Plant { CatalogNumber = "01QW01PRFC6B", Name = "Tulip", PlantType = "Flower", FoodType = "Nectar", Quantity = 3 },
                new Plant { CatalogNumber = "01QW01PRFC6C", Name = "Oak", PlantType = "Tree", FoodType = "Acorn", Quantity = 10 }
            };

            foreach (var plant in plantsToAdd)
            {
                await this.plantsManager.AddAsync(plant);
            }

            // Act
            var allPlants = await this.plantsManager.GetAllAsync();

            // Assert
            Assert.AreEqual(plantsToAdd.Count, allPlants.Count());
            foreach (var plant in plantsToAdd)
            {
                Assert.IsTrue(allPlants.Any(p => p.Name == plant.Name && p.PlantType == plant.PlantType && p.FoodType == plant.FoodType && p.Quantity == plant.Quantity));
            }
        }

        [Test]
        public async Task GetAllAsync_WhenNoPlantsExist_ShouldThrowKeyNotFoundException()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await plantsManager.GetAllAsync());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No plant found."));
        }

        [Test]
        public async Task SearchByFoodTypeAsync_WithExistingFoodType_ShouldReturnMatchingPlants()
        {
            // Arrange
            var plantsToAdd = new List<Plant>
            {
                new Plant { CatalogNumber = "01QW01PRFC6D", Name = "Rose", PlantType = "Flower", FoodType = "Nectar", Quantity = 5 },
                new Plant { CatalogNumber = "01QW01PRFC6E", Name = "Tulip", PlantType = "Flower", FoodType = "Nectar", Quantity = 3 },
                new Plant { CatalogNumber = "01QW01PRFC6F", Name = "Oak", PlantType = "Tree", FoodType = "Acorn", Quantity = 10 }
            };

            foreach (var plant in plantsToAdd)
            {
                await this.plantsManager.AddAsync(plant);
            }

            // Act
            var matchingPlants = await this.plantsManager.SearchByFoodTypeAsync("Nectar");

            // Assert
            Assert.AreEqual(2, matchingPlants.Count());
            foreach (var plant in plantsToAdd.Where(p => p.FoodType == "Nectar"))
            {
                Assert.IsTrue(matchingPlants.Any(p => p.Name == plant.Name && p.PlantType == plant.PlantType && p.FoodType == plant.FoodType && p.Quantity == plant.Quantity));
            }
        }

        [Test]
        public async Task SearchByFoodTypeAsync_WithNonExistingFoodType_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var notExistingFoodType = "This Food Type Does not Exisit";

            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await this.plantsManager.SearchByFoodTypeAsync(notExistingFoodType));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No plant found with the given food type."));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidCatalogNumber_ShouldReturnPlant()
        {
            // Arrange
            var newPlant = new Plant { CatalogNumber = "01QW01PRFC6G", Name = "Rose", PlantType = "Flower", FoodType = "Nectar", Quantity = 5 };
            await this.plantsManager.AddAsync(newPlant);
            var dbPlant = await this.dbContext.Plants.FirstOrDefaultAsync(p => p.CatalogNumber == newPlant.CatalogNumber);

            // Act
            var retrievedPlant = await this.plantsManager.GetSpecificAsync(newPlant.CatalogNumber);

            // Assert
            Assert.IsNotNull(retrievedPlant);
            Assert.AreEqual(newPlant.Name, dbPlant.Name);
            Assert.AreEqual(newPlant.PlantType, dbPlant.PlantType);
            Assert.AreEqual(newPlant.FoodType, dbPlant.FoodType);
            Assert.AreEqual(newPlant.Quantity, dbPlant.Quantity);
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidCatalogNumber_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var invalidCatalogNumber = Guid.NewGuid().ToString();

            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await this.plantsManager.GetSpecificAsync(invalidCatalogNumber));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"No plant found with catalog number: {invalidCatalogNumber}"));

        }

        [Test]
        public async Task UpdateAsync_WithValidPlant_ShouldUpdatePlant()
        {
            // Arrange
            var plantsToAdd = new List<Plant>
            {
                new Plant { CatalogNumber = "01QW01PRFC6X", Name = "Rose", PlantType = "Flower", FoodType = "Nectar", Quantity = 5 },
                new Plant { CatalogNumber = "01QW01PRFC6Y", Name = "Tulip", PlantType = "Flower", FoodType = "Nectar", Quantity = 3 },
                new Plant { CatalogNumber = "01QW01PRFC6Z", Name = "Oak", PlantType = "Tree", FoodType = "Acorn", Quantity = 10 }
            };

            foreach (var plant in plantsToAdd)
            {
                await this.plantsManager.AddAsync(plant);
            }

            var updatedPlant = plantsToAdd[0];
            updatedPlant.Name = "UPDATED!";
            updatedPlant.Quantity = 10;

            // Act
            await plantsManager.UpdateAsync(updatedPlant);
            var dbPlant = await this.dbContext.Plants.FirstOrDefaultAsync(p => p.CatalogNumber == updatedPlant.CatalogNumber);

            // Assert
            Assert.NotNull(dbPlant);
            Assert.That(dbPlant.Name, Is.EqualTo(updatedPlant.Name));
            Assert.That(dbPlant.PlantType, Is.EqualTo(updatedPlant.PlantType));
            Assert.That(dbPlant.FoodType, Is.EqualTo(updatedPlant.FoodType));
            Assert.That(dbPlant.Quantity, Is.EqualTo(updatedPlant.Quantity));
        }

        [Test]
        public async Task UpdateAsync_WithInvalidPlant_ShouldThrowValidationException()
        {
            // Arrange
            var invalidPlant = new Plant();

            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await this.plantsManager.UpdateAsync(invalidPlant));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Invalid plant!"));
        }
    }
}
