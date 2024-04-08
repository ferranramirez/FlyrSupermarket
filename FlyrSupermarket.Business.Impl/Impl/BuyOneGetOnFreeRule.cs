using Flyr.Infrastructure.Model;
using FlyrSupermarket.Business.Interface;

namespace FlyrSupermarket.Business.Impl
{
    public class BuyOneGetOneFreeRule : IPricingRule
    {
        private readonly string[] ElegibleProductCodes = ["GR1"];
        public bool CanApplyRule(string productCode)
            => ElegibleProductCodes.Contains(productCode);

        public decimal ApplyRule(Product product, int quantity)
            => (quantity / 2 + quantity % 2) * product.Price;
    }
}
