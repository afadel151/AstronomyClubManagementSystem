using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.MongoDocuments;
public class ProcessingDocument
{
    [BsonElement("software")]            public List<string> Software           { get; set; } = new();
    [BsonElement("total_integration_s")] public double TotalIntegrationS       { get; set; }
    [BsonElement("stacked_frames")]      public int StackedFrames               { get; set; }
    [BsonElement("steps")]               public List<ProcessingStep> Steps      { get; set; } = new();
}