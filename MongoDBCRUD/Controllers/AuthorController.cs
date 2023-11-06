using Microsoft.AspNetCore.Mvc;
using MongoDBCRUD.Services;
using MongoDBCRUD.Models;

namespace MongoDBCRUD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly AuthorService _authorService;

    public AuthorController(AuthorService authorService) =>
        _authorService = authorService;

    [HttpGet]
    public async Task<List<AuthorCollection>> Get() =>
        await _authorService.GetAsync();
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<AuthorCollection>> Get(string id)
    {
        var authorId = await _authorService.GetAsync(id);

        if (authorId is null)
        {
            return NotFound();
        }

        return authorId;
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Post(AuthorCollection newAuthor)
    {
        await _authorService.CreateAsync(newAuthor);

        return CreatedAtAction(nameof(Get), new { id = newAuthor.Id }, newAuthor);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, AuthorCollection updateAuthor)
    {
        var author = await _authorService.GetAsync(id);

        if (author is null)
        {
            return NotFound();
        }

        updateAuthor.Id = author.Id;

        await _authorService.UpdateAsync(id, updateAuthor);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var authorDelete = await _authorService.GetAsync(id);

        if (authorDelete is null)
        {
            return NotFound();
        }

        await _authorService.RemoveAsync(id);

        return NoContent();
    }
    
    [HttpDelete("{name}")]
    public async Task<IActionResult> DeleteAuthor(string name)
    {
        var result = await _authorService.DeleteAuthorCascadeAsync(name);

        if (result)
        {
            return Ok();
        }

        return NotFound();
    }



    
}