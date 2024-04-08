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
            var product = new Product { Price = 3.11M };

            // Act
            var result = rule.ApplyRule(product, quantity);

            // Assert
            Assert.Equal(expectedPrice, result);
        }
    }
}
