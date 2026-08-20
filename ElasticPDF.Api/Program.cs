using Elastic.Clients.Elasticsearch;
using ElasticPDF.Infrastructure.Elasticsearch;
using ElasticPDF.Infrastructure.Messaging;
using ElasticPDF.Infrastructure.MinIO;
using ElasticPDF.Infrastructure.Storage;
using Minio;

var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddSwaggerGen();

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddStorage(builder.Configuration);
builder.Services.AddRabbitMq(builder.Configuration);
builder.Services.AddElasticsearch(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    await Task.WhenAll(
        scope.ServiceProvider.GetRequiredService<ElasticsearchInitializer>().InitializeAsync(),
        scope.ServiceProvider.GetRequiredService<StorageInitializer>().InitializeAsync(),
        scope.ServiceProvider.GetRequiredService<RabbitMqQueueInitializer>().InitializeAsync()
    );
}

app.UseAuthorization();
app.MapControllers();
app.Run();