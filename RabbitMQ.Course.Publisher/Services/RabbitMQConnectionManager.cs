using RabbitMQ.Client;

namespace RabbitMQ.Course.Publisher.Services
{
    public class RabbitMQConnectionManager : IAsyncDisposable, IDisposable
    {
        private IConnectionFactory _connectionFactory;

        private IConnection _publisherConnection;
        private IChannel _publisherChannel;
        
        private IConnection _consumerConnection;
        private IChannel _consumerChannel;

        public RabbitMQConnectionManager(IConnectionFactory connectionFactory)
        {
            this._connectionFactory = connectionFactory;
        }

        public IConnection PublisherConnection
        {
            get
            {
                if (this._publisherConnection == null || !this._publisherConnection.IsOpen)
                    this._publisherConnection = this._connectionFactory.CreateConnectionAsync().Result;

                return this._publisherConnection;
            }
        }

        public IChannel PublisherChannel
        {
            get
            {
                if (this._publisherChannel == null || !this._publisherChannel.IsOpen)
                    this._publisherChannel = this.PublisherConnection.CreateChannelAsync().Result;

                return this._publisherChannel;
            }
        }

        public IConnection ConsumerConnection
        {
            get
            {
                if (this._consumerConnection == null || !this._consumerConnection.IsOpen)
                    this._consumerConnection = this._connectionFactory.CreateConnectionAsync().Result;

                return this._consumerConnection;
            }
        }
        public IChannel ConsumerChannel
        {
            get
            {
                if (this._consumerChannel == null || !this._consumerChannel.IsOpen)
                    this._consumerChannel = this.ConsumerConnection.CreateChannelAsync().Result;

                return this._consumerChannel;
            }
        }

        public void Dispose()
        {
            if (this._publisherChannel != null)
            {
                this._publisherChannel.CloseAsync().Wait();
                this._publisherChannel.Dispose();

                this._publisherChannel = null;
            }

            if (this._publisherConnection != null)
            {
                this._publisherConnection.CloseAsync().Wait();
                this._publisherConnection.Dispose();

                this._publisherConnection = null;
            }

            if (this._consumerChannel != null)
            {
                this._consumerChannel.CloseAsync().Wait();
                this._consumerChannel.Dispose();

                this._consumerChannel = null;
            }

            if (this._consumerConnection != null)
            {
                this._consumerConnection.CloseAsync().Wait();
                this._consumerConnection.Dispose();

                this._consumerConnection = null;
            }

            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (this._publisherChannel != null)
            {
                await this._publisherChannel.CloseAsync();
                await this._publisherChannel.DisposeAsync().ConfigureAwait(false);

                this._publisherChannel = null;
            }

            if (this._publisherConnection != null)
            {
                await this._publisherConnection.CloseAsync();
                await this._publisherConnection.DisposeAsync().ConfigureAwait(false);

                this._publisherConnection = null;
            }

            if (this._consumerChannel != null)
            {
                await this._consumerChannel.CloseAsync();
                await this._consumerChannel.DisposeAsync().ConfigureAwait(false);

                this._consumerChannel = null;
            }

            if (this._consumerConnection != null)
            {
                await this._consumerConnection.CloseAsync();
                await this._consumerConnection.DisposeAsync().ConfigureAwait(false);

                this._consumerConnection = null;
            }

            GC.SuppressFinalize(this);
        }
    }
}
