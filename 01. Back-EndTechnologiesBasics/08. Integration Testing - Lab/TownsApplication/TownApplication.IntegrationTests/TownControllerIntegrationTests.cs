namespace TownApplication.IntegrationTests
{
    public class TownControllerIntegrationTests
    {
        private readonly TownController _controller;

        public TownControllerIntegrationTests()
        {
            _controller = new TownController();
            _controller.ResetDatabase();
        }

        [Fact]
        public void AddTown_ValidInput_ShouldAddTown()
        {
            // Arrange
            var townName = "Sofia";
            var population = 1236000;

            // Act
            _controller.AddTown(townName, population);

            // Assert
            var townInDb = _controller.GetTownByName(townName);
            Assert.NotNull(townInDb);
            Assert.Equal(population, townInDb.Population);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("AB")]
        public void AddTown_InvalidName_ShouldThrowArgumentException(string invalidName)
        {
            // Arrange
            var population = 1236000;

            // Act & Assert
            var exeption = Assert.Throws<ArgumentException>(() => _controller.AddTown(invalidName, population));
            Assert.Equal("Invalid town name.", exeption.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddTown_InvalidPopulation_ShouldThrowArgumentException(int invalidPopulation)
        {
            // TODO: This test ensures that the AddTown method correctly handles invalid population values.

            // Arrange
            var townName = "Sofia";

            // Act
            var exeption = Assert.Throws<ArgumentException>(() => _controller.AddTown(townName, invalidPopulation));

            // Assert
            Assert.Equal("Population must be a positive number.", exeption.Message);
        }

        [Fact]
        public void AddTown_DuplicateTownName_DoesNotAddDuplicateTown()
        {
            // TODO: This test verifies that the AddTown method does not add a duplicate town.

            // Arrange
            var townName = "Sofia";
            var population = 1236000;

            _controller.AddTown(townName, population);

            // Act
            _controller.AddTown(townName, population);

            // Assert
            var result = _controller.ListTowns();
            Assert.Single(result);
            var item = result.FirstOrDefault();
            Assert.Equal(population, item.Population);
            Assert.Equal(townName, item.Name);
        }

        [Fact]
        public void UpdateTown_ShouldUpdatePopulation()
        {
            // Arrange
            var townName = "Sofia";
            var population = 1236000;
            var newPopulation = 1236000;

            _controller.AddTown(townName, population);

            // Act
            _controller.UpdateTown(_controller.GetTownByName(townName).Id, newPopulation);

            // Assert
            Assert.NotNull(_controller.GetTownByName(townName));
            Assert.Equal(newPopulation, _controller.GetTownByName(townName).Population);
        }

        [Fact]
        public void DeleteTown_ShouldDeleteTown()
        {
            // Arrange
            var townName = "Sofia";
            var population = 336000;

            _controller.AddTown(townName, population);

            // Act
            _controller.DeleteTown(_controller.GetTownByName(townName).Id);

            // Assert
            Assert.Null(_controller.GetTownByName(townName));
        }

        [Fact]
        public void ListTowns_ShouldReturnTowns()
        {
            // Arrange
            var towns = new List<string>
            {
                "Sofia",
                "Plovdiv",
                "Varna",
                "Burgas"
            };

            foreach (var town in towns)
            {
                _controller.AddTown(town, town.Length * 1000);
            }

            // Act
            var allTowns = _controller.ListTowns();

            // Assert
            Assert.Equal(towns.Count, allTowns.Count);
        }
    }
}
