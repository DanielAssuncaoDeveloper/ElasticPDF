using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticPDF.Infrastructure.Elasticsearch
{
    public static class ElasticsearchExtensions
    {
        public static IServiceCollection AddElasticsearch(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton(x =>
            new ElasticsearchClient(
                    new Uri(configuration.GetSection("Elasticsearch:Url").Value!)
                ));

            services.AddScoped<ElasticsearchInitializer>();
            return services;
        }
    }
}
