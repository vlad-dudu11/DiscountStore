using System;
using System.Collections.Generic;
using System.Linq;

namespace DiscountStore.Services
{
    public class DiscountApplier : IDiscountApplier
    {
        // Returns a tuple list, each tuple being a pair of the number of items needed for the discount to apply
        // and the number of times that the discount is applied
        public IEnumerable<Tuple<int, int>> ApplyDiscounts(IEnumerable<int> itemsNeededForDiscountList, int itemsPurchased)
        {
            if (!itemsNeededForDiscountList?.Any() ?? true || itemsPurchased < 1) yield break;

            // We remove the discounts which are meant for more items than we have
            // We sort everything descending such that we apply bigger discounts first
            itemsNeededForDiscountList = itemsNeededForDiscountList
                .Where(x => x <= itemsPurchased)
                .OrderByDescending(x => x)
                .ToList();

            var itemsRemaining = itemsPurchased;

            foreach (var quantityNeeded in itemsNeededForDiscountList)
            {
                var numberOfDiscountsApplied = itemsRemaining / quantityNeeded;
                itemsRemaining = itemsPurchased % quantityNeeded;

                yield return new Tuple<int, int>(quantityNeeded, numberOfDiscountsApplied);
            }
        }
    }
}
