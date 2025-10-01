using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Factories;
using Nepal.Payments.Gateways.Helper;
using Nepal.Payments.Gateways.Helper.ApiCall;
using Nepal.Payments.Gateways.Interfaces;
using Nepal.Payments.Gateways.Models;
using Nepal.Payments.Gateways.Models.Khalti;
using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Services.Khalti.V2
{ 
    internal class PaymentService : IPaymentService
    {
        private readonly string _secretKey;
        private readonly PaymentMode _paymentMode;


        public PaymentService(string secretKey, PaymentMode paymentMode)
        {
            _secretKey = secretKey;
            _paymentMode = paymentMode;
        }
        public async Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version)
        {
            try{
                var (apiUrl, httpMethod) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Khalti, version, PaymentAction.ProcessPayment, _paymentMode);
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", "key " + _secretKey);
                var response = await new ApiService(new HttpClient()).GetAsyncResult<RequestResponse>(apiUrl, httpMethod, headers, null, content);
                return ResponseConverter.ConvertTo<T>(new PaymentResult
                {
                    Data = response,
                    Success = true,
                    Message = "Payment initiated successfully"
                });
            }
            catch(Exception ex)
            {
                return ResponseConverter.ConvertTo<T>(new PaymentResult
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        public async Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version)
        {
            try
            {
                var (apiUrl, httpMethod) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Khalti, version, PaymentAction.VerifyPayment, _paymentMode);
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Authorization", "key " + _secretKey);
                var formContent = new Dictionary<string, string>
                {
                    { "pidx", content }
                };
                var response = await new ApiService(new HttpClient()).GetAsyncResult<Models.Khalti.PaymentResponse>(apiUrl, httpMethod, headers, formContent, null);
                return ResponseConverter.ConvertTo<T>(new PaymentResult
                {
                    Data = response,
                    Success = true,
                    Message = "Payment verified successfully"
                });
            }
            catch(Exception ex)
            {
                return ResponseConverter.ConvertTo<T>(new PaymentResult
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
    }
}
