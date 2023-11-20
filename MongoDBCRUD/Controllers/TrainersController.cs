using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDBCRUD.Services;
using MongoDBCRUD.Models;

namespace MongoDBCRUD.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainersController : ControllerBase
{
    private readonly TrainersService _trainersService;

    public TrainersController(TrainersService booksService) =>
        _trainersService = booksService;

    [HttpGet]
    public async Task<List<TrainersCollection>> GetTrainers() =>
        await _trainersService.GetTrainers();
    
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<TrainersCollection>> GetTrainersById(string id)
    {
        var bookId = await _trainersService.GetTrainerById(id);

        if (bookId is null)
        {
            return NotFound();
        }

        return bookId;
    }

    [HttpPost]
    public async Task<IActionResult> PostTrainers(TrainersCollection newBook)
    {
        await _trainersService.CreateTrainer(newBook);

        return CreatedAtAction(nameof(GetTrainers), new { id = newBook.Id }, newBook);
    }
    

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateTrainers(string id, TrainersCollection updatedBook)
    {
        var book = await _trainersService.GetTrainerById(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedBook.Id = book.Id;

        await _trainersService.UpdateTrainer(id, updatedBook);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteTrainers(string id)
    {
        var bookDelete = await _trainersService.GetTrainerById(id);

        if (bookDelete is null)
        {
            return NotFound();
        }

        await _trainersService.RemoveTrainer(
            id);

        return NoContent();
    }
    
}