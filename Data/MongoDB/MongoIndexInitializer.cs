using Data.MongoDocuments;
using MongoDB.Driver;

namespace Data.MongoDB;

public static class MongoIndexInitializer
{
    public static async Task InitializeAsync(MongoDbContext ctx)
    {
        // observation_detail
        await ctx.ObservationDetails.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<ObservationDetail>(
                Builders<ObservationDetail>.IndexKeys.Ascending(x => x.ObservationId),
                new CreateIndexOptions { Unique = true, Name = "IX_observation_id" })
        });

        // image_document
        await ctx.ImageDocuments.Indexes.CreateManyAsync(new[]
        {
            new CreateIndexModel<ImageDocument>(
                Builders<ImageDocument>.IndexKeys.Ascending(x => x.ImageId),
                new CreateIndexOptions { Unique = true }),
            new CreateIndexModel<ImageDocument>(
                Builders<ImageDocument>.IndexKeys.Ascending("fits_header.FILTER")),
            new CreateIndexModel<ImageDocument>(
                Builders<ImageDocument>.IndexKeys.Ascending("tags"))
        });
    }
}