namespace MongoDBCRUD.Models;

public class TrainerDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string TrainersCollectionName { get; set; } = null!;
    
    public string DesignersCollectionName { get; set; } = null!;
}