using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.MongoDocuments;
public class ImageDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = null!;            // = IMAGE_RECORDS.Code

    [BsonElement("image_id")]
    public int ImageId { get; set; }

    [BsonElement("fits_header")]
    public BsonDocument FitsHeader { get; set; } = new();

    [BsonElement("wcs")]
    public WcsDocument? Wcs { get; set; }

    [BsonElement("processing")]
    public ProcessingDocument? Processing { get; set; }

    [BsonElement("tags")]
    public List<string> Tags { get; set; } = new();
}