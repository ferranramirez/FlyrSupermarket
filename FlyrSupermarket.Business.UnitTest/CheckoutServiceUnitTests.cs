using Flyr.Infrastructure.Model;
using FlyrSupermarket.Business.Impl;
using FlyrSupermarket.Business.Contract;
using FlyrSupermarket.Infrastructure.Repository;
using Moq;
using FlyrSupermarket.Business.Impl.PricingRules;

namespace FlyrSupermarket.Business.UnitTest
{
    public class CheckoutServiceUnitTests
    {
        private ICheckoutService _checkout;
        private Mock<Repository<Product>> _productsRepository;
        private IList<IPricingRule> _pricingRules;
        private Mock<IPricingRule> _pricingRule;
        private IPricingRuleFactory _pricingRuleFactory;


        public CheckoutServiceUnitTests()
        {
            _productsRepository = new Mock<Repository<Product>>();
            _pricingRules = new List<IPricingRule>();
            _pricingRule = new Mock<IPricingRule>();
            _pricingRuleFactory = new PricingRuleFactory(_pricingRules);
        }

        [Fact]
        public void EmptyCart_ReturnsZeroTotal()
        {
            // Arrange
            var expected = 0M;
            _checkout = new CheckoutService(_productsRepository.Object, _pricingRuleFactory);

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
            _checkout = new CheckoutService(_productsRepository.Object, _pricingRuleFactory);

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
            _checkout = new CheckoutService(_productsRepository.Object, _pricingRuleFactory);

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
            _pricingRuleFactory = new PricingRuleFactory(_pricingRules);
            _checkout = new CheckoutService(_productsRepository.Object, _pricingRuleFactory);

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
            int quantityProduct1 = 2;
            decimal productPrice1 = 3.11M, priceWithRuleApplied1 = 3.11M;
            CreateProductWithPricingRule(ref itemCode1, ref productPrice1, ref quantityProduct1, ref priceWithRuleApplied1);

            string itemCode2 = "SR1";
            int quantityProduct2 = 3;
            decimal productPrice2 = 5M;
            decimal productPrice2_discounted = 4.55M, priceWithRuleApplied2 = productPrice2_discounted * quantityProduct2;
            CreateProductWithPricingRule(ref itemCode2, ref productPrice2, ref quantityProduct2, ref priceWithRuleApplied2);
            
            _pricingRules.Add(_pricingRule.Object);
            _pricingRuleFactory = new PricingRuleFactory(_pricingRules);
            _checkout = new CheckoutService(_productsRepository.Object, _pricingRuleFactory);

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
            int quantityProduct1 = 2;
            decimal productPrice1 = 3.11M, priceWithRuleApplied1 = 3.11M;
            CreateProductWithPricingRule(ref itemCode1, ref productPrice1, ref quantityProduct1, ref priceWithRuleApplied1);

            string itemCode2 = "SR1";
            int quantityProduct2 = 3;
            decimal productPrice2 = 5M;
            decimal productPrice2_discounted = 4.55M, priceWithRuleApplied2 = productPrice2_discounted * quantityProduct2;
            CreateProductWithPricingRule(ref itemCode2, ref productPrice2, ref quantityProduct2, ref priceWithRuleApplied2);

            string itemCode3 = "HC1";
            decimal productPrice3 = 10M;
            int quantityProduct3 = 4;
            CreateStandardProduct(ref itemCode3, ref productPrice3, ref quantityProduct3);

            _pricingRules.Add(_pricingRule.Object);
            _pricingRuleFactory = new PricingRuleFactory(_pricingRules);
            _checkout = new CheckoutService(_productsRepository.Object, _pricingRuleFactory);

            decimal expectedPrice = productPrice1 + (productPrice2_discounted * quantityProduct2) + (productPrice3 * quantityProduct3);

            // Act
            for (int i = 0; i < quantityProduct1; i++) _checkout.Scan(itemCode1);
            for (int i = 0; i < quantityProduct2; i++) _checkout.Scan(itemCode2);
            for (int i = 0; i < quantityProduct3; i++) _checkout.Scan(itemCode3);
            var totalPrice = _checkout.Total();

            // Assert
            Assert.Equal(expectedPrice, totalPrice);
        }

        private void CreateProductWithPricingRule(ref string itemCode, ref decimal productPrice, ref int quantityProduct, ref decimal priceWithRuleApplied)
        {
            Product product1 = new()
            {
                Code = itemCode,
                Name = "Product_" + itemCode,
                Price = productPrice
            };
            string productCode = itemCode;
            int productQuantity = quantityProduct;

            _productsRepository.Setup(p => p.Get(productCode)).Returns(product1);
            _pricingRule.Setup(r => r.CanApplyRule(productCode)).Returns(true);
            _pricingRule.Setup(r => r.ApplyRule(product1, productQuantity)).Returns(priceWithRuleApplied);
        }

        private void CreateStandardProduct(ref string itemCode, ref decimal productPrice, ref int quantityProduct)
        {
            Product product = new()
            {
                Code = itemCode,
                Name = "Product_" + itemCode,
                Price = productPrice
            };
            string productCode = itemCode;
            int productQuantity = quantityProduct;

            _productsRepository.Setup(p => p.Get(productCode)).Returns(product);
            _pricingRule.Setup(r => r.CanApplyRule(productCode)).Returns(false);
        }
    }
}