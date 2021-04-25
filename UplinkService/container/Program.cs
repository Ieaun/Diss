namespace UplinkService
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System.IO;
    using System.Reflection;
    using Serilog;
    using System;

    public class Program
    {
        public static void Main(string[] args)
        {
            var asmLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo
                .File(Path.Join(asmLocation, "log.txt")).CreateLogger();

            Log.Logger.Information("Starting container");
          //  try
           // {
                CreateHostBuilder(args).Build().Run();
           // }
         //   catch (Exception e)
            /*{
                Log.Logger.Information("Fatal shutdown {@Exception}", e);
                return;
            }*/
           // Log.Logger.Information("Graceful shutdown");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
