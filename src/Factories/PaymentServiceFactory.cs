using System;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Interfaces;
using Nepal.Payments.Gateways.Services.Esewa.V2;

namespace Nepal.Payments.Gateways.Factories
{
    public static class PaymentServiceFactory
    {
        public static IPaymentService GetPaymentService(PaymentMethod paymentMethod, PaymentVersion version, string secretKey, PaymentMode paymentMode)
        {
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException(nameof(secretKey), "Secret key cannot be null or empty.");

            return (paymentMethod, version) switch
            {
                (PaymentMethod.Esewa, PaymentVersion.V1) => new Services.Esewa.V1.PaymentService(secretKey, paymentMode),
                (PaymentMethod.Esewa, PaymentVersion.V2) => new PaymentService(secretKey, paymentMode),
                (PaymentMethod.Khalti, PaymentVersion.V1) => new Services.Khalti.V1.PaymentService(secretKey, paymentMode),
                (PaymentMethod.Khalti, PaymentVersion.V2) => new Services.Khalti.V2.PaymentService(secretKey, paymentMode),
                _ => throw new ArgumentException($"The combination of {paymentMethod} and {version} is not supported.", nameof(paymentMethod)),
            };
        }
    }
}
