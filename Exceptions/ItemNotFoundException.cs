using OdinShopping.Models;

namespace OdinShopping.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(Item item): this(item.Name)
        {
        }

        public ItemNotFoundException(string itemName)
            : base($"Item {itemName} was not found.")
        {
        }

        public ItemNotFoundException(int itemId)
    : base($"Item with ItemID: {itemId} was not found.")
        {
        }
    }
}
