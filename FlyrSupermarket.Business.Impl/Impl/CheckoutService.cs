using Flyr.Infrastructure.Model;
using FlyrSupermarket.Business.Contract;
using FlyrSupermarket.Infrastructure.Repository;

namespace FlyrSupermarket.Business.Impl
{
    public class CheckoutService : ICheckoutService
    {
        public IList<Product> ShoppingCart;
        public readonly IRepository<Product> _productsRepository;
        private readonly IPricingRuleFactory _pricingRuleFactory;

        public CheckoutService(IRepository<Product> productsRepository,
            IPricingRuleFactory pricingRuleFactory)
        {
            ShoppingCart = new List<Product>();
            _productsRepository = productsRepository;
            _pricingRuleFactory = pricingRuleFactory;
        }

        public decimal Total()
        {
            var totalPrice = 0M;

            foreach (var productGroup in ShoppingCart.GroupBy(sc => sc.Code))
            {
                var productCode = productGroup.Key;
                var quantity = GetQuantityInCart(productCode);
                var product = _productsRepository.Get(productCode);

                var rule = _pricingRuleFactory.GetStrategy(productCode);
                if (rule != null)
                    totalPrice += rule.ApplyRule(product, quantity);
                else
                    totalPrice += product.Price * quantity;
            }
            return totalPrice;
        }

        private int GetQuantityInCart(string productCode)
        {
            return ShoppingCart.Count(p => p.Code == productCode);
        }

        public void Scan(string productCode)
        {
            var product = _productsRepository.Get(productCode)
                ?? throw new Exception("Invalid product");
            ShoppingCart.Add(product);
        }
    }
}
