using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace payment_gateway_nepal.khalti.V1
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
