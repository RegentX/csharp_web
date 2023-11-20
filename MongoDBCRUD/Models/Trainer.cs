using System.Collections;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBCRUD.Models;

public class TrainersCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("model")]
    [JsonPropertyName("model")]
    public string ModelName { get; set; } = null!;

    [BsonElement("designer")]
    [JsonPropertyName("designer")]
    public List<string> Designers { get; set; } = new List<string>();
    
    [BsonElement("size")]
    [JsonPropertyName("size")]
    public int TrainerSize { get; set; }
    
    [BsonElement("color")]
    [JsonPropertyName("color")]
    public string TrainerColor { get; set; } = null!;
    
    [BsonElement("price")]
    [JsonPropertyName("price")]
    public string TrainerPrice { get; set; } = null!;
    
}
