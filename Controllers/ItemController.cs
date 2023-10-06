using Microsoft.AspNetCore.Mvc;
using OdinShopping.Models;
using Microsoft.AspNetCore.Authorization;
using OdinShopping.Services;

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
 
            if (result.Count > 1)
                return Ok(result);
            else
                return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Item>>> Get(int id)
        {
            var currentItem = await _itemService.GetItem(id);

            if (currentItem != null)
                return Ok(currentItem);
            else
                return NotFound();
        }

        [Authorize(Roles = "Admin")] 
        [HttpPost]
        public async Task<ActionResult<List<Item>>> AddItem(Item item) 
        {
            Item addedItem = await _itemService.AddItem(item);
            if (addedItem != null)
                return Ok(item);
            else
                return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<ActionResult<List<Item>>> UpdateItem(ItemDto request) 
        {
            Item updatedItem = await _itemService.UpdateItem(request);

            if (updatedItem != null)
                return Ok(updatedItem);                
            else
                return BadRequest("item with Id not found");
        }

        [Authorize(Roles = "Admin")] 
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Item>>> Delete(int id)
        {
            bool isDeleted = await _itemService.DeleteItem(id);
            if (isDeleted)
                return Ok();
            else
                return NotFound();
        }
    }
}
