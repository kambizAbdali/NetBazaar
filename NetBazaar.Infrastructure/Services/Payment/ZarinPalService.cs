using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using NetBazaar.Application.DTOs.Payment;
using NetBazaar.Application.Interfaces.Payment;
using RestSharp;

using System.Threading.Tasks;
using ZstdSharp.Unsafe;
namespace NetBazaar.Infrastructure.Services.Payment
{
    public class ZarinPalService : IZarinPalService
    {
        private readonly IConfiguration _configuration;
        private readonly RestClient _client;

        public ZarinPalService(IConfiguration configuration)
        {
            _configuration = configuration;
            _client = new RestClient("https://api.zarinpal.com/pg/v4/payment/");
        }

        public async Task<ZarinPalPaymentResponse> CreatePaymentRequestAsync(ZarinPalPaymentRequest request)
        {
            var restRequest = new RestRequest("request.json", Method.Post);
            var body = new
            {
                merchant_id = request.MerchantId,
                amount = request.Amount,
                description = request.Description,
                callback_url = request.CallbackUrl,
                metadata = new { email = request.Email, mobile = request.Mobile }
            };
            restRequest.AddJsonBody(body);
            restRequest.AddHeader("Content-Type", "application/json");

            var response = await _client.ExecuteAsync<ZarinPalPaymentResponse>(restRequest);
            if (!response.IsSuccessful) throw new InvalidOperationException("خطا در ارتباط با درگاه پرداخت");
            return response.Data!;
        }

        public async Task<PaymentVerificationDto> VerifyPaymentAsync(string authority, int amount)
        {
            var restRequest = new RestRequest("verify.json", Method.Post);
            var body = new
            {
                merchant_id = _configuration["ZarinPal:MerchantId"],
                authority,
                amount
            };
            restRequest.AddJsonBody(body);
            restRequest.AddHeader("Content-Type", "application/json");

            var response = await _client.ExecuteAsync<dynamic>(restRequest);
            if (!response.IsSuccessful) throw new InvalidOperationException("خطا در تأیید پرداخت");

            int status = (int)response.Data.data.code;
            long refId = (long)response.Data.data.ref_id;

            return new PaymentVerificationDto { Status = status, RefId = refId };
        }
    }

}
