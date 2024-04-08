using FlyrSupermarket.Business.Extension;
using FlyrSupermarket.Infrastructure.Extension;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FlyrSupermarket.Presentation.Console
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    BusinessModuleExtension.AddServices(services);
                    InfrastructureModuleExtension.AddServices(services);
                });
    }
}