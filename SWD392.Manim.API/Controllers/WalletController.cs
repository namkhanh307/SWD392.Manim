using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using SWD392.Manim.Repositories;
using SWD392.Manim.Repositories.ViewModel.Wallet;
using SWD392.Manim.Services.Services;

namespace SWD392.Manim.API.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    public class WalletController(IPayService payService) : ControllerBase
    {
        private readonly IPayService _payService = payService;

        [HttpGet]
        public async Task<IActionResult> GetWallet()
        {
            var result = await _payService.GetWallet();
            return Ok(new BaseResponseModel<GetWalletVM>(
                statusCode: StatusCodes.Status200OK,
                code: ResponseCodeConstants.SUCCESS,
                data: result));
        }
        [HttpPost("create")]
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
                    return Content($"Thanh toán thành công. Mã giao dịch: {orderCode}. Đã cộng tiền vào ví.");
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
