using System;

namespace payment_gateway_nepal
{
    public static class PaymentServiceFactory
    {
        public static IPaymentService GetPaymentService(PaymentMethod paymentMethod, PaymentVersion version, string secretKey, PaymentMode paymentMode)
        {
            return (paymentMethod, version) switch
            {
                (PaymentMethod.eSewa, PaymentVersion.v1) => new payment_gateway_nepal.eSewa.V1.eSewaPaymentService(secretKey, paymentMode),
                (PaymentMethod.eSewa, PaymentVersion.v2) => new payment_gateway_nepal.eSewa.V2.eSewaPaymentService(secretKey, paymentMode),
                (PaymentMethod.Khalti, PaymentVersion.v1) => new payment_gateway_nepal.khalti.V1.KhaltiPaymentService(secretKey, paymentMode),
                (PaymentMethod.Khalti, PaymentVersion.v2) => new payment_gateway_nepal.khalti.V2.KhaltiPaymentService(secretKey, paymentMode),
                _ => throw new ArgumentException("Invalid gateway or version", nameof(paymentMethod)),
            };
        }
    }
}
