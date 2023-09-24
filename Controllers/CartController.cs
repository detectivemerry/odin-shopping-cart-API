using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OdinShopping.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace OdinShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly ICartService _cartService;
        public CartController(DataContext context, IUserService userService, ICartService CartService)
        {
            _context = context;
            _userService = userService;
            _cartService = CartService;
        }

        [HttpGet]
        public async Task<ActionResult<Models.Cart>> Get()
        {
            int currentCartId = await _cartService.GetCartId();
            var currentCart = await _context.Carts.Where(x => x.CartId == currentCartId).FirstOrDefaultAsync();

            if (currentCart != null)
                return Ok(currentCart);
            else
                return BadRequest();
        }

        [HttpGet]
        [Route("CartItems")]
        public async Task<ActionResult<List<Models.CartItem>>> GetCartItems()
        {
            int currentCartId = await _cartService.GetCartId();
            var currentCart = await _context.Carts.Where(x => x.CartId == currentCartId).FirstOrDefaultAsync();
            var result = new List<Models.CartItem>();

            if (currentCart != null && currentCart.CartItems != null)
            {
                foreach (Models.CartItem cartItem in currentCart.CartItems)
                {
                    result.Add(cartItem);
                }

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
