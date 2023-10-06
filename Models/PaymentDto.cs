namespace OdinShopping.Models
{
    public class PaymentDto
    {
        public string PaymentType { get; set; } = null!;
        public int Amount { get; set; }
        public int CartId { get; set; }
    }
}
