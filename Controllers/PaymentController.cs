using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Models;

namespace OdinShopping.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> Add(PaymentDto request)
        {
            Payment newPayment = await _paymentService.AddPayment(request);

            if(newPayment != null)
                return Ok(newPayment);
            else
                return BadRequest();
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Payment>> GetPaymentWithinDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var newPayment = await _paymentService.GetPaymentWithinDate(startDate, endDate);

            if (newPayment != null)
                return Ok(newPayment);
            else
                return BadRequest();
        }
    }
}
