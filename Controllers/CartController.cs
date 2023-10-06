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
        private readonly ICartService _cartService;
        public CartController(ICartService CartService)
        {
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
        }
    }
}
