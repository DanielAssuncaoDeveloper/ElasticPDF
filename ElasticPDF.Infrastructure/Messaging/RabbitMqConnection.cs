using ElasticPDF.Infrastructure.Messaging.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ElasticPDF.Infrastructure.Messaging
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly RabbitMqOptions _options;
        private IConnection _connection;

        public RabbitMqConnection(IOptions<RabbitMqOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IConnection> GetAsync()
        {
            if (_connection is not null)
                return _connection;

            var factory = new ConnectionFactory
            {
                HostName = _options.Host,
                Port = _options.Port,
                UserName = _options.Username,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost,
                AutomaticRecoveryEnabled = true
            };

            _connection = await factory.CreateConnectionAsync();
            return _connection;
        }
    }
}
