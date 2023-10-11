using OdinShopping.Models;

namespace OdinShopping.Exceptions
{
    public class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException(string categoryName)
            : base($"Category {categoryName} was not found.")
        {
        }

        public CategoryNotFoundException(int categoryId)
: base($"Category with CategoryId: {categoryId} was not found.")
        {
        }
    }
}
