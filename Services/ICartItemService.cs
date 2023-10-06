using OdinShopping.Models;

namespace OdinShopping.Services
{
    public interface ICartItemService
    {
        public Task<CartItemDto> UpdateCartItem(CartItemDto request);
        public Task<CartItemDto> AddCartItem(CartItemDto request);
        public Task<bool> DeleteCartItem(int cartItemId);
    }
}
