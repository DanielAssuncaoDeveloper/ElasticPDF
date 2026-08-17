using Elastic.Clients.Elasticsearch;
using ElasticPDF.Infrastructure.Elasticsearch.Entities.Document;
using ElasticPDF.Infrastructure.Elasticsearch.Mapping;
using Polly;

namespace ElasticPDF.Infrastructure.Elasticsearch
{
    public class ElasticsearchInitializer
    {
        private readonly ElasticsearchClient _client;
        private IReadOnlyCollection<IEntityMapping> _mappings = [ new DocumentMapping() ];

        public ElasticsearchInitializer (ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task InitializeAsync()
        {
            bool isElasticsearchRunning = await PingElasticsearchAsync();
            if (!isElasticsearchRunning)
                throw new Exception("Elasticsearch is not running.");

            await EnsureEntityIndexes();
        }

        private Task<bool> PingElasticsearchAsync() =>
            Policy.HandleResult<bool>(isValidResponse => !isValidResponse)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () =>
                {
                    var pingResponse = await _client.PingAsync();
                    return pingResponse.IsValidResponse;
                });

        private async Task EnsureEntityIndexes()
        {
            foreach (var map in _mappings)
            {
                var existsResponse = await _client.Indices.ExistsAsync(map.EntityIndex.Name);
                if (!existsResponse.Exists)
                {
                    var createResponse = await _client.Indices.CreateAsync(map.EntityIndex.Name, c => map.Configure(c));
                    if (!createResponse.IsValidResponse)
                        throw new Exception("We were unable to create the Elasticsearch indexes.");
                }
            }
        }
    }
}
