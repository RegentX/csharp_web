using MongoDBCRUD.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCRUD.Services;

public class BooksService
{
    private readonly IMongoCollection<BooksCollection> _booksCollection;
    //private readonly IMongoCollection<AuthorCollection> _authorCollection;

    public BooksService(
        IOptions<BooksDatabaseSettings> booksDatabaseSettings)

    {
        var mongoClient = new MongoClient(
            booksDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            booksDatabaseSettings.Value.DatabaseName);

        _booksCollection = mongoDatabase.GetCollection<BooksCollection>(
            booksDatabaseSettings.Value.BooksCollectionName);
    }

    public async Task<List<BooksCollection>> GetBooks() =>
        await _booksCollection.Find(_ => true).ToListAsync();

    public async Task<BooksCollection?> GetBookById(string id) =>
        await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateBook(BooksCollection newBook) =>
        await _booksCollection.InsertOneAsync(newBook);

    public async Task UpdateBook(string id, BooksCollection updatedBook) =>
        await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveBook(string id) =>
        await _booksCollection.DeleteOneAsync(x => x.Id == id);

    public async Task<Dictionary<string, long>> GetTotalStudentsWithBooks(string bookName, string bookName2,
        string bookName3, string bookName4)
    {
        var matchStage1 = new BsonDocument("$match", new BsonDocument("name", bookName));
        var matchStage2 = new BsonDocument("$match", new BsonDocument("name", bookName2));
        var matchStage3 = new BsonDocument("$match", new BsonDocument("name", bookName3));
        var matchStage4 = new BsonDocument("$match", new BsonDocument("name", bookName4));

        var groupStage = new BsonDocument("$group", new BsonDocument
        {
            { "_id", "null" },
            { "students", new BsonDocument("$sum", 1) }
        });

        var pipeline1 = new[] { matchStage1, groupStage };
        var pipeline2 = new[] { matchStage2, groupStage };
        var pipeline3 = new[] { matchStage3, groupStage };
        var pipeline4 = new[] { matchStage4, groupStage };

        var result1 = await _booksCollection.AggregateAsync<BsonDocument>(pipeline1);
        var result2 = await _booksCollection.AggregateAsync<BsonDocument>(pipeline2);
        var result3 = await _booksCollection.AggregateAsync<BsonDocument>(pipeline3);
        var result4 = await _booksCollection.AggregateAsync<BsonDocument>(pipeline4);

        var totalStudents1 = result1.FirstOrDefault()?.GetValue("students", 0).ToDouble();
        var totalStudents2 = result2.FirstOrDefault()?.GetValue("students", 0).ToDouble();
        var totalStudents3 = result3.FirstOrDefault()?.GetValue("students", 0).ToDouble();
        var totalStudents4 = result4.FirstOrDefault()?.GetValue("students", 0).ToDouble();

        var studentCounts = new Dictionary<string, long>
        {
            { bookName, Convert.ToInt64(totalStudents1) },
            { bookName2, Convert.ToInt64(totalStudents2) },
            { bookName3, Convert.ToInt64(totalStudents3) },
            { bookName4, Convert.ToInt64(totalStudents4) }
        };

        return studentCounts;
    }
}
