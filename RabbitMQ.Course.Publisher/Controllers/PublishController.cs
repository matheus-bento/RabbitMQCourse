using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
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

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string message)
        {
            await this._rabbitMQChannel.BasicPublishAsync("", "Queue-1", Encoding.UTF8.GetBytes(message));

            return this.Ok(new
            {
                PublishedMessage = message
            });
        }
    }
}
