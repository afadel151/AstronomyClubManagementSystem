using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Storage;

public interface IStorageService
{
    Task<string> UploadAsync(string bucket, string objectKey, Stream data,
                             long size, string contentType,
                             Dictionary<string, string>? metadata = null);
    Task<string> GetPresignedUrlAsync(string bucket, string objectKey, int expirySeconds = 3600);
    Task DeleteAsync(string bucket, string objectKey);
}

public class MinioStorageService(IMinioClient minio) : IStorageService
{
    private readonly IMinioClient _minio = minio;

    public async Task<string> UploadAsync(
        string bucket, string objectKey, Stream data,
        long size, string contentType,
        Dictionary<string, string>? metadata = null)
    {
        // Ensure bucket exists
        bool exists = await _minio.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucket));
        if (!exists)
            await _minio.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket));

        var args = new PutObjectArgs()
            .WithBucket(bucket)
            .WithObject(objectKey)
            .WithStreamData(data)
            .WithObjectSize(size)
            .WithContentType(contentType);

        if (metadata != null)
            args = args.WithHeaders(metadata);

        await _minio.PutObjectAsync(args);

        return $"http://localhost:9000/{bucket}/{objectKey}";
    }

    public async Task<string> GetPresignedUrlAsync(
        string bucket, string objectKey, int expirySeconds = 3600)
    {
        return await _minio.PresignedGetObjectAsync(
            new PresignedGetObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectKey)
                .WithExpiry(expirySeconds));
    }

    public async Task DeleteAsync(string bucket, string objectKey)
    {
        await _minio.RemoveObjectAsync(
            new RemoveObjectArgs().WithBucket(bucket).WithObject(objectKey));
    }
}