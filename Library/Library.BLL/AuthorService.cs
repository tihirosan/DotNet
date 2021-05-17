using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Library.Domain;
using Library.Domain.Models;
using Library.Domain.Services;

namespace Library.BLL
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public AuthorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Author> CreateAuthor(Author newAuthor)
        {
            if (newAuthor is null)
                throw new NullReferenceException();
            
            await _unitOfWork.Authors.AddAsync(newAuthor);
            await _unitOfWork.CommitAsync();
            
            return newAuthor;
        }
        
        public async Task<Author> GetAuthorById(int id)
        {
            return await _unitOfWork.Authors.GetByIdAsync(id);
        }
        
        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _unitOfWork.Authors.GetAllAsync();
        }

        public async Task UpdateAuthor(int id, Author author)
        {
            if (!await _unitOfWork.Authors.IsExists(id))
                throw new NullReferenceException();
            
            if (author.Name.Length == 0 || author.Name.Length > 50)
                throw new InvalidDataException();

            var authorToBeUpdated = await GetAuthorById(id);
            authorToBeUpdated.Name = author.Name;
            
            await _unitOfWork.CommitAsync();
        }
        
        public async Task DeleteAuthor(Author author)
        {
            if (!await _unitOfWork.Authors.IsExists(author.Id))
                throw new NullReferenceException();
            
            _unitOfWork.Authors.Remove(author);
            
            await _unitOfWork.CommitAsync();
        }
    }
}