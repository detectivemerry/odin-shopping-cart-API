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
        private readonly ICartService _cartService;
        public CartController(DataContext context, ICartService CartService)
        {
            _context = context;
            _cartService = CartService;
        }

        [HttpGet]
        public async Task<ActionResult<Cart>> Get()
        {
            var cart = await _cartService.GetCartWithCartItemsAndItems();

            if (cart != null)
                return Ok(cart);
            else
                return BadRequest();

            //int currentCartId = await _cartService.GetCartId();
            //var currentCart = await _context.Carts.Where(x => x.CartId == currentCartId)
            //                                        .Include(cart => cart.CartItems)
            //                                        .FirstOrDefaultAsync();
            //if (currentCart != null)
            //{
            //    _context.Entry(currentCart)
            //            .Collection(cart => cart.CartItems!)
            //            .Query()
            //            .Include(cartItem => cartItem.Item)
            //            .Load();

            //    return Ok(currentCart);
            //}
            //else
            //    return BadRequest();
        }
    }
}
