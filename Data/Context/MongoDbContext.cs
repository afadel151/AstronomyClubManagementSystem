using Data.MongoDocuments;
using MongoDB.Driver;

namespace Data.MongoDB;

public class MongoDbContext(IMongoClient client, string databaseName)
{
    private readonly IMongoDatabase _database = client.GetDatabase(databaseName);

    // One typed collection per document type
    public IMongoCollection<ObservationDetail> ObservationDetails =>
        _database.GetCollection<ObservationDetail>("observation_detail");

    public IMongoCollection<ImageDocument> ImageDocuments =>
        _database.GetCollection<ImageDocument>("image_document");

    // public IMongoCollection<EquipmentSpec> EquipmentSpecs =>
    //     _database.GetCollection<EquipmentSpec>("equipment_specs");

    // public IMongoCollection<ForecastPlan> ForecastPlans =>
    //     _database.GetCollection<ForecastPlan>("forecast_plan");
}