using MongoDBCRUD.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCRUD.Services;

public class TrainersService
{
    private readonly IMongoCollection<TrainersCollection> _trainersCollection;

    public TrainersService(
        IOptions<TrainerDatabaseSettings> trainersDatabaseSettings)

    {
        var mongoClient = new MongoClient(
            trainersDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            trainersDatabaseSettings.Value.DatabaseName);

        _trainersCollection = mongoDatabase.GetCollection<TrainersCollection
        >(
            trainersDatabaseSettings.Value.TrainersCollectionName);
    }

    public async Task<List<TrainersCollection>> GetTrainers() =>
        await _trainersCollection.Find(_ => true).ToListAsync();

    public async Task<TrainersCollection?> GetTrainerById(string id) =>
        await _trainersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateTrainer(TrainersCollection newTrainer) =>
        await _trainersCollection.InsertOneAsync(newTrainer);

    public async Task UpdateTrainer(string id, TrainersCollection updatedTrainer) =>
        await _trainersCollection.ReplaceOneAsync(x => x.Id == id, updatedTrainer);

    public async Task RemoveTrainer(string id) =>
        await _trainersCollection.DeleteOneAsync(x => x.Id == id);
    
}
