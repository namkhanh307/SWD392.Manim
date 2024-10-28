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

        [HttpGet("ReturnUrl")]
        public async Task<IActionResult> ReturnUrl()
        {

            // Lấy các tham số từ query string
            string responseCode = Request.Query["code"].ToString();
            string id = Request.Query["id"].ToString();
            string cancel = Request.Query["cancel"].ToString();
            string status = Request.Query["status"].ToString();
            string orderCode = Request.Query["orderCode"];

            if (responseCode == "00" && status == "PAID") // Thanh toán thành công
            {
                try
                {
                    // Gọi service để cộng tiền vào ví

                    bool isSuccess = await _payService.HandlePaymentCallback(id, long.Parse(orderCode));

                    if (isSuccess)
                    {
                        return Content($"Thanh toán thành công. Mã giao dịch: {orderCode}. Đã cộng tiền vào ví.");
                    }
                    else
                    {
                        return Content($"Thanh toán thành công. Mã giao dịch: {orderCode}, nhưng không thể cộng tiền vào ví.");
                    }
                }
                catch (Exception ex)
                {
                    return Problem("Đã xảy ra lỗi: " + ex.Message);
                }
            }
            else if (status == "CANCELLED")
            {
                return Content("Thanh toán đã bị hủy.");
            }
            else
            {
                return Content($"Thanh toán không thành công. Mã trạng thái: {responseCode}");
            }
        }
    }
}
