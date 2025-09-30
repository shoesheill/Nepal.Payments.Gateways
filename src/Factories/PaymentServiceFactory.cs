using System;

namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Factory class for creating payment service instances based on payment method and version.
    /// </summary>
    public static class PaymentServiceFactory
    {
        /// <summary>
        /// Creates a payment service instance for the specified payment method and version.
        /// </summary>
        /// <param name="paymentMethod">The payment gateway method.</param>
        /// <param name="version">The API version.</param>
        /// <param name="secretKey">The secret key for the payment gateway.</param>
        /// <param name="paymentMode">The payment mode (sandbox or production).</param>
        /// <returns>An instance of the appropriate payment service.</returns>
        /// <exception cref="ArgumentException">Thrown when the payment method or version is not supported.</exception>
        /// <exception cref="ArgumentNullException">Thrown when secretKey is null or empty.</exception>
        public static IPaymentService GetPaymentService(PaymentMethod paymentMethod, PaymentVersion version, string secretKey, PaymentMode paymentMode)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null or empty.");

            return (paymentMethod, version) switch
            {
                (PaymentMethod.Esewa, PaymentVersion.V1) => new Services.Esewa.V1.EsewaPaymentService(secretKey, paymentMode),
                (PaymentMethod.Esewa, PaymentVersion.V2) => new Services.Esewa.V2.EsewaPaymentService(secretKey, paymentMode),
                (PaymentMethod.Khalti, PaymentVersion.V1) => new Services.Khalti.V1.KhaltiPaymentService(secretKey, paymentMode),
                (PaymentMethod.Khalti, PaymentVersion.V2) => new Services.Khalti.V2.KhaltiPaymentService(secretKey, paymentMode),
                _ => throw new ArgumentException($"The combination of {paymentMethod} and {version} is not supported.", nameof(paymentMethod)),
            };
        }
    }
}
