using RabbitMQ.Client;

namespace ElasticPDF.Infrastructure.Messaging.Interfaces
{
    public interface IRabbitMqConnection
    {
        Task<IConnection> GetAsync();
    }
}
