using ElasticPDF.Infrastructure.Messaging.Interfaces;
using RabbitMQ.Client.Events;
using System.Threading.Channels;

namespace ElasticPDF.Worker.Consumers
{
    public class DocumentProcessingConsumer : BackgroundService
    {
        private readonly IRabbitMqConnection _connection;

        public DocumentProcessingConsumer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = await _connection.GetAsync();
            await using var channel = await connection.CreateChannelAsync();

            await channel.BasicQosAsync(
                prefetchCount: 0,
                prefetchSize: 1,
                global: false);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (_, args) =>
            {
                try
                {

                }
                catch (Exception)
                {
                    await channel.BasicNackAsync(
                        deliveryTag: args.DeliveryTag,
                        multiple: false,
                        requeue: false);
                }
            };
        }
    }
}
