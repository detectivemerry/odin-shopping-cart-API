namespace OdinShopping.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Category Category { get; set; } = null!;
        public double Price { get; set; }
        public int QuantityLeft { get; set; }
    }
}
