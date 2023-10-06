using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Models;

namespace OdinShopping.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> Get()
        {
            var result = await _categoryService.GetCategory();

            if (result.Count > 1)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<List<Item>>> Add(string categoryName)
        {
            var addedCategory = await _categoryService.AddCategory(categoryName);
            if (addedCategory != null)
                return Ok(addedCategory);
            else
                return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<List<Item>>> Update(string categoryName, [FromRoute] int id)
        {
            var updatedItem = await _categoryService.UpdateCategory(categoryName, id);

            if (updatedItem != null)
                return Ok(updatedItem);
            else
                return BadRequest("item with Id not found");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Item>>> Delete(int id)
        {
            bool isDeleted = await _categoryService.DeleteCategory(id);
            if (isDeleted)
                return Ok();
            else
                return NotFound();
        }
    }
}
