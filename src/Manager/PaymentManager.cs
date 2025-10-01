using System;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Factories;

namespace Nepal.Payments.Gateways.Manager
{
    public class PaymentManager
    {
        private readonly string _secretKey;
        private readonly PaymentMethod _method;
        private readonly PaymentVersion _version;
        private readonly PaymentMode _mode;
        public PaymentManager(PaymentMethod paymentMethod, PaymentVersion paymentVersion, PaymentMode paymentMode, string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null or empty.");

            _secretKey = secretKey;
            _method = paymentMethod;
            _version = paymentVersion;
            _mode = paymentMode;
        }
        public async Task<T> InitiatePaymentAsync<T>(object content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content), "Payment content cannot be null.");

            var paymentService = PaymentServiceFactory.GetPaymentService(_method, _version, _secretKey, _mode);
            return await paymentService.InitiatePaymentAsync<T>(content, _version);
        }
        public async Task<T> VerifyPaymentAsync<T>(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content), "Verification content cannot be null or empty.");

            var paymentService = PaymentServiceFactory.GetPaymentService(_method, _version, _secretKey, _mode);
            return await paymentService.VerifyPaymentAsync<T>(content, _version);
        }
    }
}
