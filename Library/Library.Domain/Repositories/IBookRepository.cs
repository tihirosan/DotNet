using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;

namespace Library.Domain.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book> GetWithAuthorByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllWithAuthorAsync();
        Task<IEnumerable<Book>> GetAllWithAuthorByAuthorIdAsync(int authorId);
        Task<bool> IsExists(int id);
    }
}