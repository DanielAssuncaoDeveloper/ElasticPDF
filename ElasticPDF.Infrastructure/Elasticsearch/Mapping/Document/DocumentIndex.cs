using ElasticPDF.Infrastructure.Elasticsearch.Mapping;

namespace ElasticPDF.Infrastructure.Elasticsearch.Entities.Document
{
    public class DocumentIndex : IEntityIndex
    {
        public const string Alias = "document";
        public const string Version = "v1";
        
        public string Name => $"{Alias}-{Version}";
    }
}
