using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDBCRUD.Services;
using MongoDBCRUD.Models;

namespace MongoDBCRUD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BooksService _booksService;

    public BooksController(BooksService booksService) =>
        _booksService = booksService;

    [HttpGet]
    public async Task<List<BooksCollection>> Get() =>
        await _booksService.GetBooks();
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<BooksCollection>> GetBook(string id)
    {
        var bookId = await _booksService.GetBookById(id);

        if (bookId is null)
        {
            return NotFound();
        }

        return bookId;
    }

    [HttpPost]
    public async Task<IActionResult> PostBook(BooksCollection newBook)
    {
        await _booksService.CreateBook(newBook);

        return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
    }
    

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateBook(string id, BooksCollection updatedBook)
    {
        var book = await _booksService.GetBookById(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedBook.Id = book.Id;

        await _booksService.UpdateBook(id, updatedBook);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteBook(string id)
    {
        var bookDelete = await _booksService.GetBookById(id);

        if (bookDelete is null)
        {
            return NotFound();
        }

        await _booksService.RemoveBook(id);

        return NoContent();
    }
    
}