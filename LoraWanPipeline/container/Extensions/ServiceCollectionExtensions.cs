namespace LoraWAN_Pipeline.Extensions
{
    using LoraWAN_Pipeline.Queue;
    using LoraWAN_Pipeline.Tcp;
    using Microsoft.Extensions.DependencyInjection;
    using MediatR;
    using Serilog;
    using System.IO;
    using System.Reflection;
    using container.Database;
    using Microsoft.Extensions.Configuration;
    using MongoDB.Driver;
    using LoraWAN_Pipeline.Udp;
    using LoraWAN_Pipeline.Notifications.ReceivedPackets;
    using LoraWAN_Pipeline.ActivationByPersonalization.Decoders;
    using LoraWAN_Pipeline.Notifications.GatewayStatusUpdates;
    using LoraWAN_Pipeline.Notifications.TransmitPackets;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContainerService(this IServiceCollection services)
        {
            var asmLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo
                .File(Path.Join(asmLocation, "log.txt")).CreateLogger();

            services.AddMediatR(typeof(Program));
            services.AddSingleton<ILogger>(Log.Logger);

            services.AddSingleton<GatewayStatusMapper>();
            services.AddSingleton<ReceivedPacketMapper>();
            services.AddSingleton<TransmitPacketMapper>();
            services.AddSingleton<LoRaAbpDecoder>();

            return services;
        }

        public static IServiceCollection AddContainerDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterEasyNetQ(configuration.GetConnectionString("Queue"));
            services.AddSingleton<IQueue,QueueAgent>();

            services.AddSingleton(new MongoClient(configuration.GetConnectionString("MongoDb")));
            services.AddSingleton<IDatabase, MongoDatabase>();

            services.AddSingleton<ITcpHandler,TcpHandler>();
            services.AddSingleton<IUdpHandler, UdpHandler>();

            return services;
        }
    }
}
