using FlyrSupermarket.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyrSupermarket.Business.Impl
{
    public class Checkout
    {
        public readonly Offer? offer;
        public IList<ProductDto> ShoppingCart;

        public Checkout()
        {
            ShoppingCart = new List<ProductDto>();
        }

        public decimal Total()
        {
            return 0.0M;
        }

        public bool Scan(string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
            { 
                return false;
            }
            //ShoppingCart.Add(productCode);
            return true;
        }
    }
}
