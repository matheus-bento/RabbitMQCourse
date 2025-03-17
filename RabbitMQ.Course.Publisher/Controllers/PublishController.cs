using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Course.Publisher.Models;
using RabbitMQ.Course.Publisher.Services;

namespace RabbitMQ.Course.Publisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublishController : ControllerBase
    {
        private readonly IChannel _rabbitMQChannel;

        public PublishController(RabbitMQConnectionManager rabbitMQConnectionManager) =>
            this._rabbitMQChannel = rabbitMQConnectionManager.PublisherChannel;

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PublishRequest req)
        {
            if (req == null)
            {
                return this.BadRequest(new ErrorResponse
                {
                    Error = "Body not included in the request"
                });
            }

            if (req.Exchange != null)
                await this._rabbitMQChannel.BasicPublishAsync(req.Exchange, null, Encoding.UTF8.GetBytes(req.Message));
            else
                await this._rabbitMQChannel.BasicPublishAsync(null, "Queue-1", Encoding.UTF8.GetBytes(req.Message));

            return this.Ok(new PublishResponse
            {
                PublishedMessage = req.Message
            });
        }
    }
}
