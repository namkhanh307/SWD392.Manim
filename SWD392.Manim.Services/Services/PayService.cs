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
using SWD392.Manim.Repositories.ViewModel.Wallet;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Transaction = SWD392.Manim.Repositories.Entity.Transaction;
using Microsoft.AspNetCore.Mvc;

namespace SWD392.Manim.Services.Services
{
    public class PayService : IPayService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly PayOSSettings _payOSSettings;
        private readonly PayOS _payOS;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _client;

        public PayService(IOptions<PayOSSettings> settings, IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, HttpClient client)
        {
            _payOSSettings = settings.Value;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _payOS = new PayOS(_payOSSettings.ClientId, _payOSSettings.ApiKey, _payOSSettings.ChecksumKey);
            _httpContextAccessor = httpContextAccessor;
            _client = client;
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

        //public async Task<ExtendedPaymentInfo> GetPaymentInfo(string paymentLinkId)
        //{
        //    try
        //    {
        //        var getUrl = $"https://api-merchant.payos.vn/v2/payment-requests/{paymentLinkId}";

        //        var request = new HttpRequestMessage(HttpMethod.Get, getUrl);
        //        request.Headers.Add("x-client-id", _payOSSettings.ClientId);
        //        request.Headers.Add("x-api-key", _payOSSettings.ApiKey);

        //        // Send the request
        //        var response = await _client.SendAsync(request);

        //        // Ensure the request is successful
        //        response.EnsureSuccessStatusCode();

        //        var responseContent = await response.Content.ReadAsStringAsync();

        //        var responseObject = JsonConvert.DeserializeObject<JObject>(responseContent);
        //        var paymentInfo = responseObject["data"].ToObject<ObjectPayment>();


        //        string userId = Authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
        //        Guid id;
        //        ApplicationUser? user = null;
        //        if (Guid.TryParse(userId, out id))
        //        {
        //            user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(u => u.Id.Equals(id)).FirstOrDefaultAsync();

        //        }
        //        var wallet = await _unitOfWork.GetRepository<Wallet>()
        //                          .Entities
        //                          .Where(w => w.UserId == id)
        //                          .FirstOrDefaultAsync();

        //        int totalPrice = paymentInfo.Amount;

        //        string buyerName = user.FullName;
        //        string buyerPhone = user.PhoneNumber;
        //        string buyerEmail = user.Email;

        //        var extendedPaymentInfo = new ExtendedPaymentInfo
        //        {
        //            Amount = totalPrice,
        //            Description = "VQRIO123",
        //            BuyerName = buyerName,
        //            BuyerPhone = buyerPhone,
        //            BuyerEmail = buyerEmail,
        //            Status = paymentInfo.Status,

        //        };

        //        // Update product status if payment is completed
        //        if (paymentInfo.Status == "PAID")
        //        {
        //            wallet.Balance += totalPrice;
        //        }

        //        await _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);
        //        await _unitOfWork.SaveAsync();

        //        return extendedPaymentInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BadHttpRequestException("An error occurred while getting payment info.", ex);
        //    }
        //}

        public async Task<ObjectPayment> GetPaymentInfo(string paymentLinkId)
        {
            try
            {
                var getUrl = $"https://api-merchant.payos.vn/v2/payment-requests/{paymentLinkId}";

                var request = new HttpRequestMessage(HttpMethod.Get, getUrl);
                request.Headers.Add("x-client-id", _payOSSettings.ClientId);
                request.Headers.Add("x-api-key", _payOSSettings.ApiKey);

                // Gửi yêu cầu HTTP
                var response = await _client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                var paymentInfo = responseObject["data"].ToObject<ObjectPayment>();

                return paymentInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while getting payment info.", ex);
            }
        }
        public async Task<bool> HandlePaymentCallback(string paymentLinkId)
        {
            try
            {
                // Lấy thông tin thanh toán
                var paymentInfo = await GetPaymentInfo(paymentLinkId);

                // Nếu thanh toán thành công, cập nhật số dư ví
                if (paymentInfo.Status == "PAID")
                {
                    string userId = Authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
                    Guid id;
                    ApplicationUser? user = null;
                    if (Guid.TryParse(userId, out id))
                    {
                        user = await _unitOfWork.GetRepository<ApplicationUser>().Entities.Where(u => u.Id.Equals(id)).FirstOrDefaultAsync();

                    }
                    var wallet = await _unitOfWork.GetRepository<Wallet>()
                                  .Entities
                                  .Where(w => w.UserId == id)
                                  .FirstOrDefaultAsync();

                    if (wallet != null)
                    {
                        wallet.Balance += paymentInfo.Amount;
                        await _unitOfWork.GetRepository<Wallet>().UpdateAsync(wallet);
                        await _unitOfWork.SaveAsync();
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while handling payment callback.", ex);
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
