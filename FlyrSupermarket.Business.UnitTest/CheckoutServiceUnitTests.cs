using Flyr.Infrastructure.Model;
using FlyrSupermarket.Business.Impl;
using FlyrSupermarket.Infrastructure.Repository;
using Moq;

namespace FlyrSupermarket.Business.UnitTest
{
    public class CheckoutServiceUnitTests
    {
        private ICheckoutService _checkout;
        public Mock<Repository<Product>> _productsRepository;

        public CheckoutServiceUnitTests()
        {
            _productsRepository = new Mock<Repository<Product>>();
        }

        [Fact]
        public void EmptyCart_ReturnsZeroTotal()
        {
            // Arrange
            var expected = 0M;

            // Act
            var totalPrice = _checkout.Total();

            // Assert
            Assert.Equal(expected, totalPrice);
        }

        [Fact]
        public void ScanOneProduct_GetItsPrice()
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
            _checkout = new Checkout(_productsRepository.Object);

            // Act
            _checkout.Scan(itemCode);
            var totalPrice = _checkout.Total();

            // Assert
            Assert.Equal(expectedPrice, totalPrice);
        }
    }
}