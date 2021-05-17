using System.Threading.Tasks;
using Library.DAL.Repositories;
using Library.Domain;
using Library.Domain.Repositories;

namespace Library.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;
        private BookRepository _BookRepository;
        private AuthorRepository _AuthorRepository;

        public UnitOfWork(LibraryDbContext context)
        {
            _context = context;
        }

        public IBookRepository Books => _BookRepository ??= new BookRepository(_context);

        public IAuthorRepository Authors => _AuthorRepository ??= new AuthorRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}