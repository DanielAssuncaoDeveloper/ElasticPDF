namespace ElasticPDF.Infrastructure.Messaging.Common
{
    public class RabbitMqQueues
    {
        public const string DocumentProcessing = "document-processing-queue";
        public const string DocumentProcessingDLQ = "document-processing-dlq";
    }
}
