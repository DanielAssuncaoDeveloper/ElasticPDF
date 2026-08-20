using ElasticPDF.Infrastructure.Messaging.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticPDF.Infrastructure.Messaging
{
    public static class RabbitMqExtensions
    {
        public static IServiceCollection AddRabbitMq(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RabbitMqOptions>(configuration.GetSection("RabbitMQ"));

            services.AddSingleton<
                IRabbitMqConnection,
                RabbitMqConnection>();

            services.AddSingleton<
                RabbitMqQueueInitializer>();

            services.AddSingleton<
                IMessagePublisher,
                RabbitMqPublisher>();

            return services;
        }
    }
}
