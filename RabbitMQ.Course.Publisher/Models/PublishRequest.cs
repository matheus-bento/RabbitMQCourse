namespace RabbitMQ.Course.Publisher.Models
{
    public class PublishRequest
    {
        public string Message { get; set; }
        public string Exchange { get; set; }
    }
}
