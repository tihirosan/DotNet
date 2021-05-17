using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Library.Domain;
using Library.Domain.Models;
using Library.Domain.Services;

namespace Library.BLL
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Book> CreateBook(Book newBook)
        {
            if (newBook is null)
                throw new NullReferenceException();
            
            await _unitOfWork.Books.AddAsync(newBook);
            await _unitOfWork.CommitAsync();

            return newBook;
        }

        public async Task<Book> GetBookById(int id)
        {
            return await _unitOfWork.Books.GetWithAuthorByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorId(int authorId)
        {
            return await _unitOfWork.Books.GetAllWithAuthorByAuthorIdAsync(authorId);
        }

        public async Task<IEnumerable<Book>> GetAllWithAuthor()
        {
            return await _unitOfWork.Books.GetAllWithAuthorAsync();
        }

        public async Task UpdateBook(int id, Book book)
        {
            if (!await _unitOfWork.Books.IsExists(id))
                throw new NullReferenceException();
            
            if (string.IsNullOrEmpty(book.Title) || book.AuthorId <= 0)
                throw new InvalidDataException();
            
            var bookToBeUpdated = await GetBookById(id);
            bookToBeUpdated.Title = book.Title;
            bookToBeUpdated.AuthorId = book.AuthorId;

            await _unitOfWork.CommitAsync();
        }
        
        public async Task DeleteBook(Book book)
        {
            if (!(await _unitOfWork.Books.IsExists(book.Id)))
                throw new NullReferenceException();
            
            _unitOfWork.Books.Remove(book);
            
            await _unitOfWork.CommitAsync();
        }
    }
}