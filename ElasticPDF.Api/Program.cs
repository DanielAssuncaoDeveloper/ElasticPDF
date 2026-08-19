using Elastic.Clients.Elasticsearch;
using ElasticPDF.Infrastructure.Elasticsearch;
using ElasticPDF.Infrastructure.MinIO;
using Minio;

var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();
    builder.Services.AddSwaggerGen();

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddMinio(configureClient => configureClient
            .WithEndpoint(builder.Configuration.GetSection("MinIO:Endpoint").Value!)
            .WithSSL(false)
            .WithCredentials(
                builder.Configuration.GetSection("MinIO:User").Value!,
                builder.Configuration.GetSection("MinIO:Password").Value!
            )
        .Build());

builder.Services.AddSingleton(x =>
    new ElasticsearchClient(
        new Uri(builder.Configuration.GetSection("Elasticsearch:Url").Value!)
        ));

builder.Services.AddScoped<ElasticsearchInitializer>();
builder.Services.AddScoped<StorageInitializer>();

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
        scope.ServiceProvider.GetRequiredService<StorageInitializer>().InitializeAsync()
    );
}

app.UseAuthorization();
app.MapControllers();
app.Run();