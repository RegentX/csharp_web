using Microsoft.AspNetCore.Mvc;
using MongoDBCRUD.Services;
using MongoDBCRUD.Models;

namespace MongoDBCRUD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DesignerController : ControllerBase
{
    private readonly DesignerService _designerService;

    public DesignerController(DesignerService authorService) =>
        _designerService = authorService;

    [HttpGet]
    public async Task<List<DesignersCollection>> GetDesigner() =>
        await _designerService.GetDesigner();
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<DesignersCollection>> GetDesignerById(string id)
    {
        var authorId = await _designerService.GetDesignerById(id);

        if (authorId is null)
        {
            return NotFound();
        }

        return authorId;
    }
    
    
    [HttpPost]
    public async Task<IActionResult> PostDesigner(DesignersCollection newAuthor)
    {
        await _designerService.CreateDesigner(newAuthor);

        return CreatedAtAction(nameof(GetDesigner), new { id = newAuthor.Id }, newAuthor);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateDesigner(string id, DesignersCollection updateAuthor)
    {
        var author = await _designerService.GetDesignerById(id);

        if (author is null)
        {
            return NotFound();
        }

        updateAuthor.Id = author.Id;

        await _designerService.UpdateDesigner(id, updateAuthor);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteDesigner(string id)
    {
        var authorDelete = await _designerService.GetDesignerById(id);

        if (authorDelete is null)
        {
            return NotFound();
        }

        await _designerService.RemoveDesigner(id);

        return NoContent();
    }
    
    [HttpDelete("{name}")]
    public async Task<IActionResult> CascadeDeleteDesigner(string name)
    {
        var result = await _designerService.DeleteDesignerCascadeAsync(name);

        if (result)
        {
            return Ok();
        }

        return NotFound();
    }

    [HttpGet("designerCount")]
    public async Task<IActionResult> GetDesignerTrainersCount()
    {
        var result = await _designerService.CountTrainersPerAuthorAsync();
        return Ok(result);
    }

    
}