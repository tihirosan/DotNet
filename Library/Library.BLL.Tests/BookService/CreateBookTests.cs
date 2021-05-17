using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Library.Domain;
using Library.Domain.Models;
using Library.Domain.Repositories;
using NUnit.Framework;

namespace Library.BLL.Tests
{
    [TestFixture]
    public class CreateBookTests
    {
        private static (Mock<IUnitOfWork> unitOfWork, Mock<IBookRepository> BookRepo, Dictionary<int, Book> dbCollection) GetMocks()
        {
            var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            var bookRepo = new Mock<IBookRepository>(MockBehavior.Strict);
            var dbCollection = new Dictionary<int, Book>
            {
                [26] = new Book
                {
                    Id = 26,
                    AuthorId = 26,
                    Title = "test"
                },
                [27] = new Book
                {
                    Id = 27,
                    AuthorId = 27,
                    Title = "test"
                }
            };

            unitOfWork.SetupGet(e => e.Books).Returns(bookRepo.Object);
            unitOfWork.Setup(e => e.CommitAsync()).ReturnsAsync(0);
            
            bookRepo.Setup(e => e.AddAsync(It.IsAny<Book>()))
                     .Callback((Book newBook) => { dbCollection.Add(newBook.Id, newBook); })
                     .Returns((Book _) => Task.CompletedTask);

            return (unitOfWork, bookRepo, dbCollection);
        }
        
        [Test]
        public async Task CreateBook_FullInfo_Success()
        {
            // Arrange
            var (unitOfWork, bookRepo, dbCollection) = GetMocks();
            var service = new BookService(unitOfWork.Object);
            var book = new Book
            {
                Id = 28,
                Title = "test"
            };

            // Act
            await service.CreateBook(book);

            // Assert
            Assert.IsTrue(dbCollection.ContainsKey(book.Id));
        }
        
        [Test]
        public void CreateBook_NullObject_NullReferenceException()
        {
            // Arrange
            var (unitOfWork, bookRepo, dbCollection) = GetMocks();
            var service = new BookService(unitOfWork.Object);

            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.CreateBook(null));
        }
    }
}