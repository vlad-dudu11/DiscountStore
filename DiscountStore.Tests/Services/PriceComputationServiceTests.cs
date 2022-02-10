using DiscountStore.Models;
using DiscountStore.Repositories;
using DiscountStore.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscountStore.Tests.Services
{
    [TestFixture]
    public class PriceComputationServiceTests
    {
        private Mock<IPromotionsRepository> _promotionsRepositoryMock;
        private Mock<IDiscountApplier> _discountApplierMock;

        [SetUp]
        public void SetUp()
        {
            _promotionsRepositoryMock = new Mock<IPromotionsRepository>();
            _discountApplierMock = new Mock<IDiscountApplier>();
        }

        [Test]
        public void GivenNullItemsDictionary_WhenComputeIsInvoked_ThenZeroIsReturned()
        {
            // Arrange
            var priceComputationService = new PriceComputationService(_promotionsRepositoryMock.Object, _discountApplierMock.Object);

            // Act
            var result = priceComputationService.Compute(null);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GivenEmptyItemsDictionary_WhenComputeIsInvoked_ThenZeroIsReturned()
        {
            // Arrange
            var priceComputationService = new PriceComputationService(_promotionsRepositoryMock.Object, _discountApplierMock.Object);

            // Act
            var result = priceComputationService.Compute(new Dictionary<Item, int>());

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public void GivenItemsDictionaryAndNoPromotions_WhenComputeIsInvoked_ThenExpectedResultIsReturned()
        {
            // Arrange
            _promotionsRepositoryMock.Setup(x => x.Get()).Returns((IEnumerable<Promotion>)null);

            var priceComputationService = new PriceComputationService(_promotionsRepositoryMock.Object, _discountApplierMock.Object);

            var itemsDictionary = new Dictionary<Item, int>
            {
                [new Item { SKU = "SKU1", Price = 100 }] = 1,
                [new Item { SKU = "SKU2", Price = 200 }] = 2,
                [new Item { SKU = "SKU3", Price = 300 }] = 3,
            };

            // Act
            var result = priceComputationService.Compute(itemsDictionary);

            // Assert
            Assert.AreEqual(1400, result);
        }

        [Test]
        public void GivenItemsDictionaryAndNoPromotionsForGivenItems_WhenComputeIsInvoked_ThenExpectedResultIsReturned()
        {
            // Arrange
            _promotionsRepositoryMock.Setup(x => x.Get()).Returns(new List<Promotion> { new Promotion { SKU = "SKU0", Price = 300, Quantity = 2 } });

            var priceComputationService = new PriceComputationService(_promotionsRepositoryMock.Object, _discountApplierMock.Object);

            var itemsDictionary = new Dictionary<Item, int>
            {
                [new Item { SKU = "SKU1", Price = 100 }] = 1,
                [new Item { SKU = "SKU2", Price = 200 }] = 2,
                [new Item { SKU = "SKU3", Price = 300 }] = 3,
            };

            // Act
            var result = priceComputationService.Compute(itemsDictionary);

            // Assert
            Assert.AreEqual(1400, result);
        }

        [Test]
        public void GivenItemsDictionaryAndPromotionAvailableForOneItem_WhenComputeIsInvoked_ThenExpectedResultIsReturned()
        {
            // Arrange
            _promotionsRepositoryMock.Setup(x => x.Get()).Returns(new List<Promotion> { new Promotion { SKU = "SKU2", Price = 300, Quantity = 2 } });
            _discountApplierMock.Setup(x => x.ApplyDiscounts(It.Is<IEnumerable<int>>(x => x.Contains(2)), 2)).Returns(new List<Tuple<int, int>> { new Tuple<int, int>(2, 1) });

            var priceComputationService = new PriceComputationService(_promotionsRepositoryMock.Object, _discountApplierMock.Object);

            var itemsDictionary = new Dictionary<Item, int>
            {
                [new Item { SKU = "SKU1", Price = 100 }] = 1,
                [new Item { SKU = "SKU2", Price = 200 }] = 2,
                [new Item { SKU = "SKU3", Price = 300 }] = 3,
            };

            // Act
            var result = priceComputationService.Compute(itemsDictionary);

            // Assert
            Assert.AreEqual(1300, result);
        }

        [Test]
        public void GivenItemsDictionaryAndPromotionAvailableForOneItemAndUndiscountedItemsLeft_WhenComputeIsInvoked_ThenExpectedResultIsReturned()
        {
            // Arrange
            _promotionsRepositoryMock.Setup(x => x.Get()).Returns(new List<Promotion> { new Promotion { SKU = "SKU2", Price = 300, Quantity = 2 } });
            _discountApplierMock.Setup(x => x.ApplyDiscounts(It.Is<IEnumerable<int>>(x => x.Contains(2)), 3)).Returns(new List<Tuple<int, int>> { new Tuple<int, int>(2, 1) });

            var priceComputationService = new PriceComputationService(_promotionsRepositoryMock.Object, _discountApplierMock.Object);

            var itemsDictionary = new Dictionary<Item, int>
            {
                [new Item { SKU = "SKU1", Price = 100 }] = 1,
                [new Item { SKU = "SKU2", Price = 200 }] = 3,
                [new Item { SKU = "SKU3", Price = 300 }] = 3,
            };

            // Act
            var result = priceComputationService.Compute(itemsDictionary);

            // Assert
            Assert.AreEqual(1500, result);
        }

        [Test]
        public void GivenItemsDictionaryAndPromotionAvailableForMoreItemsAndUndiscountedItemsLeft_WhenComputeIsInvoked_ThenExpectedResultIsReturned()
        {
            // Arrange
            var promotions = new List<Promotion> 
            { 
                new Promotion { SKU = "SKU1", Price = 350, Quantity = 4 },
                new Promotion { SKU = "SKU1", Price = 190, Quantity = 2 },
                new Promotion { SKU = "SKU2", Price = 300, Quantity = 2 },
            };

            _promotionsRepositoryMock.Setup(x => x.Get()).Returns(promotions);
            _discountApplierMock
                .Setup(x => x.ApplyDiscounts(It.Is<IEnumerable<int>>(x => x.Contains(4) && x.Contains(2)), 7))
                .Returns(new List<Tuple<int, int>> { new Tuple<int, int>(4, 1), new Tuple<int, int>(2, 1) });
            _discountApplierMock
                .Setup(x => x.ApplyDiscounts(It.Is<IEnumerable<int>>(x => x.Contains(2)), 3))
                .Returns(new List<Tuple<int, int>> { new Tuple<int, int>(2, 1) });

            var priceComputationService = new PriceComputationService(_promotionsRepositoryMock.Object, _discountApplierMock.Object);

            var itemsDictionary = new Dictionary<Item, int>
            {
                [new Item { SKU = "SKU1", Price = 100 }] = 7,
                [new Item { SKU = "SKU2", Price = 200 }] = 3,
                [new Item { SKU = "SKU3", Price = 300 }] = 3,
            };

            // Act
            var result = priceComputationService.Compute(itemsDictionary);

            // Assert
            Assert.AreEqual(2040, result);
        }
    }
}
