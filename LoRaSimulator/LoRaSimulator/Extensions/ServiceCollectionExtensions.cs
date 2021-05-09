namespace LoRaSimulator.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using System.IO;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using LoRaSimulator.Tcp;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSimulatorService(this IServiceCollection services)
        {
            var asmLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo
                .File(Path.Join(asmLocation, "log.txt")).CreateLogger();

            services.AddSingleton<ILogger>(Log.Logger);
            return services;
        }

        public static IServiceCollection AddSimulatorDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddSingleton<ITcpHandler,TcpHandler>();
            services.AddSingleton<IUdpHandler, UdpHandler>();
            
            return services;
        }
    }
}
