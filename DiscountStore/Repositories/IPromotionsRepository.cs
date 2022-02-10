using DiscountStore.Models;
using System.Collections.Generic;

namespace DiscountStore.Repositories
{
    public interface IPromotionsRepository
    {
        IEnumerable<Promotion> Get();
    }
}
