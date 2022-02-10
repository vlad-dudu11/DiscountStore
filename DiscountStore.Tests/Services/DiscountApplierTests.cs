using DiscountStore.Services;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DiscountStore.Tests.Services
{
    [TestFixture]
    public class DiscountApplierTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void GivenNullItemsNeededList_WhenApplyDiscountsIsInvoked_ThenEmptyListIsReturned(int itemsPurchased)
        {
            // Arrange
            var discountApplier = new DiscountApplier();

            // Act
            var result = discountApplier.ApplyDiscounts(null, itemsPurchased);

            // Assert
            Assert.IsEmpty(result);
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(1)]
        public void GivenEmptyItemsNeededList_WhenApplyDiscountsIsInvoked_ThenEmptyListIsReturned(int itemsPurchased)
        {
            // Arrange
            var discountApplier = new DiscountApplier();

            // Act
            var result = discountApplier.ApplyDiscounts(Enumerable.Empty<int>(), itemsPurchased);

            // Assert
            Assert.IsEmpty(result);
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void GivenItemsNeededListAndNoItemsPurchased_WhenApplyDiscountsIsInvoked_ThenEmptyListIsReturned(int itemsPurchased)
        {
            // Arrange
            var discountApplier = new DiscountApplier();

            // Act
            var result = discountApplier.ApplyDiscounts(new List<int> { 2,3,4 }, itemsPurchased);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GivenItemsNeededListAndNotEnoughItemsPurchased_WhenApplyDiscountsIsInvoked_ThenEmptyListIsReturned()
        {
            // Arrange
            var discountApplier = new DiscountApplier();

            // Act
            var result = discountApplier.ApplyDiscounts(new List<int> { 2, 3, 4 }, 1);

            // Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GivenItemsNeededListAndEnoughItemsPurchased_WhenApplyDiscountsIsInvoked_ThenExpectedOutputIsReturned()
        {
            // Arrange
            var discountApplier = new DiscountApplier();

            // Act
            var result = discountApplier.ApplyDiscounts(new List<int> { 5, 2, 15 }, 13);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(x => x.Item1 == 5 && x.Item2 == 2)); // So we have the discount for 5 items applied twice
            Assert.IsTrue(result.Any(x => x.Item1 == 2 && x.Item2 == 1)); // And the discount for 2 items applied once
            Assert.IsFalse(result.Any(x => x.Item1 == 15)); // And the discount for 15 items is never applied
        }
    }
}
