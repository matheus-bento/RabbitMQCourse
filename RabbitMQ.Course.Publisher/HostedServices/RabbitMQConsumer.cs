using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Course.Publisher.HostedServices
{
    public class RabbitMQConsumer : BackgroundService
    {
        private ILogger _logger;
        private readonly IConnectionFactory _rabbitMQConnectionFactory;

        public RabbitMQConsumer(IConnectionFactory rabbitMQConnectionFactory, ILogger<RabbitMQConsumer> logger)
        {
            this._rabbitMQConnectionFactory = rabbitMQConnectionFactory;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await this.ConsumeMessages(stoppingToken);
        }

        private async Task ConsumeMessages(CancellationToken cancellationToken)
        {
            using (IConnection rabbitMQConnection = await this._rabbitMQConnectionFactory.CreateConnectionAsync())
            using (IChannel rabbitMQChannel = await rabbitMQConnection.CreateChannelAsync())
            {
                var consumer = new AsyncEventingBasicConsumer(rabbitMQChannel);

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

                await rabbitMQChannel.BasicConsumeAsync("Queue-1", true, consumer, cancellationToken);

                while (!cancellationToken.IsCancellationRequested)
                {

                }
            }
        }
    }
}
