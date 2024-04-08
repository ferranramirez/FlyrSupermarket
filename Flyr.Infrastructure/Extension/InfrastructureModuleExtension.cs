using Flyr.Infrastructure.Model;
using FlyrSupermarket.Infrastructure.Repository;
using FlySupermarket.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlyrSupermarket.Infrastructure.Extension
{
    public static class InfrastructureModuleExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services
                .AddScoped<IRepository<Product>, Repository<Product>>()
                .AddDbContext<FlyrContext>(opt => opt.UseInMemoryDatabase("SupermarketDb"))
                .AddScoped<FlyrContext>();

            return services;
        }
    }
}
