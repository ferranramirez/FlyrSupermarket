using Flyr.Infrastructure.Model;
using FlyrSupermarket.Business.Interface;
using FlyrSupermarket.Domain.Model;
using FlyrSupermarket.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyrSupermarket.Business.Impl
{
    public class Checkout : ICheckoutService
    {
        public IList<Product> ShoppingCart;
        public readonly IRepository<Product> _productsRepository;
        public readonly IList<IPricingRule> _pricingRules;

        public Checkout(IRepository<Product> productsRepository,
            IList<IPricingRule> pricingRules)
        {
            ShoppingCart = new List<Product>();
            _productsRepository = productsRepository;
            _pricingRules = pricingRules;
        }

        public decimal Total()
        {
            var totalPrice = 0M;

            foreach (var productGroup in ShoppingCart.GroupBy(sc => sc.Code))
            {
                var productCode = productGroup.Key;
                var quantity = GetQuantityInCart(productCode);
                var product = _productsRepository.Get(productCode);

                var rule = _pricingRules.FirstOrDefault(r => r.CanApplyRule(productCode));
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
