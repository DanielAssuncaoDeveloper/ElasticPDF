using Microsoft.AspNetCore.Mvc;

namespace ElasticPDF.Api.Controllers
{
    [ApiController]
    public class WebhookController : Controller
    {
        [HttpPost("webhook/minio")]
        public Task Webhook([FromBody] object objJson)
        {
            return Task.CompletedTask;
        }
    }
}
