using AutoMapper;
using Net.payOS.Types;
using Net.payOS;
using SWD392.Manim.Repositories.Entity;
using SWD392.Manim.Repositories.Repository.Interface;
using SWD392.Manim.Repository.ViewModel.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using SWD392.Manim.Repositories;

namespace SWD392.Manim.Services.Services
{
    public class PayService : IPayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PayOSSettings _payOSSettings;
        private readonly PayOS _payOS;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PayService(IOptions<PayOSSettings> settings, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _payOSSettings = settings.Value;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _payOS = new PayOS(_payOSSettings.ClientId, _payOSSettings.ApiKey, _payOSSettings.ChecksumKey);
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CreatePaymentResult> CreatePaymentUrlRegisterCreator(decimal balance)
        {
            try
            {
                string userId = Authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
                Guid id;
                ApplicationUser? user = null;
                if (Guid.TryParse(userId, out id))
                {
                    user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(u => u.Id.Equals(id)).FirstOrDefaultAsync();
                }
                Wallet? wallet = await _unitOfWork.GetRepository<Wallet>().Entities.Where(w => w.UserId.Equals(id)).FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Tài khoản không tồn tại!");
                }
                

                // Thông tin người mua
                string buyerName = user.FullName;
                string buyerPhone = user.PhoneNumber;
                string buyerEmail = user.Email;

                // Generate an order code and set the description
                var orderCode = new Random().Next(1, 1000);
                var description = "VQRIO123";
                var deposit = new Deposit()
                {
                    Id = Guid.NewGuid().ToString(),
                    Amount = balance,
                    UserId = user.Id.ToString(),
                    Name = user.UserName,
                    Description = description,
                    AccountNo = user.Id.ToString()
                };
                // Create signature data
                var signatureData = new Dictionary<string, object>
                {
                    { "amount", balance },
                    { "cancelUrl", _payOSSettings.ReturnUrlFail },
                    { "description", description },
                    { "expiredAt", DateTimeOffset.Now.AddMinutes(10).ToUnixTimeSeconds() },
                    { "orderCode", orderCode },
                    { "returnUrl", _payOSSettings.ReturnUrl }
                };

                // Sort and compute the signature
                var sortedSignatureData = new SortedDictionary<string, object>(signatureData);
                var dataForSignature = string.Join("&", sortedSignatureData.Select(p => $"{p.Key}={p.Value}"));
                var signature = ComputeHmacSha256(dataForSignature, _payOSSettings.ChecksumKey);

                DateTimeOffset expiredAt = DateTimeOffset.Now.AddMinutes(10);

                // Tạo instance của PaymentData
                var paymentData = new PaymentData(
                    orderCode: orderCode,
                    amount: (int)balance,
                    description: description,
                    items: null, // Truyền danh sách các ItemData đã tạo
                    cancelUrl: _payOSSettings.ReturnUrlFail,
                    returnUrl: _payOSSettings.ReturnUrl,
                    signature: signature,
                    buyerName: buyerName,
                    buyerPhone: buyerPhone,
                    buyerEmail: buyerEmail,

                    buyerAddress: "HCM", // Nếu có
                    expiredAt: (int)expiredAt.ToUnixTimeSeconds()
                );
                await _unitOfWork.GetRepository<Deposit>().InsertAsync(deposit);
                await _unitOfWork.SaveAsync();
                // Gọi API tạo thanh toán
                var paymentResult = await _payOS.createPaymentLink(paymentData);

                return paymentResult;
            }

            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the payment URL.", ex);
            }
        }

        private string? ComputeHmacSha256(string data, string checksumKey)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(checksumKey)))
            {
                var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
