using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.Domain.Models;
using Library.Domain.Repositories;

namespace Library.DAL.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(LibraryDbContext context) : base(context) { }

        public async Task<Book> GetWithAuthorByIdAsync(int id)
        {
            return await MyBookDbContext.Books.Include(m => m.Author)
                                                .SingleOrDefaultAsync(m => m.Id == id);;
        }
        
        public async Task<IEnumerable<Book>> GetAllWithAuthorAsync()
        {
            return await MyBookDbContext.Books.Include(m => m.Author)
                                                .ToListAsync();
        }
        

        public async Task<IEnumerable<Book>> GetAllWithAuthorByAuthorIdAsync(int AuthorId)
        {
            return await MyBookDbContext.Books.Include(m => m.Author)
                                                .Where(m => m.AuthorId == AuthorId)
                                                .ToListAsync();
        }
        
        public async Task<bool> IsExists(int id)
        {
            return await GetByIdAsync(id) is {};
        }
        
        private LibraryDbContext MyBookDbContext => Context as LibraryDbContext;
    }
}