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
    public class GetBookByIdTests
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
            
            authorRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollectionAuthors.ContainsKey(id));

            return (unitOfWork, bookRepo, dbCollectionBook);
        }
        
        [Test]
        public async Task GetBookById_ItemExists_Success()
        {
            // Arrange
            var (unitOfWork, bookRepo, dbCollectionBook) = GetMocks();
            var service = new BookService(unitOfWork.Object);

            // Act
            var book = await service.GetBookById(27);
            
            // Assert
            Assert.AreEqual(book, dbCollectionBook[27]);
        }
        
        [Test]
        public void GetBookById_ItemDoesNotExists_KeyNotFoundException()
        {
            // Arrange
            var (unitOfWork, bookRepo, dbCollectionBook) = GetMocks();
            var service = new BookService(unitOfWork.Object);

            // Act + Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.GetBookById(0));
        }
    }
}