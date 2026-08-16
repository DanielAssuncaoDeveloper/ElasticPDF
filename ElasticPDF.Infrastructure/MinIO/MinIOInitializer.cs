using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Notification;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticPDF.Infrastructure.MinIO
{
    public class MinIOInitializer
    {
        private readonly IMinioClient _client;

        public MinIOInitializer(IMinioClientFactory factory)
        {
            _client = factory.CreateClient();
        }

        public async Task InitializeAsync()
        {
            var existsArgs = new BucketExistsArgs().WithBucket("my-test-bucket");
            bool found = await _client.BucketExistsAsync(existsArgs).ConfigureAwait(false);

            if (!found)
            {
                var makeArgs = new MakeBucketArgs().WithBucket("my-test-bucket");
                await _client.MakeBucketAsync(makeArgs).ConfigureAwait(false);
            }

            var getArgs = new GetBucketNotificationsArgs().WithBucket("my-test-bucket");
            var bucketConfigs = await _client.GetBucketNotificationsAsync(getArgs);

            if (bucketConfigs.QueueConfigs.Any(c => c.Events.Any()))
                return;

            var queueConfig = new QueueConfig("arn:minio:sqs::primary:webhook");
            queueConfig.AddEvents([EventType.ObjectCreatedAll]);

            var bucketNotification = new BucketNotification();
            bucketNotification.AddQueue(queueConfig);

            var argsObj = new SetBucketNotificationsArgs()
                .WithBucket("my-test-bucket")
                .WithBucketNotificationConfiguration(bucketNotification);

            await _client.SetBucketNotificationsAsync(argsObj);
        }
    }
}
