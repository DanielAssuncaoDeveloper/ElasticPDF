using Elastic.Clients.Elasticsearch;
using ElasticPDF.Infrastructure.Elasticsearch.Document;
using IndexManagement = Elastic.Clients.Elasticsearch.IndexManagement;

namespace ElasticPDF.Infrastructure.Elasticsearch
{
    public class ElasticsearchInitializer
    {
        private readonly ElasticsearchClient _client;

        public ElasticsearchInitializer (ElasticsearchClient client)
        {
            _client = client;
        }

        public async Task InitializeAsync()
        {
            bool isSuccess = false;
            int remainingAttempts = 10;
            IndexManagement.ExistsResponse? existsResponse = null;

            while (!isSuccess && remainingAttempts != 0)
            {
                existsResponse = await _client.Indices.ExistsAsync(DocumentIndex.Name);
                isSuccess = existsResponse.IsValidResponse;

                if (!isSuccess)
                {
                    remainingAttempts--;
                    await Task.Delay(750 * Math.Abs(remainingAttempts - 10) + 1);
                }
            }

            if (existsResponse is null || !isSuccess)
                throw new Exception("We were unable to create the Elasticsearch indexes.");

            if (existsResponse.Exists)
                return;

            var createResponse = await _client.Indices.CreateAsync(DocumentIndex.Name, c => c.Configure());
            if (!createResponse.IsSuccess())
                throw new Exception("We were unable to create the Elasticsearch indexes.");
        }
    }
}
