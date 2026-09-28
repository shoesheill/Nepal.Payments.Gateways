using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Factories;
using Nepal.Payments.Gateways.Helper;
using Nepal.Payments.Gateways.Helper.ApiCall;
using Nepal.Payments.Gateways.Interfaces;
using Nepal.Payments.Gateways.Models;
using Nepal.Payments.Gateways.Models.Khalti;

namespace Nepal.Payments.Gateways.Services.Khalti.V2
{
    internal class PaymentService : IPaymentService, IStatusCheckService
    {
        private readonly string _secretKey;
        private readonly PaymentMode _paymentMode;
        private readonly string _baseUrl;

        public PaymentService(string secretKey, PaymentMode paymentMode, string baseUrl = null)
        {
            _secretKey = secretKey;
            _paymentMode = paymentMode;
            _baseUrl = baseUrl;
        }

        public async Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version)
        {
            try
            {
                var (apiUrl, httpMethod) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Khalti, version, PaymentAction.ProcessPayment, _paymentMode, _baseUrl);
                var response = await new ApiService(new HttpClient()).GetAsyncResult<RequestResponse>(apiUrl, httpMethod, AuthHeader(), null, content);
                return ResponseConverter.ConvertTo<T>(new PaymentResult { Data = response, Success = true, Message = "Payment initiated successfully" });
            }
            catch (Exception ex)
            {
                return ResponseConverter.ConvertTo<T>(new PaymentResult { Success = false, Message = ex.Message });
            }
        }

        /// <summary>Khalti lookup by <c>pidx</c>. <c>total_amount</c> in the answer is paisa.</summary>
        public async Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version)
        {
            try
            {
                var (apiUrl, httpMethod) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Khalti, version, PaymentAction.VerifyPayment, _paymentMode, _baseUrl);
                var response = await new ApiService(new HttpClient()).GetAsyncResult<PaymentResponse>(apiUrl, httpMethod, AuthHeader(), null, new { pidx = content });
                return ResponseConverter.ConvertTo<T>(new PaymentResult { Data = response, Success = true, Message = "Payment verified successfully" });
            }
            catch (Exception ex)
            {
                return ResponseConverter.ConvertTo<T>(new PaymentResult { Success = false, Message = ex.Message });
            }
        }

        public Task<T> CheckStatusAsync<T>(object content)
        {
            if (!(content is string pidx) || string.IsNullOrEmpty(pidx))
                throw new ArgumentException("Content must be the payment's pidx", nameof(content));
            return VerifyPaymentAsync<T>(pidx, PaymentVersion.V2);
        }

        private Dictionary<string, string> AuthHeader() => new Dictionary<string, string> { ["Authorization"] = "key " + _secretKey };
    }
}
