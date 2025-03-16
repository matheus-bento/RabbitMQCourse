using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace RabbitMQ.Course.Publisher.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublishController : ControllerBase
    {
        private readonly IConnectionFactory _rabbitMQConnectionFactory;

        public PublishController(IConnectionFactory _rabbitMQConnectionFactory) => this._rabbitMQConnectionFactory = _rabbitMQConnectionFactory;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string message)
        {
            using (IConnection rabbitMQConnection = await this._rabbitMQConnectionFactory.CreateConnectionAsync())
            using (IChannel rabbitMQChannel = await rabbitMQConnection.CreateChannelAsync())
            {
                await rabbitMQChannel.BasicPublishAsync("", "Queue-1", Encoding.UTF8.GetBytes(message));
            }

            return this.Ok(new
            {
                PublishedMessage = message
            });
        }
    }
}
