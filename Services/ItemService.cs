using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using OdinShopping.Exceptions;
using OdinShopping.Models;

namespace OdinShopping.Services
{
    public class ItemService : IItemService
    {
        private readonly DataContext _context;
        public ItemService(DataContext context)
        {
            _context = context;

        }

        // Get all items with Quantity above 0
        public async Task<List<Item>> GetAllIAvailableItems()
        {
            List<Item> items = await _context.Items
                .Where(x => x.QuantityLeft > 0)
                .Include(x => x.Category)
                .ToListAsync();

            return items;
        }

        public async Task<Item> GetItem(int itemId)
        {
            var currentItem = await _context.Items.FindAsync(itemId);

            if (currentItem != null)
                return currentItem;
            else
                throw new ItemNotFoundException(itemId);
        }

        public async Task<Item> AddItem(Item request)
        {
            _context.Items.Add(request);
            int result = await _context.SaveChangesAsync();

            if (result > 0)
                return request;
            else
                throw new OdinShoppingException("Add transaction was not successful");
        }

        public async Task<Item> UpdateItem(ItemDto request)
        {
            var dbItem = await _context.Items.FindAsync(request.ItemId);
            
            if(dbItem != null)
            {
                dbItem.Name = request.Name;
                dbItem.Author = request.Author;
                dbItem.Price = request.Price;
                dbItem.Description = request.Description;
                dbItem.QuantityLeft = request.QuantityLeft;
            }
            else
                throw new ItemNotFoundException(request.ItemId);

            int result = await _context.SaveChangesAsync();

            if (dbItem != null && result > 0)
                return dbItem;
            else
                throw new OdinShoppingException("Update transaction was not successful");
        }

        public async Task<bool> DeleteItem(int itemId)
        {
            var currentItem = await _context.Items.FindAsync(itemId);

            if (currentItem != null)
                _context.Items.Remove(currentItem);
            else
                throw new ItemNotFoundException(itemId);

            int result = await _context.SaveChangesAsync();

            if (result > 0)
                return true;
            else
                throw new OdinShoppingException("Delete transaction was not successful");
        }
    }
}
