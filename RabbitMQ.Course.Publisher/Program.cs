using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Course.Publisher.HostedServices;

namespace RabbitMQ.Course.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            WebApplication app = CreateAppBuilder(args).Build();

            // Configure the HTTP request pipeline.

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        static WebApplicationBuilder CreateAppBuilder(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            ConfigureConfiguration(builder.Configuration);

            ConfigureLogging(builder.Logging);
            ConfigureServices(builder.Configuration, builder.Services);

            return builder;
        }

        static void ConfigureLogging(ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddConsole();
        }

        static void ConfigureConfiguration(IConfigurationManager config)
        {
            config.AddEnvironmentVariables();
        }

        static void ConfigureServices(IConfiguration config, IServiceCollection services)
        {
            services.Configure<ApiOptions>(config);

            services.AddSingleton<IConnectionFactory>(serviceProvider =>
            {
                IOptions<ApiOptions> apiOptions = serviceProvider.GetRequiredService<IOptions<ApiOptions>>();

                return new ConnectionFactory
                {
                    HostName = apiOptions.Value.RabbitMqHost
                };
            });

            services.AddHostedService<RabbitMQConsumer>();

            services.AddControllers()
                    .AddJsonOptions(jsonOptions =>
                    {
                        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                    });
        }
    }
}

