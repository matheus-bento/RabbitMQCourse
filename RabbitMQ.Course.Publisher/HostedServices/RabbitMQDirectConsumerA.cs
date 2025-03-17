using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Course.Publisher.Services;

namespace RabbitMQ.Course.Publisher.HostedServices
{
    public class RabbitMQDirectConsumerA : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IChannel _rabbitMQChannel;

        public RabbitMQDirectConsumerA(
            RabbitMQConnectionManager rabbitMQConnectionManager,
            ILogger<RabbitMQDirectConsumerA> logger)
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
            try
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
                            this._logger.LogInformation($"Exchange-A received a message: {message}");
                        }

                        return Task.CompletedTask;
                    }
                    catch (Exception e)
                    {
                        return Task.FromException(e);
                    }
                };

                await this._rabbitMQChannel.ExchangeDeclareAsync("Exchange-A", ExchangeType.Direct);

                QueueDeclareOk queueDeclareResult = await this._rabbitMQChannel.QueueDeclareAsync();
                await this._rabbitMQChannel.QueueBindAsync(queueDeclareResult.QueueName, "Exchange-A", null);

                await this._rabbitMQChannel.BasicConsumeAsync(queueDeclareResult.QueueName, true, consumer);

                while (!cancellationToken.IsCancellationRequested)
                {

                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "An error occurred in {0}", nameof(RabbitMQDirectConsumerA));
            }
        }
    }
}
