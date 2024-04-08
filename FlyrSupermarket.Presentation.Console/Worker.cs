using FlyrSupermarket.Business.Contract;
using FlySupermarket.Infrastructure.Context;
using Microsoft.Extensions.Hosting;

namespace FlyrSupermarket.Presentation.Console
{
    public class Worker : BackgroundService
    {
        private readonly IHost _host;
        public ICheckoutService _checkoutService;
        private readonly FlyrContext _context;

        public Worker(IHost host, ICheckoutService checkoutService, FlyrContext context)
        {
            _host = host;
            _checkoutService = checkoutService;
            _context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var foo = _context.GetProducts();
            // TODO: Console code to play with the functionalities 
            _checkoutService.Scan("GR1");
            _checkoutService.Scan("SR1");
            _checkoutService.Scan("GR1");
            _checkoutService.Scan("GR1");
            _checkoutService.Scan("CF1");
            _checkoutService.Scan("SR1");
            _checkoutService.Scan("SR1");
            _checkoutService.Scan("CF1");
            _checkoutService.Scan("CF1");
            _checkoutService.Scan("CF1");
            var price = _checkoutService.Total();

            System.Console.WriteLine("Total:" + price);
            System.Console.ReadLine();
            await _host.StopAsync();
        }
    }
}
