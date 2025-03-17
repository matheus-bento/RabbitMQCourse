using RabbitMQ.Course.Publisher.HostedServices;
using RabbitMQ.Course.Publisher.Services;

namespace RabbitMQ.Course.Publisher.Extensions
{
    public static class ApplicationServiceConfigurationExtensions
    {
        /// <summary>
        ///     Adds the RabbitMQ connection management singleton to the services collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <returns>A reference to the <see cref="IServiceCollection"/> after all the hosted services have been added.</returns>
        public static IServiceCollection AddRabbitMQConnectionManager(this IServiceCollection services)
        {
            services.AddSingleton<RabbitMQConnectionManager>();
            return services;
        }

        /// <summary>
        ///     Adds all the hosted services that will consume the content published to RabbitMQ by the API to
        ///     the services collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <returns>A reference to the <see cref="IServiceCollection"/> after all the hosted services have been added.</returns>
        public static IServiceCollection AddApplicationHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<RabbitMQQueueConsumer>();

            services.AddHostedService<RabbitMQDirectConsumerA>();
            services.AddHostedService<RabbitMQDirectConsumerB>();
            services.AddHostedService<RabbitMQDirectConsumerC>();

            return services;
        }
    }
}
