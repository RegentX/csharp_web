using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBCRUD.Models;

public class DesignersCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("designer")]
    [JsonPropertyName("designer")]
    public string DesignerOfTrainer { get; set; }
}