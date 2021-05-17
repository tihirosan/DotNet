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
    public class GetAuthorByIdTests
    {
        private static (Mock<IUnitOfWork> unitOfWork, Mock<IAuthorRepository> AuthorRepo, Dictionary<int, Author> dbCollection) GetMocks()
        {
            var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            var authorRepo = new Mock<IAuthorRepository>(MockBehavior.Strict);
            var dbCollection = new Dictionary<int, Author>
            {
                [26] = new Author
                {
                    Id = 26,
                    Name = "Delete Group"
                },
                [27] = new Author
                {
                    Id = 27,
                    Name = "Group"
                }
            };

            unitOfWork.SetupGet(e => e.Authors).Returns(authorRepo.Object);
            unitOfWork.Setup(e => e.CommitAsync()).ReturnsAsync(0);
            
            authorRepo.Setup(e => e.GetByIdAsync(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollection[id]);

            return (unitOfWork, authorRepo, dbCollection);
        }
        
        [Test]
        public async Task GetAuthorById_ItemExists_Success()
        {
            // Arrange
            var (unitOfWork, authorRepo, dbCollection) = GetMocks();
            var service = new AuthorService(unitOfWork.Object);

            // Act
            var author = await service.GetAuthorById(27);
            
            // Assert
            Assert.AreEqual(author, dbCollection[27]);
        }
        
        [Test]
        public void GetAuthorById_ItemDoesNotExists_KeyNotFoundException()
        {
            // Arrange
            var (unitOfWork, authorRepo, dbCollection) = GetMocks();
            var service = new AuthorService(unitOfWork.Object);

            // Act + Assert
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.GetAuthorById(0));
        }
    }
}