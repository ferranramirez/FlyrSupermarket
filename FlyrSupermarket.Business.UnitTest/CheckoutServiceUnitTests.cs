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
        public Mock<IPricingRule> _pricingRule;

        public CheckoutServiceUnitTests()
        {
            _productsRepository = new Mock<Repository<Product>>();
            _pricingRules = new List<IPricingRule>();
            _pricingRule = new Mock<IPricingRule>();
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
        public void ScanSameProductTwice_WithNoPricingRule_CorrectPriceApplied()
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
        public void ScanSameProductTwice_WithOnePricingRuleWhichApplies_CorrectPriceApplied()
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

            _pricingRule.Setup(r => r.CanApplyRule(itemCode)).Returns(true);
            _pricingRule.Setup(r => r.ApplyRule(product, 2)).Returns(productPrice);

            _pricingRules.Add(_pricingRule.Object);
            _checkout = new Checkout(_productsRepository.Object, _pricingRules);

            // Act
            _checkout.Scan(itemCode);
            _checkout.Scan(itemCode);
            var totalPrice = _checkout.Total();

            // Assert
            Assert.Equal(productPrice, totalPrice);
        }

        [Fact]
        public void ScanProducts_WithDifferentPricingRules_CorrectPriceApplied()
        {
            // Arrange
            string itemCode1 = "GR1";
            decimal productPrice1 = 3.11M;
            int quantityProduct1 = 2;
            Product product1 = new()
            {
                Code = itemCode1,
                Name = "Product1",
                Price = productPrice1
            };
            _productsRepository.Setup(p => p.Get(itemCode1)).Returns(product1);
            _pricingRule.Setup(r => r.CanApplyRule(itemCode1)).Returns(true);
            _pricingRule.Setup(r => r.ApplyRule(product1, quantityProduct1)).Returns(productPrice1);

            string itemCode2 = "SR1";
            decimal productPrice2 = 5M;
            decimal productPrice2_discounted = 4.55M;
            int quantityProduct2 = 3;
            Product product2 = new()
            {
                Code = itemCode2,
                Name = "Product2",
                Price = productPrice2
            };
            _productsRepository.Setup(p => p.Get(itemCode2)).Returns(product2);
            _pricingRule.Setup(r => r.CanApplyRule(itemCode2)).Returns(true);
            _pricingRule.Setup(r => r.ApplyRule(product2, quantityProduct2)).Returns(productPrice2_discounted * quantityProduct2);

            _pricingRules.Add(_pricingRule.Object);
            _checkout = new Checkout(_productsRepository.Object, _pricingRules);

            decimal expectedPrice = productPrice1 + (productPrice2_discounted * quantityProduct2);

            // Act
            for (int i = 0; i < quantityProduct1; i++) _checkout.Scan(itemCode1);
            for (int i = 0; i < quantityProduct2; i++) _checkout.Scan(itemCode2);
            var totalPrice = _checkout.Total();

            // Assert
            Assert.Equal(expectedPrice, totalPrice);
        }

        [Fact]
        public void ScanProducts_WithDifferentPricingRulesWhichNotAlwaysApply_CorrectPriceApplied()
        {
            // Arrange
            string itemCode1 = "GR1";
            decimal productPrice1 = 3.11M;
            int quantityProduct1 = 2;
            Product product1 = new()
            {
                Code = itemCode1,
                Name = "Product1",
                Price = productPrice1
            };
            _productsRepository.Setup(p => p.Get(itemCode1)).Returns(product1);
            _pricingRule.Setup(r => r.CanApplyRule(itemCode1)).Returns(true);
            _pricingRule.Setup(r => r.ApplyRule(product1, quantityProduct1)).Returns(productPrice1);

            string itemCode2 = "SR1";
            decimal productPrice2 = 5M;
            decimal productPrice2_discounted = 4.55M;
            int quantityProduct2 = 3;
            Product product2 = new()
            {
                Code = itemCode2,
                Name = "Product2",
                Price = productPrice2
            };
            _productsRepository.Setup(p => p.Get(itemCode2)).Returns(product2);
            _pricingRule.Setup(r => r.CanApplyRule(itemCode2)).Returns(true);
            _pricingRule.Setup(r => r.ApplyRule(product2, quantityProduct2)).Returns(productPrice2_discounted * quantityProduct2);

            _pricingRules.Add(_pricingRule.Object);
            _checkout = new Checkout(_productsRepository.Object, _pricingRules);

            string itemCode3 = "HC1";
            decimal productPrice3 = 10M;
            int quantityProduct3 = 4;
            Product product3 = new()
            {
                Code = itemCode3,
                Name = "Product3",
                Price = productPrice3
            };
            _productsRepository.Setup(p => p.Get(itemCode3)).Returns(product3);
            _pricingRule.Setup(r => r.CanApplyRule(itemCode3)).Returns(false);

            _pricingRules.Add(_pricingRule.Object);
            _checkout = new Checkout(_productsRepository.Object, _pricingRules);

            decimal expectedPrice = productPrice1 + (productPrice2_discounted * quantityProduct2) + (productPrice3 * quantityProduct3);

            // Act
            for (int i = 0; i < quantityProduct1; i++) _checkout.Scan(itemCode1);
            for (int i = 0; i < quantityProduct2; i++) _checkout.Scan(itemCode2);
            for (int i = 0; i < quantityProduct3; i++) _checkout.Scan(itemCode3);
            var totalPrice = _checkout.Total();

            // Assert
            Assert.Equal(expectedPrice, totalPrice);
        }
    }
}