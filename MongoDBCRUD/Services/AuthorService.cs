using MongoDBCRUD.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCRUD.Services;

public class AuthorService
{
    private readonly IMongoCollection<AuthorCollection> _authorCollection;
    private readonly IMongoCollection<BooksCollection> _booksCollection;

    public AuthorService(
        IOptions<AuthorDatabaseSettings> authorDatabaseSettings)

    {
        var mongoClient = new MongoClient(
            authorDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            authorDatabaseSettings.Value.DatabaseName);
        
        _authorCollection = mongoDatabase.GetCollection<AuthorCollection>(
            authorDatabaseSettings.Value.AuthorsCollectionName);

    }

    public async Task<List<AuthorCollection>> GetAsync() =>
        await _authorCollection.Find(_ => true).ToListAsync();

    public async Task<AuthorCollection?> GetAsync(string id) =>
        await _authorCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(AuthorCollection newAuthor) =>
        await _authorCollection.InsertOneAsync(newAuthor);

    public async Task UpdateAsync(string id, AuthorCollection updateAuthor) =>
        await _authorCollection.ReplaceOneAsync(x => x.Id == id, updateAuthor);

    public async Task RemoveAsync(string id) =>
        await _authorCollection.DeleteOneAsync(x => x.Id == id);
    
    // public void DeleteAuthorWithCascade(string? authorId)
    // {
    //     // First, find the author
    //     var author = _authorCollection.Find(author => author.Id == authorId).FirstOrDefault();
    //
    //     if (author != null)
    //     {
    //         // Delete the author
    //         _authorCollection.DeleteOne(a => a.Id == authorId);
    //
    //         // Iterate through the books and remove the author's ObjectId
    //         var filter = Builders<BooksCollection>.Filter.Eq("author", authorId);
    //         var update = Builders<BooksCollection>.Update.Pull("author", authorId);
    //         _booksCollection.UpdateMany(filter, update);
    //     }
    // }
    
    // public void DeleteAuthorWithCascade(string authorId)
    // {
    //     if (!ObjectId.TryParse(authorId, out ObjectId authorObjectId))
    //     {
    //         return; // Invalid authorId format, handle this as needed
    //     }
    //
    //     // First, delete the author from AuthorsCollection
    //     _authorCollection.DeleteOne(a => a.Id == authorObjectId);
    //
    //     // Then, remove the author's ObjectId from the books in BooksCollection
    //     var filter = Builders<BooksCollection>.Filter.ElemMatch("author", authorObjectId);
    //     var update = Builders<BooksCollection>.Update.Pull("author", authorObjectId);
    //     _booksCollection.UpdateMany(filter, update);
    // }
    
}
