using OdinShopping.Models;

namespace OdinShopping.Services
{
    public interface IPaymentService
    {
        public Task<Payment> AddPayment(PaymentDto request);
        public Task<List<Payment>> GetPaymentWithinDate(DateTime startDate, DateTime endDate);
    }
}
