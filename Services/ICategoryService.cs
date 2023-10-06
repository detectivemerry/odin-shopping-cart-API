using OdinShopping.Models;

namespace OdinShopping.Services
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetCategory();
        public Task<Category> AddCategory(string categoryName);
        public Task<Category> UpdateCategory(string categoryName, int categoryId);
        public Task<bool> DeleteCategory(int categoryId);

    }
}
