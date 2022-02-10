using DiscountStore.Models;

namespace DiscountStore.Services
{
    public interface ICartService
    {
        void Add(Item item);
        void Remove(Item item);
        decimal GetTotal();
    }
}
