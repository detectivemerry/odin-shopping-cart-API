using Microsoft.AspNetCore.Mvc;

namespace OdinShopping.Services
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public CartService(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
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
    }
}
