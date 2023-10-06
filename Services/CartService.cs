using Azure.Core;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<int> GetCartId()
        {
            var userIdClaim = _userService.GetUserId();
            bool IsConverted = int.TryParse(userIdClaim, out int userId);
            if (!IsConverted)
                throw new Exception();

            var cart = await _context.Carts.Where(x => x.UserId == userId && x.Payment == null).FirstOrDefaultAsync();
            if (cart != null) // User has a cart without payment
                return cart.CartId;

            else // User does not have a cart without payment, create new cart
            {
                var newCart = new Models.Cart();
                newCart.UserId = userId;
                _context.Carts.Add(newCart);
                await _context.SaveChangesAsync();
                var addedCart = await _context.Carts.Where(x => x.UserId == userId && x.Payment == null).FirstOrDefaultAsync();
                if(addedCart != null)
                    return addedCart.CartId;
                else
                    throw new Exception();
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

            return currentCart;
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

        //public async Task<CartItem> AddCartItem(CartItemDto request)
        //{

        //    var item = await _context.Items.Where(x => x.ItemId == request.ItemId).FirstOrDefaultAsync();

        //    Cart currentCart = await initializeCart();
        //    int currentCartId = currentCart.CartId;

        //    //var currentCart = await _context.Carts.Where(x => x.CartId == currentCartId).FirstOrDefaultAsync();
        //    //bool isFoundInCurrentCart = await CheckIfItemAlreadyInCart();


        //    // if is a new item, add new CartItem
        //    if (item != null && currentCart != null && !isFoundInCurrentCart)
        //    {
        //        // Check if item has sufficient quantity left
        //        if (item.QuantityLeft < request.Quantity)
        //            return BadRequest();

        //        // Add new cartItem
        //        Models.CartItem newCartItem = new Models.CartItem();
        //        newCartItem.Quantity = request.Quantity;
        //        newCartItem.Item = item;
        //        newCartItem.TotalCost = item.Price * request.Quantity;
        //        newCartItem.CartId = currentCartId;
        //        newCartItem.Cart = currentCart;
        //        _context.CartItems.Add(newCartItem);

        //        //Update item's quantityLeft
        //        item.QuantityLeft -= request.Quantity;
        //        await _context.SaveChangesAsync();
        //        return Ok(request);
        //    }

        //    // if item already in cart, update that item quantity instead
        //    else if (item != null && currentCart != null && isFoundInCurrentCart)
        //    {
        //        var cartItemToBeUpdated = await _context.CartItems.FindAsync(id);
        //        // Check if item has sufficient quantity left
        //        if (item.QuantityLeft + cartItemToBeUpdated.Quantity < request.Quantity)
        //            return BadRequest();
        //        // Update CartItem
        //        cartItemToBeUpdated.Quantity = request.Quantity;
        //        cartItemToBeUpdated.TotalCost = request.Quantity * item.Price;
        //        // Update Item QuantityLeft
        //        item.QuantityLeft = item.QuantityLeft + cartItemToBeUpdated.Quantity - request.Quantity;

        //        await _context.SaveChangesAsync();
        //        return Ok(request);
        //    }
        //    else
        //        return BadRequest();
        //}


    }
}
