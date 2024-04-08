using Flyr.Infrastructure.Model;
using FlyrSupermarket.Business.Impl;
using FlyrSupermarket.Business.Interface;
using FlyrSupermarket.Infrastructure.Repository;
using Moq;

namespace FlyrSupermarket.Business.UnitTest
{
    public class CheckoutServiceUnitTests
    {
        private ICheckoutService _checkout;
        public Mock<Repository<Product>> _productsRepository;
        public IList<IPricingRule> _pricingRules;
        public Mock<IPricingRule> _buyOneFreeRule;

        public CheckoutServiceUnitTests()
        {
            _productsRepository = new Mock<Repository<Product>>();
            _pricingRules = new List<IPricingRule>();
            _buyOneFreeRule = new Mock<IPricingRule>();
        }

        [Fact]
        public void EmptyCart_ReturnsZeroTotal()
        {
            // Arrange
            var expected = 0M;
            _checkout = new Checkout(_productsRepository.Object, _pricingRules);

            // Act
            var totalPrice = _checkout.Total();

            // Assert
            Assert.Equal(expected, totalPrice);
        }

        [Fact]
        public void ScanOneProduct_GetRightPrice()
        {
            // Arrange
            string itemCode = "GR1";
            decimal expectedPrice = 3.11M;
            Product product = new()
            {
                Code = itemCode,
                Name = "Product1",
                Price = expectedPrice
            };

            _productsRepository.Setup(p => p.Get(itemCode)).Returns(product);
            _checkout = new Checkout(_productsRepository.Object, _pricingRules);

            // Act
            _checkout.Scan(itemCode);
            var totalPrice = _checkout.Total();

            // Assert
            Assert.Equal(expectedPrice, totalPrice);
        }

        [Fact]
        public void ScanSameProductTwice_WithNoPricingRule_GetRightPrice()
        {
            // Arrange
            string itemCode = "GR1";
            decimal productPrice = 3.11M;
            Product product = new()
            {
                Code = itemCode,
                Name = "Product1",
                Price = productPrice
            };

            _productsRepository.Setup(p => p.Get(itemCode)).Returns(product);
            _checkout = new Checkout(_productsRepository.Object, _pricingRules);

            // Act
            _checkout.Scan(itemCode);
            _checkout.Scan(itemCode);
            var totalPrice = _checkout.Total();

            // Assert
            Assert.Equal(productPrice * 2, totalPrice);
        }

        [Fact]
        public void ScanSameProductTwice_WithPricingRule_GetRightPrice()
        {
            // Arrange
            string itemCode = "GR1";
            decimal productPrice = 3.11M;
            Product product = new()
            {
                Code = itemCode,
                Name = "Product1",
                Price = productPrice
            };

            _productsRepository.Setup(p => p.Get(itemCode)).Returns(product);

            _buyOneFreeRule.Setup(r => r.CanApplyRule(itemCode)).Returns(true);
            _buyOneFreeRule.Setup(r => r.ApplyRule(product, 2)).Returns(productPrice);

            _pricingRules.Add(_buyOneFreeRule.Object);
            _checkout = new Checkout(_productsRepository.Object, _pricingRules);

            // Act
            _checkout.Scan(itemCode);
            _checkout.Scan(itemCode);
            var totalPrice = _checkout.Total();

            // Assert
            Assert.Equal(productPrice, totalPrice);
        }
    }
}