using FlyrSupermarket.Business.Contract;
using Microsoft.Extensions.Hosting;

namespace FlyrSupermarket.Presentation.Console
{
    public class Worker : BackgroundService
    {
        private readonly IHost _host;
        public ICheckoutService _checkoutService;

        public Worker(IHost host, ICheckoutService checkoutService)
        {
            _host = host;
            _checkoutService = checkoutService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO: Console code to play with the functionalities 

            System.Console.ReadLine();
            await _host.StopAsync();
        }
    }
}
