using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OdinShopping.Exceptions;
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
            try
            {
                Payment newPayment = await _paymentService.AddPayment(request);
                return Ok(newPayment);
            }
            catch(OdinShoppingException)
            {
                return BadRequest();
            }   
        }

        [HttpGet, Authorize(Roles = "Admin")]
        public async Task<ActionResult<Payment>> GetPaymentWithinDate([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var newPayment = await _paymentService.GetPaymentWithinDate(startDate, endDate);
                return Ok(newPayment);
            }
            catch (OdinShoppingException)
            {
                return BadRequest();
            }   
        }
    }
}
