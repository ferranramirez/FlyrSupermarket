using Flyr.Infrastructure.Model;
using FlyrSupermarket.Business.Contract;

namespace FlyrSupermarket.Business.Impl
{
    public class BulkDiscountRule : IPricingRule
    {
        readonly Dictionary<string, Func<decimal, decimal>> ElegibleProducts = new()
        {
            { "SR1", originalPrice => 4.50M },
            { "CF1", originalPrice => originalPrice * 2/3 }
        };

        public bool CanApplyRule(string productCode)
            => ElegibleProducts.ContainsKey(productCode);

        public decimal ApplyRule(Product product, int quantity)
        {
            if (quantity >= 3)
            {
                decimal discountedPrice = ElegibleProducts[product.Code](product.Price);
                return quantity * discountedPrice;
            }
            return quantity * product.Price;
        }
    }
}
