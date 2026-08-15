namespace ElasticPDF.Infrastructure.Elasticsearch.Document
{
    internal class DocumentIndex
    {
        public const string Alias = "document";
        public const string Version = "v1";
        
        public static string Name => $"{Alias}-{Version}";
    }
}
