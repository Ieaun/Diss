namespace container.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder LoggerConfiguration(this IApplicationBuilder applicationBuilder, IConfiguration configuration)
        {
            var root = (IConfigurationRoot)configuration;
            var logger = applicationBuilder.ApplicationServices.GetRequiredService<ILogger>();
            logger.Debug("Config: {@Config}", root.GetDebugView());
            return applicationBuilder;
        }
    }
}
