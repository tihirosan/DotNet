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
    public class UpdateAuthorTests
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
            authorRepo.Setup(e => e.IsExists(It.IsAny<int>()))
                      .ReturnsAsync((int id) => dbCollection.ContainsKey(id));

            return (unitOfWork, authorRepo, dbCollection);
        }
        
        [Test]
        public async Task UpdateAuthor_FullInfo_Success()
        {
            // Arrange
            var (unitOfWork, authorRepo, dbCollection)  = GetMocks();
            var service = new AuthorService(unitOfWork.Object);
            var author = new Author
            {
                Name = "New Group"
            };
        
            // Act
            await service.UpdateAuthor(27, author);
            
            // Assert
            Assert.AreEqual((await unitOfWork.Object.Authors.GetByIdAsync(27)).Name, author.Name);
        }
        
        [Test]
        public void UpdateAuthor_EmptyName_InvalidDataException()
        {
            // Arrange
            var (unitOfWork, authorRepo, dbCollection)  = GetMocks();
            var service = new AuthorService(unitOfWork.Object);
            var author = new Author()
            {
                Name = ""
            };
            
            // Act + Assert
            Assert.ThrowsAsync<InvalidDataException>(async () => await service.UpdateAuthor(27, author));
        }
        
        [Test]
        public void UpdateAuthor_NoItemForUpdate_NullReferenceException()
        {
            // Arrange
            var (unitOfWork, authorRepo, dbCollection)  = GetMocks();
            var service = new AuthorService(unitOfWork.Object);
            var author = new Author()
            {
                Name = "Update Group"
            };
            
            // Act + Assert
            Assert.ThrowsAsync<NullReferenceException>(async () => await service.UpdateAuthor(0, author));
        }
    }
}