using Elastic.Clients.Elasticsearch;
using ElasticPDF.Infrastructure.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddSingleton(x =>
    new ElasticsearchClient(
        new Uri(builder.Configuration.GetSection("Elasticsearch:Url").Value!)
        ));

builder.Services.AddScoped<ElasticsearchInitializer>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var elasticsearchInitializer = scope.ServiceProvider.GetRequiredService<ElasticsearchInitializer>();
    await elasticsearchInitializer.InitializeAsync();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();