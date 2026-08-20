using ElasticPDF.Infrastructure.Elasticsearch;
using ElasticPDF.Infrastructure.Messaging;
using ElasticPDF.Infrastructure.MinIO;
using ElasticPDF.Infrastructure.Storage;
using ElasticPDF.Worker.Consumers;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<DocumentProcessingConsumer>();

builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.AddStorage(builder.Configuration);
builder.Services.AddElasticsearch(builder.Configuration);

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    await Task.WhenAll(
        scope.ServiceProvider.GetRequiredService<ElasticsearchInitializer>().InitializeAsync(),
        scope.ServiceProvider.GetRequiredService<StorageInitializer>().InitializeAsync(),
        scope.ServiceProvider.GetRequiredService<RabbitMqQueueInitializer>().InitializeAsync()
    );
}

host.Run();
