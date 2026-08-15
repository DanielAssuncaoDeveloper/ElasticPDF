using Elastic.Clients.Elasticsearch.IndexManagement;
using Elastic.Clients.Elasticsearch.Mapping;

namespace ElasticPDF.Infrastructure.Elasticsearch.Document
{
    internal static class DocumentMapping
    {
        public static CreateIndexRequestDescriptor Configure(
            this CreateIndexRequestDescriptor descriptor)
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
