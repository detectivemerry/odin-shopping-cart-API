using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OdinShopping.Models;
using System.Security.Claims;

namespace OdinShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartItemController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        public CartItemController(DataContext context, IUserService userService, ICartService CartService)
        {
            _context = context;
            _userService = userService;
            _cartService = CartService;
        }

        [HttpPost]
        public async Task<ActionResult<Models.CartItemDto>> AddCartItem(CartItemDto request)
        {
            var item = await _context.Items.Where(x => x.ItemId == request.ItemId).FirstOrDefaultAsync();
            int currentCartId = await _cartService.GetCartId();
            var currentCart = await _context.Carts.Where(x => x.CartId == currentCartId).FirstOrDefaultAsync();
            if (item != null && currentCart != null)
            {
                Models.CartItem newCartItem = new Models.CartItem();
                newCartItem.Quantity = request.Quantity;
                newCartItem.Item = item;
                newCartItem.TotalCost = item.Price * request.Quantity;
                newCartItem.CartId = currentCartId;
                newCartItem.Cart = currentCart;
                _context.CartItems.Add(newCartItem);
                await _context.SaveChangesAsync();

                return Ok(request);
            }
            else
                return BadRequest();
        }

        [Route("{id}")]
        [HttpPut]
        public async Task<ActionResult<List<Models.CartItem>>> Update(int id, [FromBody] int newQuantity)
        {
            var cartItemToBeUpdated = await _context.CartItems.FindAsync(id);

            if (cartItemToBeUpdated == null)
            {
                return BadRequest("item with Id not found");
            }
            else
            {
                cartItemToBeUpdated.Quantity = newQuantity;
                await _context.SaveChangesAsync();
                return Ok();
            }

        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult<List<Models.CartItem>>> Delete(int id)
        {
            var cartItemToBeDeleted = await _context.CartItems.FindAsync(id);

            if (cartItemToBeDeleted == null)
            {
                return BadRequest("item with Id not found");
            }
            else
            {
                _context.CartItems.Remove(cartItemToBeDeleted);
                await _context.SaveChangesAsync();
                return Ok();
            }

        }

    }
}
