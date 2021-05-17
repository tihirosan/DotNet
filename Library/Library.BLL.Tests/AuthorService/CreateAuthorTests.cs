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
    public class CreateAuthorTests
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
            
            authorRepo.Setup(e => e.AddAsync(It.IsAny<Author>()))
                      .Callback((Author newAuthor) => { dbCollection.Add(newAuthor.Id, newAuthor); })
                      .Returns((Author _) => Task.CompletedTask);

            return (unitOfWork, authorRepo, dbCollection);
        }
        
        [Test]
        public async Task CreateAuthor_FullInfo_Success()
        {
            // Arrange
            var (unitOfWork, authorRepo, dbCollection) = GetMocks();
            var service = new AuthorService(unitOfWork.Object);
            var author = new Author
            {
                Id = 28,
                Name = "New Group"
            };

            // Act
            await service.CreateAuthor(author);

            // Assert
            Assert.IsTrue(dbCollection.ContainsKey(author.Id));
        }
        
        [Test]
        public void CreateAuthor_NullObject_NullReferenceException()
        {
            // Arrange
            var (unitOfWork, authorRepo, dbCollection) = GetMocks();
            var service = new AuthorService(unitOfWork.Object);

            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.CreateAuthor(null));
        }
    }
}