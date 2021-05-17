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
    public class DeleteBookTests
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
            
            bookRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                     .ReturnsAsync((int id) => dbCollectionBook.ContainsKey(id));
            bookRepo.Setup(e => e.Remove(It.IsAny<Book>()))
                     .Callback((Book newBook) => { dbCollectionBook.Remove(newBook.Id); });
            
            authorRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollectionAuthors.ContainsKey(id));
            authorRepo.Setup(e => e.Remove(It.IsAny<Author>()))
                      .Callback((Author newAuthor) => { dbCollectionAuthors.Remove(newAuthor.Id); });

            return (unitOfWork, bookRepo, dbCollectionBook);
        }

        [Test]
        public async Task DeleteBook_TargetItem_Success()
        {
            // Arrange
            var (unitOfWork, bookRepo, dbCollectionBook) = GetMocks();
            var service = new BookService(unitOfWork.Object);
            var book = new Book
            {
                Id = 26,
                Title = "test"
            };

            // Act
            await service.DeleteBook(book);
            
            // Assert
            Assert.IsFalse(dbCollectionBook.ContainsKey(26));
        }

        [Test]
        public void DeleteBook_ItemDoesNotExists_NullReferenceException()
        {
            // Arrange
            var (unitOfWork, bookRepo, dbCollectionBook) = GetMocks();
            var service = new BookService(unitOfWork.Object);
            var book = new Book
            {
                Id = 0,
                Title = "test"
            };

            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.DeleteBook(book));
        }
    }
}