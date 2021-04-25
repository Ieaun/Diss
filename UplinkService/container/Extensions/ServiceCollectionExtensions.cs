namespace UplinkService.Extensions
{
    using UplinkService.Queue;
    using Microsoft.Extensions.DependencyInjection;
    using MediatR;
    using Serilog;
    using System.IO;
    using System.Reflection;
    using UplinkService.Database;
    using Microsoft.Extensions.Configuration;
    using MongoDB.Driver;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContainerService(this IServiceCollection services)
        {
            var asmLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo
                .File(Path.Join(asmLocation, "log.txt")).CreateLogger();

            services.AddMediatR(typeof(Program));
            services.AddSingleton<ILogger>(Log.Logger);
            return services;
        }

        public static IServiceCollection AddContainerDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterEasyNetQ(configuration.GetConnectionString("Queue"));
            services.AddSingleton<IQueue,QueueAgent>();

            services.AddSingleton(new MongoClient(configuration.GetConnectionString("MongoDb")));
            services.AddSingleton<IDatabase, MongoDatabase>();

            return services;
        }
    }
}
