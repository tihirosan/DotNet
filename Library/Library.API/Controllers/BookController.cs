using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Library.API.Resources;
using Library.API.Validators;
using Microsoft.AspNetCore.Mvc;
using Library.Domain.Models;
using Library.Domain.Services;

namespace Library.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        
        public BooksController(IBookService bookService, IMapper mapper)
        {
            _mapper = mapper;
            _bookService = bookService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookResource>> GetBookById(int id)
        {
            var book = await _bookService.GetBookById(id);
            var bookResource = _mapper.Map<Book, BookResource>(book);

            return Ok(bookResource);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookResource>>> GetAllBooks()
        {
            var books = await _bookService.GetAllWithAuthor();
            var bookResources = _mapper.Map<IEnumerable<Book>, IEnumerable<BookResource>>(books);

            return Ok(bookResources);
        }

        [HttpPost]
        public async Task<ActionResult<BookResource>> CreateBook([FromBody] SaveBookResource saveBookResource)
        {
            var validator = new SaveBookResourceValidator();
            var validationResult = await validator.ValidateAsync(saveBookResource);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);
    
            var bookToCreate = _mapper.Map<SaveBookResource, Book>(saveBookResource);
            var newBook = await _bookService.CreateBook(bookToCreate);
            var bookResource = _mapper.Map<Book, BookResource>(newBook);

            return Ok(bookResource);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<BookResource>> UpdateBook(int id, [FromBody] SaveBookResource saveBookResource)
        {
            var validator = new SaveBookResourceValidator();
            var validationResult = await validator.ValidateAsync(saveBookResource);
            
            var requestIsInvalid = id == 0 || !validationResult.IsValid;
            if (requestIsInvalid)
                return BadRequest(validationResult.Errors);
            
            var book = _mapper.Map<SaveBookResource, Book>(saveBookResource);

            await _bookService.UpdateBook(id, book);

            var updatedBook = await _bookService.GetBookById(id);
            var updatedBookResource = _mapper.Map<Book, BookResource>(updatedBook);

            return Ok(updatedBookResource);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (id == 0)
                return BadRequest();
    
            var book = await _bookService.GetBookById(id);
            if (book == null)
                return NotFound();

            await _bookService.DeleteBook(book);

            return NoContent();
        }
    }
}