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
    

    [HttpGet("books/{bookName}/{bookName2}/{bookName3}/{bookName4}")]
    public async Task<IActionResult> GetTotalStudentsForBooks(string bookName, string bookName2, string bookName3, string bookName4)
    {
        try
        {
            var studentCounts = await _booksService.GetTotalStudentsWithBooks(bookName, bookName2, bookName3, bookName4);

            return Ok(studentCounts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }




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
    
    // [HttpPost]
    // public async Task<IActionResult> Post(AuthorCollection newAuthor)
    // {
    //     await _booksService.CreateAsync(newAuthor);
    //
    //     return CreatedAtAction(nameof(Get), new { id = newAuthor.Id }, newAuthor);
    // }

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
    
    // [HttpDelete("CascadeDelete/{authorName}")]
    // public async Task<IActionResult> DeleteAuthorAndRelated(string authorName)
    // {
    //     if (string.IsNullOrWhiteSpace(authorName))
    //     {
    //         return BadRequest("Where is my author?.");
    //     }
    //
    //     try
    //     {
    //         await _booksService.CascadeDelete(authorName);
    //         return Ok($"Author '{authorName}' and other's documents was delete! Congratulations! It works!.");
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, $"Error: {ex.Message}");
    //     }
    // }
}