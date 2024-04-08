using Flyr.Infrastructure.Model;
using FlyrSupermarket.Business.Interface;

namespace FlyrSupermarket.Business.Impl
{
    public class BulkDiscountRule : IPricingRule
    {
        private readonly string[] ElegibleProductCodes = ["SR1"];
        readonly Dictionary<string, decimal> discountedPrice = new()
        {
            { "SR1", 4.50M }
        };
        public bool CanApplyRule(string productCode)
            => ElegibleProductCodes.Contains(productCode);

        public decimal ApplyRule(Product product, int quantity)
        {
            var productPrice = product.Price;
            if (quantity >= 3)
                discountedPrice.TryGetValue(product.Code, out productPrice);

            return quantity * productPrice;
        }
    }
}
