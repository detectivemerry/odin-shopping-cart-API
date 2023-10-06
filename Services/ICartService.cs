using Microsoft.AspNetCore.Mvc;
using OdinShopping.Models;

namespace OdinShopping.Services
{
    public interface ICartService
    {
        public Task<Cart> InitializeCart();
        public Task<Cart> GetCartWithCartItemsAndItems();
        public Task<CartItem> FindItemInCart(int itemId);
    }
}
