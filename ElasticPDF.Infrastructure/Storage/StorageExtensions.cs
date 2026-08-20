using ElasticPDF.Infrastructure.MinIO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace ElasticPDF.Infrastructure.Storage
{
    public static class StorageExtensions
    {
        public static IServiceCollection AddStorage(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMinio(configureClient => configureClient
                        .WithEndpoint(configuration.GetSection("MinIO:Endpoint").Value!)
                        .WithSSL(false)
                        .WithCredentials(
                            configuration.GetSection("MinIO:User").Value!,
                            configuration.GetSection("MinIO:Password").Value!
                        )
                    .Build());

            services.AddScoped<StorageInitializer>();
            return services;
        }
    }
}
