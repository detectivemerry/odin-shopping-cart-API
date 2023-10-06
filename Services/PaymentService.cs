using OdinShopping.Models;
using System.Collections.Specialized;
using System.ComponentModel;

namespace OdinShopping.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly DataContext _context;
        public PaymentService(DataContext context)
        {
            _context = context;
        }

        public async Task<Payment> AddPayment(PaymentDto request)
        {
            //Add Payment 
            Models.Payment payment = new Payment();
            payment.Amount = request.Amount;
            payment.PaymentType = request.PaymentType;
            payment.TransactionDate = DateTime.Now;
            _context.Payment.Add(payment);

            //Update Cart
            var currentCart = await _context.Carts.Where(x => x.CartId == request.CartId).FirstOrDefaultAsync();

            if (currentCart != null)
            {
                currentCart.Payment = payment;

                int result = await _context.SaveChangesAsync();
                if (result > 0)
                    return payment; 
            }

            return new Payment();
        }

        public async Task<List<Payment>> GetPaymentWithinDate(DateTime startDate, DateTime endDate)
        {
            List<Payment> payments = await _context.Payment
                                            .Where(x => x.TransactionDate >= startDate && x.TransactionDate <= endDate)
                                            .ToListAsync();
            return payments;
        }
    }
}
