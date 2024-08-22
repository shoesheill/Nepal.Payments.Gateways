using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace payment_gateway_nepal.khalti.V2
{
    internal class khaltiPaymentService : IPaymentService
    {
        private readonly string _secretKey;
        private readonly PaymentMode _paymentMode;


        public khaltiPaymentService(string secretKey, PaymentMode paymentMode)
        {
            _secretKey = secretKey;
            _paymentMode = paymentMode;
        }
        public async Task<T> ProcessPayment<T>(object content, PaymentVersion version)
        {
            var (apiUrl, httpMethod) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Khalti, version, PaymentAction.ProcessPayment, _paymentMode);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Authorization", "key " + _secretKey);
            string json = JsonConvert.SerializeObject(content);
            var response = await new ApiService(new HttpClient()).GetAsyncResult<string>(apiUrl, httpMethod, headers, null, json);
            ApiResponse apiResponse = new ApiResponse { data = response };
            return (T)Convert.ChangeType(apiResponse, typeof(T));
        }

        public Task<T> VerifyPayment<T>(string content, PaymentVersion version)
        {
            throw new NotImplementedException();
        }
    }
}
