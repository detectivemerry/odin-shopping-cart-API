using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Models;

namespace OdinShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly DataContext _context;

        public PaymentController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Models.Payment>> Get(PaymentDto request)
        {
            //Add Payment 
            Models.Payment payment = new Models.Payment();
            payment.Amount = request.Amount;
            payment.PaymentType =  request.PaymentType;
            _context.Payment.Add(payment);

            //Update Cart
            var currentCart = await _context.Carts.Where(x => x.CartId == request.Cart.CartId).FirstOrDefaultAsync();
            if (currentCart != null)
                currentCart.Payment = payment;
            else
                return BadRequest();

            await _context.SaveChangesAsync();

            return Ok(payment);
        }
    }
}
