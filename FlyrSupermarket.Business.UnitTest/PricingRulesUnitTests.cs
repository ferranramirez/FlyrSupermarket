using Flyr.Infrastructure.Model;
using FlyrSupermarket.Business.Impl.PricingRules;
using Xunit;

namespace FlyrSupermarket.Business.Impl.Tests.PricingRules
{
    public class PricingRulesUnitTests
    {
        [Theory]
        [InlineData("GR1", true)]
        [InlineData("SR1", false)]
        [InlineData("CF1", false)]
        public void BuyOneGetOneFreeRule_CanApplyRule_ValidProductCodes_ReturnsCorrectResult(string productCode, bool expectedResult)
        {
            // Arrange
            var rule = new BuyOneGetOneFreeRule();

            // Act
            var result = rule.CanApplyRule(productCode);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(1, 3.11)]
        [InlineData(2, 3.11)]
        [InlineData(3, 6.22)]
        [InlineData(4, 6.22)]
        [InlineData(5, 9.33)]
        public void BuyOneGetOneFreeRule_ApplyRule_ValidQuantity_ReturnsCorrectPrice(int quantity, decimal expectedPrice)
        {
            // Arrange
            var rule = new BuyOneGetOneFreeRule();
            var product = new Product { Price = 3.11M }; // Assuming product price is 3.11 for testing

            // Act
            var result = rule.ApplyRule(product, quantity);

            // Assert
            Assert.Equal(expectedPrice, result);
        }

        [Theory]
        [InlineData("GR1", false)]
        [InlineData("SR1", true)]
        [InlineData("CF1", true)]
        public void BulkDiscountRule_CanApplyRule_ValidProductCodes_ReturnsCorrectResult(string productCode, bool expectedResult)
        {
            // Arrange
            var rule = new BulkDiscountRule();

            // Act
            var result = rule.CanApplyRule(productCode);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("SR1", 1, 5.00)]
        [InlineData("SR1", 2, 10.00)]
        [InlineData("SR1", 3, 13.50)] // Bulk discount applied
        [InlineData("SR1", 4, 18)] // Bulk discount applied
        [InlineData("SR1", 5, 22.5)] // Bulk discount applied
        public void BulkDiscountRule_ValidQuantity_ReturnsCorrectPrice(string productCode, int quantity, decimal expectedPrice)
        {
            // Arrange
            var rule = new BulkDiscountRule();
            var product = new Product { Code = productCode, Price = 5.00M }; // Assuming product price is 5.00 for testing

            // Act
            var result = rule.ApplyRule(product, quantity);

            // Assert
            Assert.Equal(expectedPrice, result);
        }

        [Theory]
        [InlineData("CF1", 1, 15)]
        [InlineData("CF1", 2, 30)]
        [InlineData("CF1", 3, 30)] // Bulk discount applied
        [InlineData("CF1", 4, 40)] // Bulk discount applied
        [InlineData("CF1", 5, 50)] // Bulk discount applied
        public void BulkDiscountRuleWithDynamicDiscount_ValidQuantity_ReturnsCorrectPrice(string productCode, int quantity, decimal expectedPrice)
        {
            // Arrange
            var rule = new BulkDiscountRule();
            var product = new Product { Code = productCode, Price = 15M }; // Assuming product price is 15M for testing

            // Act
            var result = rule.ApplyRule(product, quantity);

            // Assert
            Assert.Equal(expectedPrice, result);
        }
    }
}
