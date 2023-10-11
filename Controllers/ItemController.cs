using Microsoft.AspNetCore.Mvc;
using OdinShopping.Models;
using Microsoft.AspNetCore.Authorization;
using OdinShopping.Services;
using OdinShopping.Exceptions;

namespace OdinShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> Get()
        {
            List<Item> result = await _itemService.GetAllIAvailableItems();
 
            if (result.Count >= 1)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> Get(int id)
        {
            try
            {
                var currentItem = await _itemService.GetItem(id);
                return Ok(currentItem);
            }
            catch(ItemNotFoundException)
            {
                return NotFound();
            }  
        }

        [Authorize(Roles = "Admin")] 
        [HttpPost]
        public async Task<ActionResult<Item>> AddItem(Item item) 
        {
            try
            {
                Item addedItem = await _itemService.AddItem(item);
                return Ok(item);
            }
            catch(OdinShoppingException)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<List<Item>>> UpdateItem(ItemDto request) 
        {
            try
            {
                Item updatedItem = await _itemService.UpdateItem(request);
                return Ok(updatedItem);
            }
            catch(ItemNotFoundException)
            {
                return NotFound();
            }
            catch (OdinShoppingException)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")] 
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                bool isDeleted = await _itemService.DeleteItem(id);
                return Ok();
            }
            catch(ItemNotFoundException)
            {
                return NotFound();
            }
            catch(OdinShoppingException)
            {
                return BadRequest();
            }
        }
    }
}
