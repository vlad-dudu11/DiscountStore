using DiscountStore.Models;
using System.Collections.Generic;

namespace DiscountStore.Repositories
{
    public interface IItemsRepository
    {
        IEnumerable<Item> Get();
    }
}
