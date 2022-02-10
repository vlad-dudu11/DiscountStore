using DiscountStore.Models;
using DiscountStore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscountStore.Services
{
    public class PriceComputationService : IPriceComputationService
    {
        private readonly IPromotionsRepository _promotionsRepository;
        private readonly IDiscountApplier _discountApplier;

        public PriceComputationService(
            IPromotionsRepository promotionsRepository,
            IDiscountApplier discountApplier)
        {
            _promotionsRepository = promotionsRepository ?? throw new ArgumentNullException(nameof(promotionsRepository));
            _discountApplier = discountApplier ?? throw new ArgumentNullException(nameof(discountApplier));
        }

        public decimal Compute(IDictionary<Item, int> itemsDictionary)
        {
            if (!itemsDictionary?.Any() ?? true) return 0;

            var total = 0m;

            var promotions = _promotionsRepository.Get() ?? Enumerable.Empty<Promotion>();

            foreach (var entry in itemsDictionary)
            {
                var currentItemPrice = entry.Key.Price;
                var currentItemQuantity = entry.Value;

                var currentItemPromotions = promotions.Where(p => p.SKU == entry.Key.SKU).ToList();

                // If no promotions available, we simply multiply the quantity with the price
                if (!currentItemPromotions.Any())
                {
                    total += currentItemQuantity * currentItemPrice;
                    continue;
                }

                var itemsPerDiscountType = _discountApplier.ApplyDiscounts(currentItemPromotions.Select(p => p.Quantity), currentItemQuantity);

                // If we manage to apply any discounts, we multiply the discounted price with the number of times it applies and we add it to the total
                foreach (var itemPerDiscountType in itemsPerDiscountType)
                {
                    total += currentItemPromotions.First(x => x.Quantity == itemPerDiscountType.Item1).Price * itemPerDiscountType.Item2;
                }

                // Finally we determine how many items are left that are not discounted
                var itemsWithAppliedDiscount = itemsPerDiscountType.Sum(x => x.Item1 * x.Item2);
                var notDiscountedQuantity = currentItemQuantity - itemsWithAppliedDiscount;

                total += notDiscountedQuantity * currentItemPrice;
            }

            return total;
        }
    }
}
