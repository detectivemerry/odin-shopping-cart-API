namespace OdinShopping.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public double TotalCost { get; set; }
        public int Quantity { get; set; }
        public Item Item { get; set; } = null!;
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
    }
}
