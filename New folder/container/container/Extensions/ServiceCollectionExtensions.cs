using System.IO;
using System.Reflection;

namespace container.Extensions
{
    using container.Queue;
    using Microsoft.Extensions.DependencyInjection;
    using MediatR;
    using Serilog;

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

        public static IServiceCollection AddContainerDataAccessLayer(this IServiceCollection services)
        {
            services.AddSingleton<QueueAgent>();
            return services;
        }

    }
}
