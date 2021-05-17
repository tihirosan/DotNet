using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Library.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Library.Domain.Models;
using Library.Domain.Services;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        private readonly AbstractValidator<SaveAuthorResource> _validator;
        
        public AuthorsController(IAuthorService authorService, IMapper mapper, AbstractValidator<SaveAuthorResource> validator)
        {
            _mapper = mapper;
            _authorService = authorService;
            _validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorResource>>> GetAllAuthors()
        {
            var authors = await _authorService.GetAllAuthors();
            var authorResources = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorResource>>(authors);

            return Ok(authorResources);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorResource>> GetAuthorById(int id)
        {
            var author = await _authorService.GetAuthorById(id);
            var authorResource = _mapper.Map<Author, AuthorResource>(author);

            return Ok(authorResource);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorResource>> CreateAuthor([FromBody] SaveAuthorResource saveAuthorResource)
        {
            var validationResult = await _validator.ValidateAsync(saveAuthorResource);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var authorToCreate = _mapper.Map<SaveAuthorResource, Author>(saveAuthorResource);
            var newAuthor = await _authorService.CreateAuthor(authorToCreate);
            var authorResource = _mapper.Map<Author, AuthorResource>(newAuthor);

            return Ok(authorResource);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AuthorResource>> UpdateAuthor(int id, [FromBody] SaveAuthorResource saveAuthorResource)
        {
            var validationResult = await _validator.ValidateAsync(saveAuthorResource);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
            
            var author = _mapper.Map<SaveAuthorResource, Author>(saveAuthorResource);

            await _authorService.UpdateAuthor(id, author);

            var updatedAuthor = await _authorService.GetAuthorById(id);
            var updatedAuthorResource = _mapper.Map<Author, AuthorResource>(updatedAuthor);

            return Ok(updatedAuthorResource);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorService.GetAuthorById(id);

            await _authorService.DeleteAuthor(author);
            
            return NoContent();
        }
    }
}