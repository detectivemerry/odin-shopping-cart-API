using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Models;

namespace OdinShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpPost]
        public async Task<ActionResult<CartItem>> AddCartItem(CartItemDto request)
        {
            var cartItem = await _cartItemService.AddCartItem(request);

            if (cartItem != null)
                return Ok(cartItem);
            else
                return BadRequest();    
        }

        [HttpPut]
        public async Task<ActionResult<List<CartItem>>> Update(CartItemDto request)
        {
            var cartItem = await _cartItemService.UpdateCartItem(request);
            if (cartItem != null)
                return Ok(cartItem);
            else
                return BadRequest();
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            bool wasItemDeleted = await _cartItemService.DeleteCartItem(id);

            if (wasItemDeleted)
                return Ok();
            else
                return BadRequest();
        }
    }
}
