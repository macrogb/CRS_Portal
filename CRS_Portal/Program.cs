using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace SCV_Portal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Live
            //Log.Logger = new LoggerConfiguration()
            //   .Enrich.FromLogContext()
            //   .MinimumLevel.Debug()
            //   .WriteTo.File(@"C:\SCV SAAS\UAT Environment\04. SCVOne_Live\Logs\SCV.log", rollingInterval: RollingInterval.Day,
            //   rollOnFileSizeLimit: false)
            //   .CreateLogger();

            //Log.Logger = new LoggerConfiguration()
            //   .Enrich.FromLogContext()
            //   .MinimumLevel.Debug()
            //   .WriteTo.File(@"F:\Arun\Work\SCV\SCV One\SCV\SCV\wwwroot\Logs\SCV.log", rollingInterval: RollingInterval.Day, 
            //                rollOnFileSizeLimit: false)
            //       .CreateLogger();
            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            finally
            {
                //Log.CloseAndFlush();
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                //.UseSerilog()
                .UseStartup<Startup>()
                .ConfigureKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = 1073741824; //1GB
                });
    }
}
