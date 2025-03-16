using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace RabbitMQ.Course.Publisher.Controllers;

[ApiController]
[Route("[controller]")]
public class PublishController : ControllerBase
{
    private readonly IConnectionFactory _rabbitMQConnectionFactory;

    public PublishController(IConnectionFactory _rabbitMQConnectionFactory)
    {
        this._rabbitMQConnectionFactory = _rabbitMQConnectionFactory;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            result = "OK"
        });
    }
}
