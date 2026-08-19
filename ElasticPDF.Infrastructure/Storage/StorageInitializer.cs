using ElasticPDF.Infrastructure.MinIO.Bucket;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Notification;
using Polly;

namespace ElasticPDF.Infrastructure.MinIO
{
    public class StorageInitializer
    {
        private readonly IMinioClient _client;

        public StorageInitializer(IMinioClientFactory factory)
        {
            _client = factory.CreateClient()
                .WithRetryPolicy(async (executeCallback) =>
                {
                    var retryPolicy = Policy
                        .Handle<HttpRequestException>(e =>
                                e.HttpRequestError is
                                    HttpRequestError.NameResolutionError or
                                    HttpRequestError.ConnectionError
                            )
                        .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

                    return await retryPolicy.ExecuteAsync(executeCallback);
                });
        }

        public async Task InitializeAsync()
            => await EnsureDocumentBucket();

        private async Task EnsureDocumentBucket()
        {
            var bucketList = await _client.ListBucketsAsync();
            if (!bucketList.Buckets.Any(b => b.Name == DocumentBucket.Name))
            {
                var makeArgs = new MakeBucketArgs().WithBucket(DocumentBucket.Name);
                await _client.MakeBucketAsync(makeArgs).ConfigureAwait(false);
            }

            await EnsureDocumentBucketNotification();
        }

        private async Task EnsureDocumentBucketNotification()
        {
            var getArgs = new GetBucketNotificationsArgs().WithBucket(DocumentBucket.Name);
            var bucketConfigs = await _client.GetBucketNotificationsAsync(getArgs);

            if (bucketConfigs.QueueConfigs.Any(c => c.Events.Any()))
                return;

            var queueConfig = new QueueConfig("arn:minio:sqs::primary:webhook");
            queueConfig.AddEvents([EventType.ObjectCreatedAll]);

            var bucketNotification = new BucketNotification();
            bucketNotification.AddQueue(queueConfig);

            var argsObj = new SetBucketNotificationsArgs()
                .WithBucket(DocumentBucket.Name)
                .WithBucketNotificationConfiguration(bucketNotification);

            await _client.SetBucketNotificationsAsync(argsObj);
        }
    }
}
