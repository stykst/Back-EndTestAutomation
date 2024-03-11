using Homies.Data;
using Homies.Data.Models;
using Homies.Models.Event;
using Homies.Services;
using Microsoft.EntityFrameworkCore;

namespace Homies.Tests
{
    [TestFixture]
    internal class EventServiceTests
    {
        private HomiesDbContext _dbContext;
        private EventService _eventService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HomiesDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Use unique database name to avoid conflicts
                .Options;
            _dbContext = new HomiesDbContext(options);

            _eventService = new EventService(_dbContext);
        }

        [Test]
        public async Task AddEventAsync_ShouldAddEvent_WhenValidEventModelAndUserId()
        {
            // Step 1: Arrange - Set up the initial conditions for the test
            // Create a new event model with test data
            //var eventModel = new EventFormModel
            //{
            //    Name = "Test Event",
            //    Description = "Test Description",
            //    Start = DateTime.Now,
            //    End = DateTime.Now.AddHours(2)
            //};
            // Define a user ID for testing purposes
            //string userId = "testUserId";

            // Step 2: Act - Perform the action being tested
            // Call the service method to add the event

            // Step 3: Assert - Verify the outcome of the action
            // Retrieve the added event from the database

            // Assert that the added event is not null, indicating it was successfully added

            // Assert that the description of the added event matches the description provided in the event model

            throw new NotImplementedException();
        }


        [Test]
        public async Task GetAllEventsAsync_ShouldReturnAllEvents()
        {
            // Step 1: Arrange - Set up the initial conditions for the test
            // Create two event models with test data

            // Define a user ID for testing purposes
            string userId = "testUserId";

            // Step 2: Act - Perform the action being tested
            // Add the two events to the database using the event service

            // Step 3: Act - Retrieve the count of events from the database

            // Step 4: Assert - Verify the outcome of the action
            // Assert that the count of events in the database is equal to the expected count (2)

            throw new NotImplementedException();
        }
    }
}
