namespace ElasticPDF.Domain.Models.Elasticsearch
{
    public class Document
    {
        public string FileName { get; set; }
        public string ObjectKey { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
