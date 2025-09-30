using System;
using System.Threading.Tasks;

namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Main payment manager class that provides a unified interface for payment operations.
    /// </summary>
    public class PaymentManager
    {
        private readonly string _secretKey;
        private readonly PaymentMethod _method;
        private readonly PaymentVersion _version;
        private readonly PaymentMode _mode;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentManager"/> class.
        /// </summary>
        /// <param name="paymentMethod">The payment gateway method to use.</param>
        /// <param name="paymentVersion">The API version to use.</param>
        /// <param name="paymentMode">The payment mode (sandbox or production).</param>
        /// <param name="secretKey">The secret key for the payment gateway.</param>
        /// <exception cref="ArgumentNullException">Thrown when secretKey is null or empty.</exception>
        public PaymentManager(PaymentMethod paymentMethod, PaymentVersion paymentVersion, PaymentMode paymentMode, string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null or empty.");

            _secretKey = secretKey;
            _method = paymentMethod;
            _version = paymentVersion;
            _mode = paymentMode;
        }

        /// <summary>
        /// Initiates a payment transaction asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of response expected.</typeparam>
        /// <param name="content">The payment request content.</param>
        /// <returns>A task that represents the asynchronous operation and contains the response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when content is null.</exception>
        public async Task<T> InitiatePaymentAsync<T>(object content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content), "Payment content cannot be null.");

            var paymentService = PaymentServiceFactory.GetPaymentService(_method, _version, _secretKey, _mode);
            return await paymentService.InitiatePaymentAsync<T>(content, _version);
        }

        /// <summary>
        /// Verifies a payment transaction asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of response expected.</typeparam>
        /// <param name="content">The payment verification content (usually a transaction ID or encoded data).</param>
        /// <returns>A task that represents the asynchronous operation and contains the verification response.</returns>
        /// <exception cref="ArgumentNullException">Thrown when content is null or empty.</exception>
        public async Task<T> VerifyPaymentAsync<T>(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException(nameof(content), "Verification content cannot be null or empty.");

            var paymentService = PaymentServiceFactory.GetPaymentService(_method, _version, _secretKey, _mode);
            return await paymentService.VerifyPaymentAsync<T>(content, _version);
        }
    }
}
