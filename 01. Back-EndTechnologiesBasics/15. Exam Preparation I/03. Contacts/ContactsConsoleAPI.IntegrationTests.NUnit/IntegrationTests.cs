using ContactsConsoleAPI.Business;
using ContactsConsoleAPI.Business.Contracts;
using ContactsConsoleAPI.Data.Models;
using ContactsConsoleAPI.DataAccess;
using ContactsConsoleAPI.DataAccess.Contrackts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsConsoleAPI.IntegrationTests.NUnit
{
    public class IntegrationTests
    {
        private TestContactDbContext dbContext;
        private IContactManager contactManager;

        [SetUp]
        public void SetUp()
        {
            this.dbContext = new TestContactDbContext();
            this.contactManager = new ContactManager(new ContactRepository(this.dbContext));
        }


        [TearDown]
        public void TearDown()
        {
            this.dbContext.Database.EnsureDeleted();
            this.dbContext.Dispose();
        }


        //positive test
        [Test]
        public async Task AddContactAsync_ShouldAddNewContact()
        {
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };

            await contactManager.AddAsync(newContact);

            var dbContact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Contact_ULID == newContact.Contact_ULID);

            Assert.NotNull(dbContact);
            Assert.AreEqual(newContact.FirstName, dbContact.FirstName);
            Assert.AreEqual(newContact.LastName, dbContact.LastName);
            Assert.AreEqual(newContact.Phone, dbContact.Phone);
            Assert.AreEqual(newContact.Email, dbContact.Email);
            Assert.AreEqual(newContact.Address, dbContact.Address);
            Assert.AreEqual(newContact.Contact_ULID, dbContact.Contact_ULID);
        }

        //Negative test
        [Test]
        public async Task AddContactAsync_TryToAddContactWithInvalidCredentials_ShouldThrowException()
        {
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH", //must be minimum 10 symbols - numbers or Upper case letters
                Email = "invalid_Mail", //invalid email
                Gender = "Male",
                Phone = "0889933779"
            };

            var ex = Assert.ThrowsAsync<ValidationException>(async () => await contactManager.AddAsync(newContact));
            var actual = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Contact_ULID == newContact.Contact_ULID);

            Assert.IsNull(actual);
            Assert.That(ex?.Message, Is.EqualTo("Invalid contact!"));

        }

        [Test]
        public async Task DeleteContactAsync_WithValidULID_ShouldRemoveContactFromDb()
        {
            // Arrange
            var newContact = new Contact()
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Address = "Anything for testing address",
                Contact_ULID = "1ABC23456HH",
                Email = "test@gmail.com",
                Gender = "Male",
                Phone = "0889933779"
            };

            await contactManager.AddAsync(newContact);

            // Act
            await contactManager.DeleteAsync(newContact.Contact_ULID);

            // Assert
            var dbContact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Contact_ULID == newContact.Contact_ULID);
            Assert.Null(dbContact);
        }

        [Test]
        public async Task DeleteContactAsync_TryToDeleteWithNullOrWhiteSpaceULID_ShouldThrowException()
        {
            // Arrange
            string ulid = "";

            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await contactManager.DeleteAsync(ulid));
            Assert.That(ex?.Message, Is.EqualTo("ULID cannot be empty."));
        }

        [Test]
        public async Task GetAllAsync_WhenContactsExist_ShouldReturnAllContacts()
        {
            // Arrange
            var contact1 = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Contact_ULID = "ABC123",
                Email = "john.doe@example.com",
                Gender = "Male",
                Phone = "123-456-7890"
            };
            var contact2 = new Contact()
            {
                FirstName = "Jane",
                LastName = "Smith",
                Address = "456 Elm St",
                Contact_ULID = "DEF456",
                Email = "jane.smith@example.com",
                Gender = "Female",
                Phone = "987-654-3210"
            };
            await dbContext.Contacts.AddRangeAsync(contact1, contact2);
            await dbContext.SaveChangesAsync();

            // Act
            var contacts = await contactManager.GetAllAsync();

            // Assert
            Assert.NotNull(contacts);
            Assert.AreEqual(2, contacts.Count());
        }

        [Test]
        public async Task GetAllAsync_WhenNoContactsExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await contactManager.GetAllAsync());
            Assert.That(ex?.Message, Is.EqualTo("No contact found."));
        }

        [Test]
        public async Task SearchByFirstNameAsync_WithExistingFirstName_ShouldReturnMatchingContacts()
        {
            // Arrange
            var contact1 = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Contact_ULID = "ABC123",
                Email = "john.doe@example.com",
                Gender = "Male",
                Phone = "123-456-7890"
            };
            var contact2 = new Contact()
            {
                FirstName = "John",
                LastName = "Smith",
                Address = "456 Elm St",
                Contact_ULID = "DEF456",
                Email = "john.smith@example.com",
                Gender = "Male",
                Phone = "987-654-3210"
            };
            await dbContext.Contacts.AddRangeAsync(contact1, contact2);
            await dbContext.SaveChangesAsync();

            // Act
            var contacts = await contactManager.SearchByFirstNameAsync("John");

            // Assert
            Assert.NotNull(contacts);
            Assert.AreEqual(2, contacts.Count());
        }

        [Test]
        public async Task SearchByFirstNameAsync_WithNonExistingFirstName_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await contactManager.SearchByFirstNameAsync("NonExistingFirstName"));
            Assert.That(ex?.Message, Is.EqualTo("No contact found with the given first name."));
        }

        [Test]
        public async Task SearchByLastNameAsync_WithExistingLastName_ShouldReturnMatchingContacts()
        {
            // Arrange
            var contact1 = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Contact_ULID = "ABC123",
                Email = "john.doe@example.com",
                Gender = "Male",
                Phone = "123-456-7890"
            };
            var contact2 = new Contact()
            {
                FirstName = "Jane",
                LastName = "Doe",
                Address = "456 Elm St",
                Contact_ULID = "DEF456",
                Email = "jane.doe@example.com",
                Gender = "Female",
                Phone = "987-654-3210"
            };
            await dbContext.Contacts.AddRangeAsync(contact1, contact2);
            await dbContext.SaveChangesAsync();

            // Act
            var contacts = await contactManager.SearchByLastNameAsync("Doe");

            // Assert
            Assert.NotNull(contacts);
            Assert.AreEqual(2, contacts.Count());
        }

        [Test]
        public async Task SearchByLastNameAsync_WithNonExistingLastName_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await contactManager.SearchByLastNameAsync("NonExistingLastName"));
            Assert.That(ex?.Message, Is.EqualTo("No contact found with the given last name."));
        }

        [Test]
        public async Task GetSpecificAsync_WithValidULID_ShouldReturnContact()
        {
            // Arrange
            var contact = new Contact()
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                Contact_ULID = "ABC123",
                Email = "john.doe@example.com",
                Gender = "Male",
                Phone = "123-456-7890"
            };
            await dbContext.Contacts.AddAsync(contact);
            await dbContext.SaveChangesAsync();

            // Act
            var retrievedContact = await contactManager.GetSpecificAsync("ABC123");

            // Assert
            Assert.NotNull(retrievedContact);
            Assert.AreEqual(contact.FirstName, retrievedContact.FirstName);
            Assert.AreEqual(contact.LastName, retrievedContact.LastName);
            Assert.AreEqual(contact.Address, retrievedContact.Address);
            Assert.AreEqual(contact.Contact_ULID, retrievedContact.Contact_ULID);
            Assert.AreEqual(contact.Email, retrievedContact.Email);
            Assert.AreEqual(contact.Gender, retrievedContact.Gender);
            Assert.AreEqual(contact.Phone, retrievedContact.Phone);
        }

        [Test]
        public async Task GetSpecificAsync_WithInvalidULID_ShouldThrowKeyNotFoundException()
        {
            // Arrange

            // Act & Assert
            var ex = Assert.ThrowsAsync<KeyNotFoundException>(async () => await contactManager.GetSpecificAsync("InvalidULID"));
            Assert.That(ex?.Message, Is.EqualTo("No contact found with ULID: InvalidULID"));
        }

        [Test]
        public async Task UpdateAsync_WithValidContact_ShouldUpdateContact()
        {
            // Arrange
            var updatedContact = new Contact
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Address = "Updated address",
                Contact_ULID = "01HN8RHJVVD4D15K5DT7GW6N8G",
                Email = "updated_email@gmail.com",
                Gender = "Female",
                Phone = "1234567890"
            };

            // Act
            await contactManager.UpdateAsync(updatedContact);

            // Assert
            var retrievedContact = await dbContext.Contacts.FirstOrDefaultAsync(c => c.Contact_ULID == updatedContact.Contact_ULID);
            Assert.NotNull(retrievedContact);
            Assert.AreEqual(updatedContact.FirstName, retrievedContact.FirstName);
            Assert.AreEqual(updatedContact.LastName, retrievedContact.LastName);
            Assert.AreEqual(updatedContact.Phone, retrievedContact.Phone);
            Assert.AreEqual(updatedContact.Email, retrievedContact.Email);
            Assert.AreEqual(updatedContact.Address, retrievedContact.Address);
            Assert.AreEqual(updatedContact.Contact_ULID, retrievedContact.Contact_ULID);
        }

        [Test]
        public async Task UpdateAsync_WithInvalidContact_ShouldThrowValidationException()
        {
            // Arrange
            var updatedContact = new Contact
            {
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Address = "Updated address",
                Contact_ULID = "01HN8RHJVVD4D15K5DT7GW6N8G",
                Email = "invalid_email", // Invalid email format
                Gender = "Female",
                Phone = "1234567890"
            };

            // Act & Assert
            var ex = Assert.ThrowsAsync<ValidationException>(async () => await contactManager.UpdateAsync(updatedContact));
            Assert.That(ex?.Message, Is.EqualTo("Invalid contact!"));
        }
    }
}
