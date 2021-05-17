using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Library.Domain.Models;
using Library.Domain.Repositories;

namespace Library.DAL.Repositories
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(LibraryDbContext context) : base(context) { }
        
        public async Task<Author> GetWithBooksByIdAsync(int id)
        {
            return await MyBookDbContext.Authors.Include(a => a.Books)
                                                 .SingleOrDefaultAsync(a => a.Id == id);
        }
        
        public async Task<IEnumerable<Author>> GetAllWithBooksAsync()
        {
            return await MyBookDbContext.Authors.Include(a => a.Books)
                                                 .ToListAsync();
        }
        
        public async Task<bool> IsExists(int id)
        {
            return await GetByIdAsync(id) is {};
        }

        private LibraryDbContext MyBookDbContext => Context as LibraryDbContext;
    }
}