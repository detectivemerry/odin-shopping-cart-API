using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Exceptions;
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
            try
            {
                var cartItem = await _cartItemService.AddCartItem(request);
                return Ok(cartItem);
            }
            catch (OdinShoppingException)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult<CartItem>> Update(CartItemDto request)
        {
            try
            {
                var cartItem = await _cartItemService.UpdateCartItem(request);
                return Ok(cartItem);
            }
            catch(OdinShoppingException)
            {
                return BadRequest();
            }
        } 

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                bool wasItemDeleted = await _cartItemService.DeleteCartItem(id);
                return Ok();
            }
            catch(OdinShoppingException)
            {
                return BadRequest();
            }
        }
    }
}
