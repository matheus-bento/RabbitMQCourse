namespace RabbitMQ.Course.Publisher
{
    public class ApiOptions
    {
        [ConfigurationKeyName("RABBIT_MQ_HOST")]
        public string RabbitMqHost { get; set; }
    }
}
