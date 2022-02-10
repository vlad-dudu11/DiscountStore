using DiscountStore.Models;
using DiscountStore.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DiscountStore.Tests.Services
{
    [TestFixture]
    public class CartServiceTests
    {
        private Mock<IPriceComputationService> _priceComputationServiceMock;
        private Mock<IDictionary<Item, int>> _cartItemsDictionaryMock;

        [SetUp]
        public void SetUp()
        {
            _priceComputationServiceMock = new Mock<IPriceComputationService>();
            _cartItemsDictionaryMock = new Mock<IDictionary<Item, int>>();
        }

        [Test]
        public void GivenItem_WhenAddIsInvoked_ThenContainsKeyIsInvoked()
        {
            // Arrange
            var cartService = new CartService(_priceComputationServiceMock.Object, _cartItemsDictionaryMock.Object);

            // Act
            cartService.Add(new Item());

            // Assert
            _cartItemsDictionaryMock.Verify(x => x.ContainsKey(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public void GivenNewItem_WhenAddIsInvoked_ThenAddIsInvoked()
        {
            // Arrange
            var cartService = new CartService(_priceComputationServiceMock.Object, _cartItemsDictionaryMock.Object);

            // Act
            cartService.Add(new Item());

            // Assert
            _cartItemsDictionaryMock.Verify(x => x.Add(It.IsAny<Item>(), 1), Times.Once);
        }

        [Test]
        public void GivenExistingItem_WhenAddIsInvoked_ThenAddIsNotInvoked()
        {
            // Arrange
            _cartItemsDictionaryMock.Setup(x => x.ContainsKey(It.IsAny<Item>())).Returns(true);

            var cartService = new CartService(_priceComputationServiceMock.Object, _cartItemsDictionaryMock.Object);

            // Act
            cartService.Add(new Item());

            // Assert
            _cartItemsDictionaryMock.Verify(x => x.Add(It.IsAny<Item>(), It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GivenItem_WhenRemoveIsInvoked_ThenTryGetValueIsInvoked()
        {
            // Arrange
            var cartService = new CartService(_priceComputationServiceMock.Object, _cartItemsDictionaryMock.Object);

            // Act
            cartService.Remove(new Item());

            // Assert
            int value;
            _cartItemsDictionaryMock.Verify(x => x.TryGetValue(It.IsAny<Item>(), out value), Times.Once);
        }

        [Test]
        public void GivenNewItem_WhenRemoveIsInvoked_ThenRemoveIsNotInvoked()
        {
            // Arrange
            int value;
            _cartItemsDictionaryMock.Setup(x => x.TryGetValue(It.IsAny<Item>(), out value)).Returns(false);

            var cartService = new CartService(_priceComputationServiceMock.Object, _cartItemsDictionaryMock.Object);
            
            // Act
            cartService.Remove(new Item());

            // Assert
            _cartItemsDictionaryMock.Verify(x => x.Remove(It.IsAny<Item>()), Times.Never);
        }

        [Test]
        public void GivenExistingItemAndSimilarItemsRemaining_WhenRemoveIsInvoked_ThenRemoveIsNotInvoked()
        {
            // Arrange
            int value = 2;
            _cartItemsDictionaryMock.Setup(x => x.TryGetValue(It.IsAny<Item>(), out value)).Returns(true);

            var cartService = new CartService(_priceComputationServiceMock.Object, _cartItemsDictionaryMock.Object);

            // Act
            cartService.Remove(new Item());

            // Assert
            _cartItemsDictionaryMock.Verify(x => x.Remove(It.IsAny<Item>()), Times.Never);
        }

        [Test]
        public void GivenExistingItemAndNoSimilarItemsRemaining_WhenRemoveIsInvoked_ThenRemoveIsInvoked()
        {
            // Arrange
            int value = 1;
            _cartItemsDictionaryMock.Setup(x => x.TryGetValue(It.IsAny<Item>(), out value)).Returns(true);

            var cartService = new CartService(_priceComputationServiceMock.Object, _cartItemsDictionaryMock.Object);

            // Act
            cartService.Remove(new Item());

            // Assert
            _cartItemsDictionaryMock.Verify(x => x.Remove(It.IsAny<Item>()), Times.Once);
        }

        [Test]
        public void GivenCart_WhenGetTotalIsInvoked_ThenComputeIsInvoked()
        {
            // Arrange
            var cartService = new CartService(_priceComputationServiceMock.Object, _cartItemsDictionaryMock.Object);

            // Act
            cartService.GetTotal();

            // Assert
            _priceComputationServiceMock.Verify(x => x.Compute(It.IsAny<IDictionary<Item, int>>()), Times.Once);
        }

        [TestCase(100)]
        [TestCase(200)]
        [TestCase(300)]
        public void GivenCart_WhenGetTotalIsInvoked_ThenExpectedAmountIsReturned(decimal totalAmount)
        {
            // Arrange
            _priceComputationServiceMock.Setup(x => x.Compute(It.IsAny<IDictionary<Item, int>>())).Returns(totalAmount);

            var cartService = new CartService(_priceComputationServiceMock.Object, _cartItemsDictionaryMock.Object);

            // Act
            var result = cartService.GetTotal();

            // Assert
            Assert.AreEqual(totalAmount, result);
        }
    }
}
