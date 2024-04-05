using Flyr.Infrastructure.Model;
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
        public readonly Offer? offer;
        public IList<Product> ShoppingCart;
        public IRepository<Product> _productsRepository;

        public Checkout(IRepository<Product> productsRepository)
        {
            ShoppingCart = new List<Product>();
            _productsRepository = productsRepository;
        }

        public decimal Total()
        {
            var totalPrice = 0M;
            foreach (var item in ShoppingCart)
            {
                var product = _productsRepository.Get(item.Code);
                totalPrice += item.Price;
            }
            return totalPrice;
        }

        public void Scan(string productCode)
        {
            var product = _productsRepository.Get(productCode)
                ?? throw new Exception("Invalid product");
            ShoppingCart.Add(product);
        }
    }
}
