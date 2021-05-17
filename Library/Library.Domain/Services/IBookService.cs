using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Domain.Models;

namespace Library.Domain.Services
{
    public interface IBookService
    {
        Task<Book> CreateBook(Book newBook);
        Task<Book> GetBookById(int id);
        Task<IEnumerable<Book>> GetAllWithAuthor();
        Task<IEnumerable<Book>> GetBooksByAuthorId(int authorId);
        Task UpdateBook(int id, Book book);
        Task DeleteBook(Book book);
    }
}