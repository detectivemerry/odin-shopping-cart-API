namespace OdinShopping.Models
{
    public class ItemDto
    {
        public int ItemId { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public int QuantityLeft { get; set; }
    }
}
