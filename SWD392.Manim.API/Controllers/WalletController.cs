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
    }
}
