using System;
using System.Net.Http;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Helper.ApiCall;
using Nepal.Payments.Gateways.Interfaces;

namespace Nepal.Payments.Gateways.Services.Khalti.V1
{
    /// <summary>
    /// Khalti payment service implementation for API version 1.
    /// Note: V1 is not fully implemented by Khalti, this is a placeholder.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        private readonly string _secretKey;
        private readonly PaymentMode _paymentMode;
        private readonly ApiService _apiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentService"/> class.
        /// </summary>
        /// <param name="secretKey">The secret key for Khalti.</param>
        /// <param name="paymentMode">The payment mode (sandbox or production).</param>
        public PaymentService(string secretKey, PaymentMode paymentMode)
        {
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            _paymentMode = paymentMode;
            _apiService = new ApiService(new HttpClient());
        }

        /// <summary>
        /// Initiates a payment transaction asynchronously for Khalti V1.
        /// Note: Khalti V1 is not fully supported, this method throws NotImplementedException.
        /// </summary>
        /// <typeparam name="T">The type of response expected.</typeparam>
        /// <param name="content">The payment request content.</param>
        /// <param name="version">The payment gateway API version.</param>
        /// <returns>A task that represents the asynchronous operation and contains the response.</returns>
        /// <exception cref="NotImplementedException">Thrown because Khalti V1 is not supported.</exception>
        public Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version)
        {
            throw new NotImplementedException("Khalti V1 API is not supported. Please use Khalti V2 API.");
        }

        /// <summary>
        /// Verifies a payment transaction asynchronously for Khalti V1.
        /// Note: Khalti V1 is not fully supported, this method throws NotImplementedException.
        /// </summary>
        /// <typeparam name="T">The type of response expected.</typeparam>
        /// <param name="content">The payment verification content.</param>
        /// <param name="version">The payment gateway API version.</param>
        /// <returns>A task that represents the asynchronous operation and contains the verification response.</returns>
        /// <exception cref="NotImplementedException">Thrown because Khalti V1 is not supported.</exception>
        public Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version)
        {
            throw new NotImplementedException("Khalti V1 API is not supported. Please use Khalti V2 API.");
        }
    }
}
