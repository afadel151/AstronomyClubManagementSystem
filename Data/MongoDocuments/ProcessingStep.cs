using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.MongoDocuments;


public class ProcessingStep
{
    [BsonElement("seq")]    public int    Seq    { get; set; }
    [BsonElement("step")]   public string Step   { get; set; } = null!;
    [BsonElement("tool")]   public string Tool   { get; set; } = null!;
    [BsonElement("notes")]  public string Notes  { get; set; } = null!;
}