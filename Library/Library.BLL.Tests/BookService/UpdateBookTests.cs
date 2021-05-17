using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Moq;
using Library.Domain;
using Library.Domain.Models;
using Library.Domain.Repositories;
using NUnit.Framework;

namespace Library.BLL.Tests
{
    [TestFixture]
    public class UpdateBookTests
    {
        private static (Mock<IUnitOfWork> unitOfWork, Mock<IBookRepository> BookRepo, Dictionary<int, Book> dbCollectionBook) GetMocks()
        {
            var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            var bookRepo = new Mock<IBookRepository>(MockBehavior.Strict);
            var authorRepo = new Mock<IAuthorRepository>(MockBehavior.Strict);
            var dbCollectionBook = new Dictionary<int, Book>
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
            
            var dbCollectionAuthors = new Dictionary<int, Author>
            {
                [26] = new Author
                {
                    Id = 26,
                    Name = "Pushkin"
                },
                [27] = new Author
                {
                    Id = 27,
                    Name = "Lermontov"
                }
            };

            unitOfWork.SetupGet(e => e.Books).Returns(bookRepo.Object);
            unitOfWork.SetupGet(e => e.Authors).Returns(authorRepo.Object);
            unitOfWork.Setup(e => e.CommitAsync()).ReturnsAsync(0);
            
            bookRepo.Setup(e => e.GetWithAuthorByIdAsync(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollectionBook[id]);
            bookRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollectionBook.ContainsKey(id));
            
            authorRepo.Setup(e => e.GetByIdAsync(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollectionAuthors[id]);
            authorRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollectionAuthors.ContainsKey(id));

            return (unitOfWork, bookRepo, dbCollectionBook);
        }
        
        [Test]
        public async Task UpdateBook_FullInfo_Success()
        {
            // Arrange
            var (unitOfWork, bookRepo, dbCollectionBook)  = GetMocks();
            var service = new BookService(unitOfWork.Object);
            var book = new Book
            {
                AuthorId = 27,
                Title = "test"
            };
        
            // Act
            await service.UpdateBook(27, book);
            
            // Assert
            Assert.AreEqual((await unitOfWork.Object.Books.GetWithAuthorByIdAsync(27)).Title, book.Title);
        }
        
        [Test]
        public void UpdateBook_EmptyName_InvalidDataException()
        {
            // Arrange
            var (unitOfWork, bookRepo, dbCollectionBook)  = GetMocks();
            var service = new BookService(unitOfWork.Object);
            var book = new Book()
            {
                Title = "test"
            };
            
            // Act + Assert
            Assert.ThrowsAsync<InvalidDataException>(async () => await service.UpdateBook(27, book));
        }
        
        [Test]
        public void UpdateBook_NoItemForUpdate_NullReferenceException()
        {
            // Arrange
            var (unitOfWork, bookRepo, dbCollectionBook)  = GetMocks();
            var service = new BookService(unitOfWork.Object);
            var book = new Book()
            {
                Title = "test"
            };
            
            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.UpdateBook(0, book));
        }
    }
}