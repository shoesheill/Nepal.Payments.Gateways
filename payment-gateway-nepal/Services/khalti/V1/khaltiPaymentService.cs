using System;
using System.Threading.Tasks;

namespace payment_gateway_nepal.khalti.V1
{
    internal class KhaltiPaymentService : IPaymentService
    {
        private readonly string _secretKey;
        private readonly PaymentMode _paymentMode;


        public KhaltiPaymentService(string secretKey, PaymentMode paymentMode)
        {
            _secretKey = secretKey;
            _paymentMode = paymentMode;
        }
        public Task<T> ProcessPayment<T>(object content, PaymentVersion version)
        {
            throw new NotImplementedException();
        }

        public Task<T> VerifyPayment<T>(string content, PaymentVersion version)
        {
            throw new NotImplementedException();
        }
    }
}
