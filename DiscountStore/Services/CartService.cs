using DiscountStore.Models;
using System;
using System.Collections.Generic;

namespace DiscountStore.Services
{
    public class CartService : ICartService
    {
        private readonly IPriceComputationService _priceComputationService;
        private IDictionary<Item, int> _cartItemsDictionary;

        public CartService(IPriceComputationService priceComputationService, IDictionary<Item, int> cartItemsDictionary)
        {
            _priceComputationService = priceComputationService ?? throw new ArgumentNullException(nameof(priceComputationService));
            _cartItemsDictionary = cartItemsDictionary ?? throw new ArgumentNullException(nameof(cartItemsDictionary));
        }

        public void Add(Item item)
        {
            if (_cartItemsDictionary.ContainsKey(item))
            {
                _cartItemsDictionary[item]++;
                return;
            }

            _cartItemsDictionary.Add(item, 1);
        }

        public void Remove(Item item)
        {
            if (_cartItemsDictionary.TryGetValue(item, out var count))
            {
                if (count == 1) _cartItemsDictionary.Remove(item);
                else _cartItemsDictionary[item]--;
            }
        }

        public decimal GetTotal() => _priceComputationService.Compute(_cartItemsDictionary);
    }
}
