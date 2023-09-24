using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Azure.Core;

namespace OdinShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {

        private readonly DataContext _context;

        public ItemController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Models.Item>>> Get()
        {
            List<Models.Item> items = await _context.Items
                .Where(x => x.QuantityLeft > 0)
                .ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<Models.Item>>> Get(int id)
        {
            //List<Models.Item> items = await _context.Items.ToListAsync();
            //var currentItem = items.Find(i => i.Id == id); 

            var currentItem = await _context.Items.FindAsync(id);

            if (currentItem == null)
            {
                return BadRequest("item with Id not found");
            }


            return Ok(currentItem);
        }

        [HttpPost]
        public async Task<ActionResult<List<Models.Item>>> AddItem(Models.Item item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return Ok(_context.Items);
        }

        [HttpPut]
        public async Task<ActionResult<List<Models.Item>>> UpdateItem(Models.Item request)
        {
            //var currentItem = items.Find(i => i.Id == request.Id);

            var dbItem = await _context.Items.FindAsync(request.ItemId);

            if (dbItem == null)
            {
                return BadRequest("item with Id not found");
            }

            dbItem.Name = request.Name;
            dbItem.Price = request.Price;
            dbItem.Description = request.Description;

            
            await _context.SaveChangesAsync();

            return Ok(await _context.Items.ToListAsync());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Models.Item>>> Delete(int id)
        {
            var currentItem = await _context.Items.FindAsync(id);

            if (currentItem == null)
            {
                return BadRequest("item with Id not found");
            }

            _context.Items.Remove(currentItem);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
