namespace MongoDBCRUD.Models;

public class DesignerDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;
    
    public string DesignersCollectionName { get; set; } = null!;
    
    public string TrainersCollectionName { get; set; } = null!;
}