using DiscountStore.Models;
using System.Collections.Generic;

namespace DiscountStore.Services
{
    public interface IPriceComputationService
    {
        decimal Compute(IDictionary<Item, int> itemsDictionary);
    }
}
