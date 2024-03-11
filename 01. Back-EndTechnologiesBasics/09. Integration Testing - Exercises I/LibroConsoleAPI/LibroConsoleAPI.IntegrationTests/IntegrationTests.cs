using LibroConsoleAPI.Business;
using LibroConsoleAPI.Business.Contracts;
using LibroConsoleAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibroConsoleAPI.IntegrationTests
{
    public class IntegrationTests : IClassFixture<BookManagerFixture>
    {
        private readonly BookManagerFixture _fixture;
        private readonly IBookManager _bookManager;
        private readonly TestLibroDbContext _dbContext;

        public IntegrationTests()
        {
            _fixture = new BookManagerFixture();
            _bookManager = _fixture.BookManager;
            _dbContext = _fixture.DbContext;
        }

        [Fact]
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
            await _bookManager.AddAsync(newBook);

            // Assert
            var bookInDb = await _dbContext.Books.FirstOrDefaultAsync(b => b.ISBN == newBook.ISBN);
            Assert.NotNull(bookInDb);
            Assert.Equal("Test Book", bookInDb.Title);
            Assert.Equal("John Doe", bookInDb.Author);
        }

        [Fact]
        public async Task AddBookAsync_TryToAddBookWithInvalidCredentials_ShouldThrowException()
        {
            // Arrange: Prepare an invalid book (e.g., missing required fields)
            var invalidBook = new Book(); // Invalid book without required fields

            // Act and Assert: Use Assert.ThrowsAsync to assert that an exception of type ValidationException is thrown
            await Assert.ThrowsAsync<ValidationException>(() => _bookManager.AddAsync(invalidBook));
        }


        [Fact]
        public async Task DeleteBookAsync_WithValidISBN_ShouldRemoveBookFromDb()
        {
            // Arrange: Add a book with a valid ISBN to the database
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
            await _bookManager.AddAsync(newBook);

            // Act: Delete the book with the valid ISBN
            await _bookManager.DeleteAsync("1234567890123");

            // Assert: Verify that the book is removed from the database
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookManager.GetSpecificAsync("1234567890123"));
        }


        [Fact]
        public async Task DeleteBookAsync_TryToDeleteWithNullOrWhiteSpaceISBN_ShouldThrowException()
        {
            // Arrange: Prepare a null or whitespace ISBN
            string nullOrWhiteSpaceISBN = null; // Or string.Empty, or whitespace string

            // Act and Assert: Use Assert.ThrowsAsync to assert that an exception is thrown
            await Assert.ThrowsAsync<ArgumentException>(() => _bookManager.DeleteAsync(nullOrWhiteSpaceISBN));
        }


        [Fact]
        public async Task GetAllAsync_WhenBooksExist_ShouldReturnAllBooks()
        {
            // Arrange: Add some books to the database
            var book1 = new Book
            {
                Title = "Book 1",
                Author = "Author 1",
                ISBN = "1111111111111", // Example ISBN
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 200,
                Price = 29.99
            };
            var book2 = new Book
            {
                Title = "Book 2",
                Author = "Author 2",
                ISBN = "2222222222222", // Example ISBN
                YearPublished = 2021,
                Genre = "Non-Fiction",
                Pages = 250,
                Price = 39.99
            };

            await _bookManager.AddAsync(book1);
            await _bookManager.AddAsync(book2);

            // Act: Retrieve all books
            var result = await _bookManager.GetAllAsync();

            // Assert: Verify that all expected books are returned
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Assuming you expect 2 books to be returned
            Assert.Contains(result, b => b.ISBN == "1111111111111");
            Assert.Contains(result, b => b.ISBN == "2222222222222");
        }


        [Fact]
        public async Task GetAllAsync_WhenNoBooksExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange: Ensure there are no books in the database (by clearing it, for example)
            _dbContext.Books.RemoveRange(_dbContext.Books);
            await _dbContext.SaveChangesAsync();

            // Act and Assert: Use Assert.ThrowsAsync to assert that a KeyNotFoundException is thrown
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookManager.GetAllAsync());
        }


        [Fact]
        public async Task SearchByTitleAsync_WithValidTitleFragment_ShouldReturnMatchingBooks()
        {
            // Arrange: Add some books with titles containing the specified title fragment to the database
            var book1 = new Book
            {
                Title = "Book with valid fragment in title",
                Author = "Author 1",
                ISBN = "1111111111111", // Example ISBN
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 200,
                Price = 29.99
            };
            var book2 = new Book
            {
                Title = "Another book with valid fragment in title",
                Author = "Author 2",
                ISBN = "2222222222222", // Example ISBN
                YearPublished = 2021,
                Genre = "Non-Fiction",
                Pages = 250,
                Price = 39.99
            };

            await _bookManager.AddAsync(book1);
            await _bookManager.AddAsync(book2);

            // Act: Search books by the valid title fragment
            var result = await _bookManager.SearchByTitleAsync("valid fragment");

            // Assert: Verify that all expected matching books are returned
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Assuming you expect 2 matching books to be returned
            Assert.Contains(result, b => b.Title == "Book with valid fragment in title");
            Assert.Contains(result, b => b.Title == "Another book with valid fragment in title");
        }


        [Fact]
        public async Task SearchByTitleAsync_WithInvalidTitleFragment_ShouldThrowKeyNotFoundException()
        {
            // Arrange: Ensure there are no books in the database that match the invalid title fragment
            // For example, clear the database or remove any existing books with matching titles
            _dbContext.Books.RemoveRange(_dbContext.Books.Where(b => b.Title.Contains("invalid title fragment")));
            await _dbContext.SaveChangesAsync();

            // Act and Assert: Use Assert.ThrowsAsync to assert that a KeyNotFoundException is thrown
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookManager.SearchByTitleAsync("invalid title fragment"));
        }


        [Fact]
        public async Task GetSpecificAsync_WithValidIsbn_ShouldReturnBook()
        {
            // Arrange: Add a book with the valid ISBN to the database
            var expectedBook = new Book
            {
                Title = "Test Book",
                Author = "John Doe",
                ISBN = "1234567890123", // Example ISBN
                YearPublished = 2021,
                Genre = "Fiction",
                Pages = 100,
                Price = 19.99
            };

            await _bookManager.AddAsync(expectedBook);

            // Act: Get the book by the valid ISBN
            var result = await _bookManager.GetSpecificAsync("1234567890123");

            // Assert: Verify that the expected book is returned
            Assert.NotNull(result);
            Assert.Equal(expectedBook, result);
        }


        [Fact]
        public async Task GetSpecificAsync_WithInvalidIsbn_ShouldThrowKeyNotFoundException()
        {
            // Arrange: Ensure there are no books in the database with the invalid ISBN
            // For example, clear the database or remove any existing books with the invalid ISBN
            _dbContext.Books.RemoveRange(_dbContext.Books.Where(b => b.ISBN == "invalid ISBN"));
            await _dbContext.SaveChangesAsync();

            // Act and Assert: Use Assert.ThrowsAsync to assert that a KeyNotFoundException is thrown
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _bookManager.GetSpecificAsync("invalid ISBN"));
        }


        [Fact]
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

            await _bookManager.AddAsync(initialBook); // Add the initial book to the database

            // Act: Update the book's details

            var bookToUpdate = await _bookManager.GetSpecificAsync(initialBook.ISBN);

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

            // Act: Update the book with the updated details
            await _bookManager.UpdateAsync(bookToUpdate);

            // Assert: Retrieve the book from the database and verify that it has been updated
            var updatedBookFromDb = await _bookManager.GetSpecificAsync("1234567890123");
            Assert.NotNull(updatedBookFromDb);
            Assert.Equal(updatedBook.Title, updatedBookFromDb.Title);
            Assert.Equal(updatedBook.Author, updatedBookFromDb.Author);
            Assert.Equal(updatedBook.YearPublished, updatedBookFromDb.YearPublished);
            Assert.Equal(updatedBook.Genre, updatedBookFromDb.Genre);
            Assert.Equal(updatedBook.Pages, updatedBookFromDb.Pages);
            Assert.Equal(updatedBook.Price, updatedBookFromDb.Price);
        }


        [Fact]
        public async Task UpdateAsync_WithInvalidBook_ShouldThrowValidationException()
        {
            // Arrange: Prepare an invalid book (e.g., missing required fields)
            var invalidBook = new Book(); // Invalid book without required fields

            // Act and Assert: Use Assert.ThrowsAsync to assert that a ValidationException is thrown
            await Assert.ThrowsAsync<ValidationException>(() => _bookManager.UpdateAsync(invalidBook));
        }


    }
}
