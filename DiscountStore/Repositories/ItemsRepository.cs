using DiscountStore.Models;
using System.Collections.Generic;

namespace DiscountStore.Repositories
{
    public class ItemsRepository : IItemsRepository
    {
        public IEnumerable<Item> Get()
        {
            yield return new Item { SKU = "Vase", Price = 1.2m };
            yield return new Item { SKU = "Big mug", Price = 1 };
            yield return new Item { SKU = "Napkins pack", Price = 0.45m };
        }
    }
}
