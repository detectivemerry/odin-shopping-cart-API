using Microsoft.EntityFrameworkCore;
using OdinShopping.Exceptions;
using OdinShopping.Models;

namespace OdinShopping.Services
{
    public class CartItemService : ICartItemService
    {
        
        private readonly DataContext _context;
        private readonly ICartService _cartService;

        public CartItemService(DataContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<CartItemDto> AddCartItem(CartItemDto request)
        {
            var item = await _context.Items.Where(x => x.ItemId == request.ItemId).FirstOrDefaultAsync();

            var cartItemInCart = await _cartService.FindItemInCart(request.ItemId);

            //If already in cart, update CartItem
            if (cartItemInCart != null)
            {
                request.CartItemId = cartItemInCart.CartId;
                request.Quantity += cartItemInCart.Quantity;
                var updatedCartItem = await UpdateCartItem(request);
                return request;
            }
            //If not in cart, add new CartItem
            else
            {
                Cart currentCart = await _cartService.InitializeCart();

                if (item != null && currentCart != null)
                {
                    // Check if item has sufficient quantity left
                    if (item.QuantityLeft < request.Quantity)
                        throw new Exception("Insufficient Quantity");

                    // Add new cartItem
                    Models.CartItem newCartItem = new CartItem();
                    newCartItem.Quantity = request.Quantity;
                    newCartItem.Item = item;
                    newCartItem.TotalCost = item.Price * request.Quantity;
                    newCartItem.CartId = currentCart.CartId;
                    newCartItem.Cart = currentCart;
                    _context.CartItems.Add(newCartItem);

                    //Reduce quantityLeft on item
                    item.QuantityLeft -= request.Quantity;
                    int result = await _context.SaveChangesAsync();

                    if (result > 0)
                        return request;
                }

                throw new OdinShoppingException();
            }
        }

        public async Task<CartItemDto> UpdateCartItem(CartItemDto request)
        {
            var cartItemToBeUpdated = await _context.CartItems.FindAsync(request.CartItemId);
            var item = await _context.Items.Where(x => x.ItemId == request.ItemId).FirstOrDefaultAsync();

            if (cartItemToBeUpdated != null && item != null)
            {
                // Check if item has sufficient quantity left
                if (item.QuantityLeft + cartItemToBeUpdated.Quantity < request.Quantity)
                    throw new Exception("Insufficient Quantity");
                
                // Update Item QuantityLeft
                item.QuantityLeft = item.QuantityLeft + cartItemToBeUpdated.Quantity - request.Quantity;

                // Update CartItem
                cartItemToBeUpdated.Quantity = request.Quantity;
                cartItemToBeUpdated.TotalCost = request.Quantity * item.Price;

                int result = await _context.SaveChangesAsync();

                if (result > 0)
                    return request;
            }

            throw new OdinShoppingException();
        }

        public async Task<bool> DeleteCartItem(int cartItemId)
        {
            var cartItemToBeDeleted = await _context.CartItems
                                            .Include(ci => ci.Item) 
                                            .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId);
            if (cartItemToBeDeleted != null)
            {
                if (cartItemToBeDeleted.Item != null)
                {
                    // Update Item QuantityLeft
                    cartItemToBeDeleted.Item.QuantityLeft += cartItemToBeDeleted.Quantity;

                    // Delete CartItem
                    _context.CartItems.Remove(cartItemToBeDeleted);

                    int result = await _context.SaveChangesAsync();

                    if (result > 0)
                        return true;
                    else
                        throw new OdinShoppingException();
                }
            }
            throw new OdinShoppingException();
        }
    }
}
