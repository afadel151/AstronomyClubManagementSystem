using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.MongoDocuments;

public class ObservationDetail
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = null!;           // = OBSERVATIONS.ObsId

    [BsonElement("observation_id")]
    public int ObservationId { get; set; }             // SQL Server PK

    [BsonElement("observation_type")]
    public string ObservationType { get; set; } = null!;

    // Flexible body — use BsonDocument for fully dynamic content
    [BsonElement("detail")]
    public BsonDocument Detail { get; set; } = new();
}