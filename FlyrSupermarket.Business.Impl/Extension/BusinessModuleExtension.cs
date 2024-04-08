using FlyrSupermarket.Business.Contract;
using FlyrSupermarket.Business.Impl;
using FlyrSupermarket.Business.Impl.PricingRules;
using Microsoft.Extensions.DependencyInjection;

namespace FlyrSupermarket.Business.Extension
{
    public static class BusinessModuleExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services
                .AddScoped<ICheckoutService, CheckoutService>()
                .AddScoped<IPricingRuleFactory, PricingRuleFactory>()
                .AddScoped<IPricingRule, BuyOneGetOneFreeRule>()
                .AddScoped<IPricingRule, BulkDiscountRule>();

            return services;
        }
    }
}
