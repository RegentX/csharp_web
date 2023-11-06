using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBCRUD.Models;

public class AuthorCollection
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("author")]
    [JsonPropertyName("author")]
    public string AuthorOfBook { get; set; } = null!;
}