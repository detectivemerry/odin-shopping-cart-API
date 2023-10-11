using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Exceptions;
using OdinShopping.Models;

namespace OdinShopping.Services
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private Cart? _cart = null;

        public CartService(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<Cart> InitializeCart()
        {
            if (_cart == null)
            {
                _cart = await GetCurrentCartWithoutPayment()!;
            }
            return _cart;
        }

        public async Task<Cart> GetCurrentCartWithoutPayment()
        {
            var userIdClaim = _userService.GetUserId();
            bool IsConverted = int.TryParse(userIdClaim, out int userId);

            if (!IsConverted)
                throw new Exception("User Id is invalid, not in int");

            Cart? cart = await _context.Carts
                .Where(x => x.UserId == userId && x.Payment == null)
                .FirstOrDefaultAsync();

            // User has an existing cart without payment
            if (cart != null)
                return cart;
            
            // User does not have a cart without payment, create new cart
            else
            {
                var newCart = new Cart();
                newCart.UserId = userId;
                _context.Carts.Add(newCart);

                int result = await _context.SaveChangesAsync();
                if(result > 0)
                {
                    var addedCart = await _context.Carts.Where(x => x.UserId == userId && x.Payment == null).FirstOrDefaultAsync();
                    if(addedCart != null)
                        return addedCart;
                    else
                        throw new Exception("Added cart was not found in database");
                }
                else
                    throw new Exception("Failed to create new cart for user");
            }
        }

        public async Task<Cart> GetCartWithCartItemsAndItems()
        {
            Cart currentCart = await InitializeCart();

            _context.Entry(currentCart)
                    .Collection(cart => cart.CartItems!)
                    .Query()
                    .Include(cartItem => cartItem.Item)
                        .ThenInclude(item => item.Category)
                    .Load();

            if (currentCart != null)
                return currentCart;
            else
                throw new OdinShoppingException("No cart was returned");
        }

        public async Task<CartItem> FindItemInCart(int itemId)
        {
            var cart = await GetCartWithCartItemsAndItems();
            if(cart.CartItems != null)
            {
                foreach (var cartItem in cart.CartItems)
                {
                    if (cartItem.Item.ItemId == itemId)
                        return cartItem;
                }
            }
            return new CartItem();
        }
    }
}
