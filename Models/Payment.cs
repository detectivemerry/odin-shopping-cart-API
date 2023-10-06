namespace OdinShopping.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentType { get; set; } = null!;
        public int Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
