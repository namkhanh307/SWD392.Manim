using Net.payOS.Types;
using SWD392.Manim.Repositories.ViewModel.Wallet;

namespace SWD392.Manim.Services.Services
{
    public interface IPayService
    {
        Task<CreatePaymentResult> CreatePaymentUrlRegisterCreator(decimal balance);
        Task<ObjectPayment> GetPaymentInfo(string paymentLinkId);
        Task<bool> HandlePaymentCallback(string paymentLinkId);
    }
}
