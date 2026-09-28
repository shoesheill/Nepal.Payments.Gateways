using System;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Factories;
using Nepal.Payments.Gateways.Interfaces;

namespace Nepal.Payments.Gateways.Manager
{
    public class PaymentManager
    {
        private readonly string _secretKey;
        private readonly PaymentMethod _method;
        private readonly PaymentVersion _version;
        private readonly PaymentMode _mode;
        private readonly string _baseUrl;

        public PaymentManager(PaymentMethod paymentMethod, PaymentVersion paymentVersion, PaymentMode paymentMode, string secretKey)
            : this(paymentMethod, paymentVersion, paymentMode, secretKey, null) { }

        /// <param name="baseUrl">Optional host override for eSewa V2 / Khalti V2 (proxy, or a provider host change).</param>
        public PaymentManager(PaymentMethod paymentMethod, PaymentVersion paymentVersion, PaymentMode paymentMode, string secretKey, string baseUrl)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null or empty.");

            _secretKey = secretKey;
            _method = paymentMethod;
            _version = paymentVersion;
            _mode = paymentMode;
            _baseUrl = baseUrl;
        }
        public async Task<T> InitiatePaymentAsync<T>(object content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content), "Payment content cannot be null.");

            var paymentService = PaymentServiceFactory.GetPaymentService(_method, _version, _secretKey, _mode, _baseUrl);
            return await paymentService.InitiatePaymentAsync<T>(content, _version);
        }
        public async Task<T> VerifyPaymentAsync<T>(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content), "Verification content cannot be null or empty.");

            var paymentService = PaymentServiceFactory.GetPaymentService(_method, _version, _secretKey, _mode, _baseUrl);
            return await paymentService.VerifyPaymentAsync<T>(content, _version);
        }

        /// <summary>
        ///     Server-side status check: eSewa V2 takes a <see cref="Models.eSewa.StatusRequest" />,
        ///     Khalti V2 the payment's <c>pidx</c>.
        /// </summary>
        public async Task<T> CheckStatusAsync<T>(object content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content), "Status content cannot be null.");

            var paymentService = PaymentServiceFactory.GetPaymentService(_method, _version, _secretKey, _mode, _baseUrl);
            if (!(paymentService is IStatusCheckService statusService))
                throw new NotSupportedException($"{_method} {_version} does not support status checks.");

            return await statusService.CheckStatusAsync<T>(content);
        }
    }
}
