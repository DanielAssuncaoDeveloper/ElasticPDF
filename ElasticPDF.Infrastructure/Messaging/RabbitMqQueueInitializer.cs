using ElasticPDF.Infrastructure.Messaging.Common;
using ElasticPDF.Infrastructure.Messaging.Interfaces;

namespace ElasticPDF.Infrastructure.Messaging
{
    public class RabbitMqQueueInitializer
    {
        private readonly IRabbitMqConnection _connection;

        public RabbitMqQueueInitializer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public async Task InitializeAsync()
        {
            var connection = await _connection.GetAsync();

            await using var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: RabbitMqQueues.DocumentProcessing,
                durable: true,
                exclusive: false,
                autoDelete: false);
        }
    }
}
