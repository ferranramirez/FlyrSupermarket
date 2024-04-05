using FlyrSupermarket.Business.Impl;
using Moq;

namespace FlyrSupermarket.Business.UnitTest
{
    public class CheckoutServiceUnitTests
    {
        private Mock<Checkout> _checkout;

        public CheckoutServiceUnitTests()
        {
            _checkout = new Mock<Checkout>();
        }

        [Fact]
        public void EmptyCart_ReturnsZeroTotal()
        {
            // Arrange
            var expected = 0M;
            //_checkout.Setup(x => x.Total()).Returns(0M);

            // Act
            var totalPrice = _checkout.Object.Total();

            // Assert
            Assert.Equal(expected, totalPrice);
        }

        [Fact]
        public void ScanOneProduct_GetItsPrice()
        {
            // Arrange
            string itemCode = "GR1";
            decimal expectedPrice = 3.11M;
            //_checkout.Setup(x => x.Scan(itemCode)).Returns(true);
            //_checkout.Setup(x => x.Total()).Returns(0M);

            // Act
            _checkout.Object.Scan(itemCode);
            var totalPrice = _checkout.Object.Total();

            // Assert
            Assert.Equal(expectedPrice, totalPrice);
        }
    }
}