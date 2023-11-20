using System.Linq.Expressions;
using MongoDBCRUD.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBCRUD.Services;

public class DesignerService
{
    private readonly IMongoCollection<DesignersCollection> _designerCollection;
    private readonly IMongoCollection<TrainersCollection> _trainersCollection;

    public DesignerService(
        IOptions<DesignerDatabaseSettings> designerDatabaseSettings)

    {
        var mongoClient = new MongoClient(
            designerDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            designerDatabaseSettings.Value.DatabaseName);
        
        _designerCollection = mongoDatabase.GetCollection<DesignersCollection>(
            designerDatabaseSettings.Value.DesignersCollectionName);
        
        _trainersCollection = mongoDatabase.GetCollection<TrainersCollection>(
            designerDatabaseSettings.Value.TrainersCollectionName);

    }

    public async Task<List<DesignersCollection>> GetDesigner() =>
        await _designerCollection.Find(_ => true).ToListAsync();

    public async Task<DesignersCollection?> GetDesignerById(string id) =>
        await _designerCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<DesignersCollection?> GetDesignerByName(string name) =>
        await _designerCollection.Find(x => x.DesignerOfTrainer == name).FirstOrDefaultAsync();
    public async Task CreateDesigner(DesignersCollection newDesigner) =>
        await _designerCollection.InsertOneAsync(newDesigner);

    public async Task UpdateDesigner(string id, DesignersCollection updateDesigner) =>
        await _designerCollection.ReplaceOneAsync(x => x.Id == id, updateDesigner);

    public async Task RemoveDesigner(string id) =>
        await _designerCollection.DeleteOneAsync(x => x.Id == id);
    
    public async Task<bool> DeleteDesignerCascadeAsync(string designerName)
    {
        var designerFilter = Builders<DesignersCollection>.Filter.Eq(a => a.DesignerOfTrainer, designerName);
        var deleteResult = await _designerCollection.DeleteOneAsync(designerFilter);
        
        if (designerFilter != null)
        {
            var trainersFilter = Builders<TrainersCollection>.Filter.AnyEq(b => b.Designers, designerName);
            //var update = Builders<TrainersCollection>.Update.Pull(b => b.Designers, designerName);
            //var updateResult = await _trainersCollection.UpdateManyAsync(trainersFilter, update);
            var updateResult = await _trainersCollection.DeleteManyAsync(trainersFilter);
            
            return updateResult.DeletedCount > 0;
        }

        return false;
    }
    
    /*public async Task<bool> DeleteDesignerCascadeAsync(string designerName)
    {
        var designerFilter = Builders<DesignersCollection>.Filter.Eq(a => a.DesignerOfTrainer, designerName);

        if (designerFilter != null)
        {
            var trainersFilter = Builders<TrainersCollection>.Filter.AnyEq(b => b.Designers, designerName);

            var update = Builders<TrainersCollection>.Update.PullFilter(b => b.Designers, Builders<string>.Filter.Eq(d => d, designerName));
            var updateResult = await _trainersCollection.UpdateManyAsync(trainersFilter, update);

            var deleteTrainersResult = await _trainersCollection.DeleteManyAsync(trainersFilter);
            var deleteDesignersResult = await _designerCollection.DeleteOneAsync(designerFilter);

            return updateResult.ModifiedCount > 0 || deleteTrainersResult.DeletedCount > 0 || deleteDesignersResult.DeletedCount > 0;
        }

        return false;
    }*/




    public async Task<Dictionary<string, int>> CountTrainersPerAuthorAsync()
    {
        var trainers = await _trainersCollection.Find(new BsonDocument()).ToListAsync();
        var designerCounts = new Dictionary<string, int>();

        foreach (var trainer in trainers)
        {
            foreach (var designer in trainer.Designers)
            {
                if (designerCounts.ContainsKey(designer))
                {
                    designerCounts[designer] += int.Parse(trainer.TrainerPrice);
                }
                else
                {
                    designerCounts[designer] = int.Parse(trainer.TrainerPrice);
                }
            }
        }

        return designerCounts;
    }
    
    
}
