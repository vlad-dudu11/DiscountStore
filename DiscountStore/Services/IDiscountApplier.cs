using System;
using System.Collections.Generic;

namespace DiscountStore.Services
{
    public interface IDiscountApplier
    {
        IEnumerable<Tuple<int, int>> ApplyDiscounts(IEnumerable<int> itemsNeededForDiscountList, int itemsPurchased);
    }
}
