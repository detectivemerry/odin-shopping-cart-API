using OdinShopping.Models;

namespace OdinShopping.Services
{
    public interface IItemService
    {
        public Task<List<Item>> GetAllIAvailableItems();
        public Task<Item> GetItem(int id);
        public Task<Item> AddItem(Item request);
        public Task<Item> UpdateItem(ItemDto request);
        public Task<bool> DeleteItem(int id);
    }
}
