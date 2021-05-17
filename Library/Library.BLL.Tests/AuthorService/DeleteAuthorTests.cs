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
    public class DeleteAuthorTests
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
            
            authorRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollection.ContainsKey(id));
            authorRepo.Setup(e => e.Remove(It.IsAny<Author>()))
                      .Callback((Author newAuthor) => { dbCollection.Remove(newAuthor.Id); });

            return (unitOfWork, authorRepo, dbCollection);
        }

        [Test]
        public async Task DeleteAuthor_TargetItem_Success()
        {
            // Arrange
            var (unitOfWork, authorRepo, dbCollection) = GetMocks();
            var service = new AuthorService(unitOfWork.Object);
            var author = new Author
            {
                Id = 26,
                Name = "Delete Group"
            };

            // Act
            await service.DeleteAuthor(author);
            
            // Assert
            Assert.IsFalse(dbCollection.ContainsKey(26));
        }

        [Test]
        public void DeleteAuthor_ItemDoesNotExists_NullReferenceException()
        {
            // Arrange
            var (unitOfWork, authorRepo, dbCollection) = GetMocks();
            var service = new AuthorService(unitOfWork.Object);
            var author = new Author
            {
                Id = 0,
                Name = "Delete Group"
            };

            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.DeleteAuthor(author));
        }
    }
}