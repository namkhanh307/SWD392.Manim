using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Services.Services;

namespace SWD392.Manim.API.Controllers
{
    [Route("api/wallet")]
    [ApiController]
    public class WalletController(IPayService payService) : ControllerBase
    {
        private readonly IPayService _payService = payService;

        [HttpPost("/create")]
        public async Task<IActionResult> CreatePaymentUrl([FromQuery] decimal balance)
        {
            try
            {
                CreatePaymentResult result = await _payService.CreatePaymentUrlRegisterCreator(balance);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Problem("Thất bại");
            }
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetPayment([FromRoute] string id)
        //{
        //    try
        //    {
        //        var result = await _payService.GetPaymentInfo(id);

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Problem("Thất bại");
        //    }
        //}

        [HttpGet("callback/{paymentLinkId}")]
        public async Task<IActionResult> PaymentCallback([FromRoute] string paymentLinkId)
        {
            try
            {
                // Gọi service để xử lý callback thanh toán
                var result = await _payService.HandlePaymentCallback(paymentLinkId);

                if (result)
                {
                    return Ok(new { message = "Wallet balance updated successfully." });
                }
                else
                {
                    return BadRequest(new { message = "Payment not completed or wallet not found." });
                }
            }
            catch (Exception ex)
            {
                return Problem("An error occurred while processing payment callback.");
            }
        }
    }
}
