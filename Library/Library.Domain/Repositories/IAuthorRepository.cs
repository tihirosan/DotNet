using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;

namespace Library.Domain.Repositories
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<Author> GetWithBooksByIdAsync(int id);
        Task<IEnumerable<Author>> GetAllWithBooksAsync();
        Task<bool> IsExists(int id);
    }
}