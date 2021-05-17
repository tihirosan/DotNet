using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;

namespace Library.Domain.Services
{
    public interface IAuthorService
    {
        Task<Author> CreateAuthor(Author newAuthor);
        Task<Author> GetAuthorById(int id);
        Task<IEnumerable<Author>> GetAllAuthors();
        Task UpdateAuthor(int id, Author author);
        Task DeleteAuthor(Author author);
    }
}