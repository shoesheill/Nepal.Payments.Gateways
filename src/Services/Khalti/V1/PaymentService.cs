using System;
using System.Net.Http;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Helper.ApiCall;
using Nepal.Payments.Gateways.Interfaces;

namespace Nepal.Payments.Gateways.Services.Khalti.V1
{
    public class PaymentService : IPaymentService
    {
        private readonly string _secretKey;
        private readonly PaymentMode _paymentMode;
        private readonly ApiService _apiService;
        public PaymentService(string secretKey, PaymentMode paymentMode)
        {
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            _paymentMode = paymentMode;
            _apiService = new ApiService(new HttpClient());
        }

        public Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version)
        {
            throw new NotImplementedException("Khalti V1 API is not supported. Please use Khalti V2 API.");
        }
        public Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version)
        {
            throw new NotImplementedException("Khalti V1 API is not supported. Please use Khalti V2 API.");
        }
    }
}
