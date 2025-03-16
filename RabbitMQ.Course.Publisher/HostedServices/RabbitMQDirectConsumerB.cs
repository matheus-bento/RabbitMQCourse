using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Course.Publisher.Services;

namespace RabbitMQ.Course.Publisher.HostedServices
{
    public class RabbitMQDirectConsumerB : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IChannel _rabbitMQChannel;

        public RabbitMQDirectConsumerB(
            RabbitMQConnectionManager rabbitMQConnectionManager,
            ILogger<RabbitMQDirectConsumerB> logger)
        {
            this._rabbitMQChannel = rabbitMQConnectionManager.ConsumerChannel;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.ConsumeMessages(stoppingToken);
        }

        private async Task ConsumeMessages(CancellationToken cancellationToken)
        {
            var consumer = new AsyncEventingBasicConsumer(this._rabbitMQChannel);

            consumer.ReceivedAsync += (sender, eventArgs) =>
            {
                try
                {
                    var messageBytes = eventArgs.Body.ToArray();

                    if (messageBytes.Length > 0)
                    {
                        var message = Encoding.UTF8.GetString(messageBytes);
                        this._logger.LogInformation($"Received message: {message}");
                    }

                    return Task.CompletedTask;
                }
                catch (Exception e)
                {
                    return Task.FromException(e);
                }
            };

            // TODO: bind to a direct exchange
            //await rabbitMQChannel.ExchangeBindAsync();

            while (!cancellationToken.IsCancellationRequested)
            {

            }
        }
    }
}
