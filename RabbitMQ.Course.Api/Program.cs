namespace RabbitMQ.Course.Api
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
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services);

            return builder;
        }

        static void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }
    }
}

