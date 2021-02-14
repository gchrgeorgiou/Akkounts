using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Akkounts.Publisher
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    //Worker
                    services.AddHostedService<Worker>();
                });
    }
}
