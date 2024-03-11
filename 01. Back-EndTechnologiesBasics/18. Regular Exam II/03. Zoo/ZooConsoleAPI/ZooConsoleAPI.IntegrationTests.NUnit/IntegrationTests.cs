using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;
using ZooConsoleAPI.Business;
using ZooConsoleAPI.Business.Contracts;
using ZooConsoleAPI.Data.Model;
using ZooConsoleAPI.DataAccess;

namespace ZooConsoleAPI.IntegrationTests.NUnit
{
    public class IntegrationTests
    {
        private TestAnimalDbContext dbContext;
        private IAnimalsManager animalsManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestAnimalDbContext();
            this.animalsManager = new AnimalsManager(new AnimalRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //positive test
        [Test]
        public async Task AddAnimalAsync_ShouldAddNewAnimal()
        {
            // Arrange
            var newAnimal = new Animal
            {
                CatalogNumber = "01QWTWXTQSH1",
                Name = "Lion",
                Breed = "African Lion",
                Type = "Mammal",
                Age = 5,
                Gender = "Male",
                IsHealthy = true
            };

            // Act
            await this.animalsManager.AddAsync(newAnimal);
            var dbAnimal = await this.dbContext.Animals.FirstOrDefaultAsync(a => a.CatalogNumber == newAnimal.CatalogNumber);

            // Assert
            Assert.IsNotNull(dbAnimal);
            Assert.That(dbAnimal.Name, Is.EqualTo(newAnimal.Name));
            Assert.That(dbAnimal.Breed, Is.EqualTo(newAnimal.Breed));
            Assert.That(dbAnimal.Type, Is.EqualTo(newAnimal.Type));
            Assert.That(dbAnimal.Age, Is.EqualTo(newAnimal.Age));
            Assert.That(dbAnimal.Gender, Is.EqualTo(newAnimal.Gender));
            Assert.That(dbAnimal.IsHealthy, Is.EqualTo(newAnimal.IsHealthy));
        }

        //Negative test
        [Test]
        public async Task AddAnimalAsync_TryToAddAnimalWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var newAnimal = new Animal
            {
                CatalogNumber = "01QWTWXTQSH1",
                Name = "",
                Breed = "African Lion",
                Type = "Mammal",
                Age = 5,
                Gender = "Male",
                IsHealthy = true
            };

            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await this.animalsManager.AddAsync(newAnimal));
            var actual = await this.dbContext.Animals.FirstOrDefaultAsync(a => a.CatalogNumber == newAnimal.CatalogNumber);

            // Assert
            Assert.IsNull(actual);
            Assert.That(ex?.Message, Is.EqualTo("Invalid animal!"));
        }

