using System.Threading.Tasks;

namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Interface for payment service that handles payment operations.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Initiates a payment transaction asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of response expected.</typeparam>
        /// <param name="content">The payment request content.</param>
        /// <param name="version">The payment gateway API version.</param>
        /// <returns>A task that represents the asynchronous operation and contains the response.</returns>
        Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version);

        /// <summary>
        /// Verifies a payment transaction asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of response expected.</typeparam>
        /// <param name="content">The payment verification content (usually a transaction ID or encoded data).</param>
        /// <param name="version">The payment gateway API version.</param>
        /// <returns>A task that represents the asynchronous operation and contains the verification response.</returns>
        Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version);
    }
}
