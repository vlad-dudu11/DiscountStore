using DiscountStore.Models;
using System.Collections.Generic;

namespace DiscountStore.Repositories
{
    public class PromotionsRepository : IPromotionsRepository
    {
        public IEnumerable<Promotion> Get()
        {
            yield return new Promotion { SKU = "Big mug", Quantity = 2, Price = 1.5m };
            yield return new Promotion { SKU = "Napkins pack", Quantity = 3, Price = 0.9m };
        }
    }
}
