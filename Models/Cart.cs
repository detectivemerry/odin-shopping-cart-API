using System.Reflection.Metadata;

namespace OdinShopping.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public List<CartItem>? CartItems { get; set;}
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public Payment? Payment { get; set; }
    }
}
