using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Data.MongoDocuments;
public class WcsDocument
{
    [BsonElement("solved")]       public bool Solved     { get; set; }
    [BsonElement("solver")]       public string Solver   { get; set; } = null!;
    [BsonElement("CRVAL1")]       public double CrVal1   { get; set; }
    [BsonElement("CRVAL2")]       public double CrVal2   { get; set; }
    [BsonElement("pixel_scale_arcsec")] public double PixelScaleArcsec { get; set; }
    [BsonElement("field_width_deg")]    public double FieldWidthDeg    { get; set; }
    [BsonElement("rotation_deg")]       public double RotationDeg      { get; set; }
}