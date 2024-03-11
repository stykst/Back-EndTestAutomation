using LibroConsoleAPI.Business;
using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using LibroConsoleAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibroConsoleAPI.IntegrationTests.NUnit
{
    public  class IntegrationTests
    {
        private TestLibroDbContext dbContext;
        private IBookManager bookManager;

        [SetUp]
        public void SetUp()
        {
            string dbName = $"TestDb_{Guid.NewGuid()}";
            this.dbContext = new TestLibroDbContext(dbName);
            this.bookManager = new BookManager(new BookRepository(this.dbContext));
        }

        [TearDown]
        public void TearDown()
        {
            this.dbContext.Dispose();
        }

        [Test]
        public async Task AddBookAsync_ShouldAddBook()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123",
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act
            await bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.That(bookInDb, Is.Not.Null);
            Assert.That(bookInDb.Title, Is.EqualTo("Test Book"));
            Assert.That(bookInDb.Author, Is.EqualTo("John Doe"));
        }

        [Test]
        public async Task AddBookAsync_TryToAddBookWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange
            var invalidBook = new Book
            {
                // Provide invalid credentials, e.g., missing required fields
                Title = null, // Title is required, so we'll leave it null
                Author = "John Doe",
                ISBN = "1234567890123", // Example ISBN
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act and Assert
            // Use Assert.ThrowsAsync to assert that an exception of type ValidationException is thrown
            Assert.ThrowsAsync<ValidationException>(() => bookManager.AddAsync(invalidBook));
        }


        [Test]
        public async Task DeleteBookAsync_WithValidISBN_ShouldRemoveBookFromDb()
        {
            // Arrange
            var newBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123", // Example ISBN
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await bookManager.AddAsync(newBook); // Add the book to the database

            // Act
            await bookManager.DeleteAsync(newBook.ISBN); // Delete the book from the database

            // Assert
            var bookInDb = await dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.That(bookInDb, Is.Null); // Assert that the book is no longer in the database
        }


        [Test]
        public async Task DeleteBookAsync_TryToDeleteWithNullOrWhiteSpaceISBN_ShouldThrowException()
        {
            // Arrange
            var invalidISBN = " "; // Whitespace ISBN

            // Act and Assert
            // Use Assert.ThrowsAsync to assert that an exception of type ArgumentException is thrown
            Assert.ThrowsAsync<ArgumentException>(() => bookManager.DeleteAsync(invalidISBN));
        }


        [Test]
        public async Task GetAllAsync_WhenBooksExist_ShouldReturnAllBooks()
        {
            // Arrange
            var booksToAdd = new List<Book>
    {
        new Book
        {
            Title = "Book 1",
            Author = "Author 1",
            ISBN = "1234567890123", // Example ISBN
            YearPublished = 2021,
            Genre = "Fiction",
            Pages = 100,
            Price = 19.99
        },
        new Book
        {
            Title = "Book 2",
            Author = "Author 2",
            ISBN = "2345678901234", // Example ISBN
            YearPublished = 2020,
            Genre = "Non-Fiction",
            Pages = 150,
            Price = 29.99
        }
    };

            foreach (var book in booksToAdd)
            {
                await bookManager.AddAsync(book); // Add books to the database
            }

            // Act
            var result = await bookManager.GetAllAsync();

            // Assert
            Assert.That(result, Is.Not.Null); // Assert that the result is not null
            Assert.That(result.Count(), Is.EqualTo(2)); // Assert that all added books are returned
        }


        [Test]
        public async Task GetAllAsync_WhenNoBooksExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange: Ensure that there are no books in the database

            // Act and Assert: Use Assert.ThrowsAsync to assert that an exception of type KeyNotFoundException is thrown
            Assert.ThrowsAsync<KeyNotFoundException>(() => bookManager.GetAllAsync());
        }


        [Test]
        public async Task SearchByTitleAsync_WithValidTitleFragment_ShouldReturnMatchingBooks()
        {
            // Arrange: Add books with titles containing the specified title fragment
            var booksToAdd = new List<Book>
    {
        new Book
        {
            Title = "Book with Title Fragment",
            Author = "Author 1",
            ISBN = "1234567890123", // Example ISBN
            YearPublished = 2021,
            Genre = "Fiction",
            Pages = 100,
            Price = 19.99
        },
        new Book
        {
            Title = "Another Book with Title Fragment",
            Author = "Author 2",
            ISBN = "2345678901234", // Example ISBN
            YearPublished = 2020,
            Genre = "Non-Fiction",
            Pages = 150,
            Price = 29.99
        }
    };

            foreach (var book in booksToAdd)
            {
                await bookManager.AddAsync(book); // Add books to the database
            }

            // Act: Search for books with the specified title fragment
            var result = await bookManager.SearchByTitleAsync("Fragment");

            // Assert: Ensure that the result contains matching books
            Assert.That(result, Is.Not.Null); // Assert that the result is not null
            Assert.That(result.Any(), Is.True); // Assert that there is at least one matching book
            Assert.That(result.All(b => b.Title.Contains("Fragment")), Is.True); // Assert that all returned books contain the specified title fragment
        }


        [Test]
        public async Task SearchByTitleAsync_WithInvalidTitleFragment_ShouldThrowKeyNotFoundException()
        {
            // Arrange: Ensure that there are no books in the database or no matching books with the specified title fragment

            // Act and Assert: Use Assert.ThrowsAsync to assert that an exception of type KeyNotFoundException is thrown
            Assert.ThrowsAsync<KeyNotFoundException>(() => bookManager.SearchByTitleAsync("InvalidFragment"));
        }


        [Test]
        public async Task GetSpecificAsync_WithValidIsbn_ShouldReturnBook()
        {
            // Arrange: Add a book with the specified ISBN to the database
            var bookToAdd = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123", // Example ISBN
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await bookManager.AddAsync(bookToAdd); // Add the book to the database

            // Act: Get the book with the specified ISBN
            var result = await bookManager.GetSpecificAsync("1234567890123");

            // Assert: Ensure that the result is not null and it represents the correct book
            Assert.NotNull(result); // Assert that the result is not null
            Assert.That(result.Title, Is.EqualTo("Test Book")); // Assert that the returned book has the correct title
            Assert.That(result.Author, Is.EqualTo("John Doe")); // Assert that the returned book has the correct author
        }


        [Test]
        public async Task GetSpecificAsync_WithInvalidIsbn_ShouldThrowKeyNotFoundException()
        {
            // Arrange: Ensure that there are no books in the database or no book with the specified invalid ISBN

            // Act and Assert: Use Assert.ThrowsAsync to assert that an exception of type KeyNotFoundException is thrown
            Assert.ThrowsAsync<KeyNotFoundException>(() => bookManager.GetSpecificAsync("InvalidISBN"));
        }


        [Test]
        public async Task UpdateAsync_WithValidBook_ShouldUpdateBook()
        {
            // Arrange: Add a book with the specified details to the database
            var initialBook = new Book
            {
                Title = "Initial Title",
                Author = "Initial Author",
                ISBN = "1234567890123", // Example ISBN
                YearPublished = 2020,
                Genre = "Initial Genre",
                Pages = 200,
                Price = 29.99
            };

            await bookManager.AddAsync(initialBook); // Add the initial book to the database

            // Act: Update the book's details

            var bookToUpdate = await bookManager.GetSpecificAsync(initialBook.ISBN);

            var updatedBook = new Book
            {
                Title = "Updated Title",
                Author = "Updated Author",
                ISBN = "1234567890123", // Same ISBN as initial book
                YearPublished = 2021,
                Genre = "Updated Genre",
                Pages = 250,
                Price = 39.99
            };

            bookToUpdate.Title = updatedBook.Title;
            bookToUpdate.Author = updatedBook.Author;
            bookToUpdate.YearPublished = updatedBook.YearPublished;
            bookToUpdate.Genre = updatedBook.Genre;
            bookToUpdate.Pages = updatedBook.Pages;
            bookToUpdate.Price = updatedBook.Price;            

            await bookManager.UpdateAsync(bookToUpdate); // Update the book in the database

            // Assert: Retrieve the updated book from the database and ensure it reflects the changes
            var result = await bookManager.GetSpecificAsync("1234567890123"); // Get the updated book

            Assert.That(result, Is.Not.Null); // Assert that the result is not null
            Assert.That(result.Title, Is.EqualTo("Updated Title")); // Assert that the title is updated
            Assert.That(result.Author, Is.EqualTo("Updated Author")); // Assert that the author is updated
            Assert.That(result.YearPublished, Is.EqualTo(2021)); // Assert that the year published is updated
            Assert.That(result.Genre, Is.EqualTo("Updated Genre")); // Assert that the genre is updated
            Assert.That(result.Pages, Is.EqualTo(250)); // Assert that the number of pages is updated
            Assert.That(result.Price, Is.EqualTo(39.99)); // Assert that the price is updated
        }


        [Test]
        public async Task UpdateAsync_WithInvalidBook_ShouldThrowValidationException()
        {
            // Arrange: Prepare an invalid book (e.g., missing required fields)
            var invalidBook = new Book
            {
                // Missing required fields (e.g., Title)
                Author = "John Doe",
                ISBN = "1234567890123", // Example ISBN
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            // Act and Assert: Use Assert.ThrowsAsync to assert that an exception of type ValidationException is thrown
            Assert.ThrowsAsync<ValidationException>(() => bookManager.UpdateAsync(invalidBook));
        }

    }
}
