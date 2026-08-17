using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;
using ElasticPDF.Infrastructure.Elasticsearch.Mapping;

namespace ElasticPDF.Infrastructure.Elasticsearch.Entities.Document
{
    public class DocumentMapping : IEntityMapping
    {
        public IEntityIndex EntityIndex { get; } = new DocumentIndex();
        public CreateIndexRequestDescriptor Configure(CreateIndexRequestDescriptor descriptor)
        {
            return descriptor.Mappings(m => m
                .Dynamic(DynamicMapping.False)
                .Properties<Domain.Models.Elasticsearch.Document>(p => p
                    .Keyword(x => x.FileName)
                    .Keyword(x => x.ObjectKey)
                    .Keyword(x => x.Status)
                    .Text(x => x.Content)
                    .Text(x => x.Title)
                    .Date(x => x.PublishDate)
                )
            );
        }
    }
}
