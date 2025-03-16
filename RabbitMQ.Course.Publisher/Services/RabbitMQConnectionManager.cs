using RabbitMQ.Client;

namespace RabbitMQ.Course.Publisher.Services
{
    public class RabbitMQConnectionManager : IDisposable
    {
        private IConnectionFactory _connectionFactory;

        private readonly IConnection _publisherConnection;
        private readonly IConnection _consumerConnection;

        private readonly IChannel _publisherChannel;
        private readonly IChannel _consumerChannel;

        public RabbitMQConnectionManager(IConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;

            this._publisherConnection = this._connectionFactory.CreateConnectionAsync().Result;
            this._consumerConnection = this._connectionFactory.CreateConnectionAsync().Result;

            this._publisherChannel = this._publisherConnection.CreateChannelAsync().Result;
            this._consumerChannel = this._consumerConnection.CreateChannelAsync().Result;
        }

        public IChannel PublisherChannel
        {
            get =>
                this._publisherChannel.IsOpen ?
                this._publisherChannel :
                this._publisherConnection.CreateChannelAsync().Result;
        }

        public IChannel ConsumerChannel
        {
            get =>
                this._consumerChannel.IsOpen ?
                this._consumerChannel :
                this._consumerConnection.CreateChannelAsync().Result;
        }

        public void Dispose()
        {
            this._publisherChannel.CloseAsync().Wait();
            this._consumerChannel.CloseAsync().Wait();

            this._publisherConnection.CloseAsync().Wait();
            this._consumerConnection.CloseAsync().Wait();

            this._publisherChannel.Dispose();
            this._consumerChannel.Dispose();

            this._publisherConnection.Dispose();
            this._consumerConnection.Dispose();
        }
    }
}
