using System;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Interfaces;
using Nepal.Payments.Gateways.Services.Esewa.V2;

namespace Nepal.Payments.Gateways.Factories
{
    public static class PaymentServiceFactory
    {
        public static IPaymentService GetPaymentService(PaymentMethod paymentMethod, PaymentVersion version, string secretKey, PaymentMode paymentMode)
            => GetPaymentService(paymentMethod, version, secretKey, paymentMode, null);

        /// <param name="baseUrl">Overrides the built-in host for eSewa V2 and Khalti V2.</param>
        public static IPaymentService GetPaymentService(PaymentMethod paymentMethod, PaymentVersion version, string secretKey, PaymentMode paymentMode, string baseUrl)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null or empty.");

            return (paymentMethod, version) switch
            {
                (PaymentMethod.Esewa, PaymentVersion.V1) => new Services.Esewa.V1.PaymentService(secretKey, paymentMode),
                (PaymentMethod.Esewa, PaymentVersion.V2) => new PaymentService(secretKey, paymentMode, baseUrl),
                (PaymentMethod.Khalti, PaymentVersion.V1) => new Services.Khalti.V1.PaymentService(secretKey, paymentMode),
                (PaymentMethod.Khalti, PaymentVersion.V2) => new Services.Khalti.V2.PaymentService(secretKey, paymentMode, baseUrl),
                (PaymentMethod.FonePay, _) => new Services.Fonepay.PaymentService(secretKey, paymentMode),
                _ => throw new ArgumentException($"The combination of {paymentMethod} and {version} is not supported.", nameof(paymentMethod)),
            };
        }
    }
}
