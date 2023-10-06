using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICartItemService _cartItemService;
        public CartItemController(DataContext context, IUserService userService, ICartService cartService, ICartItemService cartItemService)
        {
            _context = context;
            _userService = userService;
            _cartService = cartService;
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


        //[Route("{id}")]
        //[HttpPut]
        //public async Task<ActionResult<List<Models.CartItem>>> Update(CartItemDto request, int id)
        //{
        //    var cartItemToBeUpdated = await _context.CartItems.FindAsync(id);
        //    var item = await _context.Items.Where(x => x.ItemId == request.ItemId).FirstOrDefaultAsync();

        //    if (cartItemToBeUpdated != null && item != null)
        //    {
        //        // Check if item has sufficient quantity left
        //        if (item.QuantityLeft + cartItemToBeUpdated.Quantity < request.Quantity)
        //            return BadRequest();
        //        // Update CartItem
        //        cartItemToBeUpdated.Quantity = request.Quantity;
        //        cartItemToBeUpdated.TotalCost = request.Quantity * item.Price;
        //        // Update Item QuantityLeft
        //        item.QuantityLeft = item.QuantityLeft + cartItemToBeUpdated.Quantity - request.Quantity;

        //        await _context.SaveChangesAsync();
        //        return Ok();    
        //    }
        //    else
        //    {
        //        return BadRequest("item with Id not found");
        //    }
        //}

        //[Route("{id}")]
        //[HttpDelete]
        //public async Task<ActionResult<List<Models.CartItem>>> Delete(int id)
        //{
        //    var cartItemToBeDeleted = await _context.CartItems.FindAsync(id);

        //    if (cartItemToBeDeleted != null)
        //    {
        //        var item = await _context.Items.Where(x => x.ItemId == cartItemToBeDeleted.Item.ItemId).FirstOrDefaultAsync();

        //        if(item != null)
        //        {
        //            // Update Item QuantityLeft
        //            item.QuantityLeft += cartItemToBeDeleted.Quantity;

        //            // Delete CartItem
        //            _context.CartItems.Remove(cartItemToBeDeleted);

        //            await _context.SaveChangesAsync();
        //            return Ok();
        //        }
        //        else
        //            return BadRequest("item with Id not found");
        //    }
        //    else
        //        return BadRequest("CartItem with Id not found");


        //}

    }
}
