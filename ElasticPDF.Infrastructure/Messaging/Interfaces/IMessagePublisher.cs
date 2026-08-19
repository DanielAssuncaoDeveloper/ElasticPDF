namespace ElasticPDF.Infrastructure.Messaging.Interfaces
{
    public interface IMessagePublisher
    {
        public Task PublishAsync<T>(string queue, T message);
    }
}
