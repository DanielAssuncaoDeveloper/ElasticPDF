using ElasticPDF.Infrastructure.Messaging.Interfaces;
using RabbitMQ.Client;
using System.Text.Json;

namespace ElasticPDF.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IMessagePublisher
    {
        private readonly IRabbitMqConnection _connection;

        public RabbitMqPublisher(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public async Task PublishAsync<T>(string queue, T message)
        {
            var connection = await _connection.GetAsync();
            await using var channel = await connection.CreateChannelAsync();

            var body = JsonSerializer.SerializeToUtf8Bytes(message);

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: queue,
                mandatory: false,
                body: body,
                basicProperties: new BasicProperties());

        }
    }
}