        [Test]
        public async Task DeleteAnimalAsync_WithValidCatalogNumber_ShouldRemoveAnimalFromDb()
        {
            // Arrange
            var newAnimal = new Animal
            {
                CatalogNumber = "01QWTWXTQSH2",
                Name = "Lion",
                Breed = "African Lion",
                Type = "Mammal",
                Age = 5,
                Gender = "Male",
                IsHealthy = true
            };
            await this.animalsManager.AddAsync(newAnimal);

            // Act
            await this.animalsManager.DeleteAsync(newAnimal.CatalogNumber);
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await this.animalsManager.GetSpecificAsync(newAnimal.CatalogNumber));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"No animal found with catalog number: {newAnimal.CatalogNumber}"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("     ")]
        public async Task DeleteAnimalAsync_TryToDeleteWithNullOrWhiteSpaceCatalogNumber_ShouldThrowException(string catalogNumber)
        {
            // Act
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await this.animalsManager.DeleteAsync(catalogNumber));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Catalog number cannot be empty."));
        }


        [Test]
        public async Task GetAllAsync_WhenAnimalsExist_ShouldReturnAllAnimals()
        {
            // Arrange
            var animals = new List<Animal>
            {
                new Animal 
                { 
                    CatalogNumber = "01QWTWXTQSH3", 
                    Name = "Lion", 
                    Breed = "African Lion", 
                    Type = "Mammal", 
                    Age = 5, 
                    Gender = "Male",      
                    IsHealthy = true 
                },
                new Animal 
                { 
                    CatalogNumber = "01QWTWXTQSH4", 
                    Name = "Tiger", 
                    Breed = "Bengal Tiger", 
                    Type = "Mammal", 
                    Age = 6, 
                    Gender = "Female",     
                    IsHealthy = true
                },
                new Animal 
                { 
                    CatalogNumber = "01QWTWXTQSH5", 
                    Name = "Elephant", 
                    Breed = "African Elephant", 
                    Type = "Mammal", 
                    Age = 10, 
                    Gender = "Male", 
                    IsHealthy = false 
                }
            };

            foreach (var animal in animals)
            {
                await this.animalsManager.AddAsync(animal);
            }

            // Act
            var allAnimals = await this.animalsManager.GetAllAsync();

            // Assert
            Assert.AreEqual(animals.Count, allAnimals.Count());
            foreach (var animal in animals)
            {
                Assert.IsTrue(allAnimals.Any(a => a.Name == animal.Name && a.Breed == animal.Breed && a.Type == animal.Type &&a.Age == animal.Age && a.Gender == animal.Gender && a.IsHealthy == animal.IsHealthy));
            }
        }

        [Test]
        public async Task GetAllAsync_WhenNoAnimalsExist_ShouldThrowKeyNotFoundException()
        {
            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await animalsManager.GetAllAsync());

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No animal found."));
        }


        [Test]
        public async Task SearchByTypeAsync_WithExistingType_ShouldReturnMatchingAnimals()
        {
            // Arrange
            var animals = new List<Animal>
            {
                new Animal
                {
                    CatalogNumber = "01QWTWXTQSH6",
                    Name = "Lion",
                    Breed = "African Lion",
                    Type = "Mammal", 
                    Age = 5,
                    Gender = "Male",
                    IsHealthy = true
                },
                new Animal
                {
                    CatalogNumber = "01QWTWXTQSH7",
                    Name = "Tiger",
                    Breed = "Bengal Tiger",
                    Type = "Mammal",
                    Age = 6,
                    Gender = "Female",
                    IsHealthy = true
                },
                new Animal
                {
                    CatalogNumber = "01QWTWXTQSH8",
                    Name = "Cobra",
                    Breed = "African Cobra",
                    Type = "Reptil",
                    Age = 10,
                    Gender = "Male",
                    IsHealthy = false
                }
            };

            foreach (var animal in animals)
            {
                await this.animalsManager.AddAsync(animal);
            }

            // Act
            var matchingAnimals = await this.animalsManager.SearchByTypeAsync("Mammal");

            // Assert
            Assert.IsNotNull(matchingAnimals);
            Assert.AreEqual(2, matchingAnimals.Count());
            foreach (var animal in animals.Where(a => a.Type == "Mammal"))
            {
                Assert.IsTrue(matchingAnimals.Any(a => a.Name == animal.Name && a.Breed == animal.Breed && a.Type == animal.Type && a.Age == animal.Age && a.Gender == animal.Gender && a.IsHealthy == animal.IsHealthy));
            }
        }

        [Test]
        public async Task SearchByTypeAsync_WithNonExistingType_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var notExistingType = "This Type Does not Exisit";

            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await this.animalsManager.SearchByTypeAsync(notExistingType));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("No animal found with the given type."));
        }


        [Test]
        public async Task GetSpecificAsync_WithValidCatalogNumber_ShouldReturnAnimal()
        {
            // Arrange
            var newAnimal = new Animal
            {
                CatalogNumber = "01QWTWXTQSH9",
                Name = "Lion",
                Breed = "African Lion",
                Type = "Mammal",
                Age = 5,
                Gender = "Male",
                IsHealthy = true
            };
            await this.animalsManager.AddAsync(newAnimal);
            var dbAnimal = await this.dbContext.Animals.FirstOrDefaultAsync(a => a.CatalogNumber == newAnimal.CatalogNumber);

            // Act
            var retrievedAnimal = await this.animalsManager.GetSpecificAsync(newAnimal.CatalogNumber);

            // Assert
            Assert.IsNotNull(dbAnimal);
            Assert.That(dbAnimal.Name, Is.EqualTo(newAnimal.Name));
            Assert.That(dbAnimal.Breed, Is.EqualTo(newAnimal.Breed));
            Assert.That(dbAnimal.Type, Is.EqualTo(newAnimal.Type));
            Assert.That(dbAnimal.Age, Is.EqualTo(newAnimal.Age));
            Assert.That(dbAnimal.Gender, Is.EqualTo(newAnimal.Gender));
            Assert.That(dbAnimal.IsHealthy, Is.EqualTo(newAnimal.IsHealthy));
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidCatalogNumber_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var invalidCatalogNumber = Guid.NewGuid().ToString();

            // Act
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await this.animalsManager.GetSpecificAsync(invalidCatalogNumber));

            // Assert
            Assert.That(ex.Message, Is.EqualTo($"No animal found with catalog number: {invalidCatalogNumber}"));
        }


        [Test]
        public async Task UpdateAsync_WithValidAnimal_ShouldUpdateAnimal()
        {
            // Arrange
            var animals = new List<Animal>
            {
                new Animal
                {
                    CatalogNumber = "01QWTWXTQSI1",
                    Name = "Lion",
                    Breed = "African Lion",
                    Type = "Mammal", 
                    Age = 5,
                    Gender = "Male",
                    IsHealthy = true
                },
                new Animal
                {
                    CatalogNumber = "01QWTWXTQSI2",
                    Name = "Tiger",
                    Breed = "Bengal Tiger",
                    Type = "Mammal",
                    Age = 6,
                    Gender = "Female",
                    IsHealthy = true
                },
                new Animal
                {
                    CatalogNumber = "01QWTWXTQSI3",
                    Name = "Cobra",
                    Breed = "African Cobra",
                    Type = "Reptil",
                    Age = 10,
                    Gender = "Male",
                    IsHealthy = false
                }
            };

            foreach (var animal in animals)
            {
                await this.animalsManager.AddAsync(animal);
            }

            var updatedAnimal = animals[0];
            updatedAnimal.Name = "UPDATED!";
            updatedAnimal.Age = 10;

            // Act
            await this.animalsManager.UpdateAsync(updatedAnimal);
            var dbAnimal = await this.dbContext.Animals.FirstOrDefaultAsync(a => a.CatalogNumber == updatedAnimal.CatalogNumber);

            // Assert
            Assert.IsNotNull(dbAnimal);
            Assert.That(dbAnimal.Name, Is.EqualTo(updatedAnimal.Name));
            Assert.That(dbAnimal.Breed, Is.EqualTo(updatedAnimal.Breed));
            Assert.That(dbAnimal.Type, Is.EqualTo(updatedAnimal.Type));
            Assert.That(dbAnimal.Age, Is.EqualTo(updatedAnimal.Age));
            Assert.That(dbAnimal.Gender, Is.EqualTo(updatedAnimal.Gender));
            Assert.That(dbAnimal.IsHealthy, Is.EqualTo(updatedAnimal.IsHealthy));
        }

        [Test]
        public async Task UpdateAsync_WithInvalidAnimal_ShouldThrowValidationException()
        {
            // Arrange
            var invalidAnimal = new Animal();

            // Act
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await this.animalsManager.UpdateAsync(invalidAnimal));

            // Assert
            Assert.That(ex.Message, Is.EqualTo("Invalid animal!"));
        }

    }
}

