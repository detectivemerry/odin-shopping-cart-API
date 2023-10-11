using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Exceptions;
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

            if (result.Count >= 1)
                return Ok(result);
            else
                return NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<List<Item>>> Add(string categoryName)
        {
            try
            {
                var addedCategory = await _categoryService.AddCategory(categoryName);
                return Ok(addedCategory);
            }
            catch(OdinShoppingException)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<List<Item>>> Update(string categoryName, [FromRoute] int id)
        {
            try
            {
                var updatedItem = await _categoryService.UpdateCategory(categoryName, id);
                return Ok(updatedItem);
            }
            catch(OdinShoppingException)
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Item>>> Delete(int id)
        {
            try
            {
                bool isDeleted = await _categoryService.DeleteCategory(id);
                return Ok();
            }
            catch (CategoryNotFoundException)
            {
                return NotFound();
            }
            catch (OdinShoppingException)
            {
                return BadRequest();
            }      
        }
    }
}
