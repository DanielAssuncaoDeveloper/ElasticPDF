using Elastic.Clients.Elasticsearch.IndexManagement;

namespace ElasticPDF.Infrastructure.Elasticsearch.Mapping
{
    public interface IEntityMapping
    {
        public IEntityIndex EntityIndex { get; }
        public CreateIndexRequestDescriptor Configure(CreateIndexRequestDescriptor descriptor);
    }
}
