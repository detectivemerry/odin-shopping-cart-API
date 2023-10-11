using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Exceptions;
using OdinShopping.Models;


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
            try
            {
                var cart = await _cartService.GetCartWithCartItemsAndItems();
                return Ok(cart);
            }
            catch (OdinShoppingException)
            {
                return BadRequest();
            }
        }
    }
}
