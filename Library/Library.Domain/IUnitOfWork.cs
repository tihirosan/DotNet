using System;
using System.Threading.Tasks;
using Library.Domain.Repositories;

namespace Library.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        IAuthorRepository Authors { get; }
        Task<int> CommitAsync();
    }
}