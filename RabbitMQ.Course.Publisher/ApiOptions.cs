namespace RabbitMQ.Course.Publisher
{
    public class ApiOptions
    {
        [ConfigurationKeyName("RABBIT_MQ_HOST")]
        public string RabbitMqHost { get; set; }

        [ConfigurationKeyName("RABBIT_MQ_USERNAME")]
        public string RabbitMqUsername { get; set; }

        [ConfigurationKeyName("RABBIT_MQ_PASSWORD")]
        public string RabbitMqPassword { get; set; }
    }
}
