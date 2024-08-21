using System;
using System.Net.Http;

namespace payment_gateway_nepal
{
    public class PaymentEndpointFactory
    {
        public static (string apiUrl, HttpMethod httpMethod) GetEndpoint(PaymentMethod paymentMethod, PaymentVersion version, PaymentAction paymentAction, PaymentMode paymentMode)
        {
            return (paymentMethod, version, paymentMode, paymentAction) switch
            {
                (PaymentMethod.eSewa, PaymentVersion.v2, PaymentMode.Production, PaymentAction.ProcessPayment) => (ApiEndPoints.eSewa.V2.BaseUrl + ApiEndPoints.eSewa.V2.ProcessPaymentUrl, ApiEndPoints.eSewa.V2.ProcessPaymentMethod),
                (PaymentMethod.eSewa, PaymentVersion.v2, PaymentMode.Production, PaymentAction.VerifyPayment) => (ApiEndPoints.eSewa.V2.BaseUrl + ApiEndPoints.eSewa.V2.VerifyPaymentMethod, ApiEndPoints.eSewa.V2.VerifyPaymentMethod),
                (PaymentMethod.eSewa, PaymentVersion.v2, PaymentMode.Sandbox, PaymentAction.ProcessPayment) => (ApiEndPoints.eSewa.V2.SandboxBaseUrl + ApiEndPoints.eSewa.V2.ProcessPaymentUrl, ApiEndPoints.eSewa.V2.ProcessPaymentMethod),
                (PaymentMethod.eSewa, PaymentVersion.v2, PaymentMode.Sandbox, PaymentAction.VerifyPayment) => (ApiEndPoints.eSewa.V2.SandboxBaseUrl + ApiEndPoints.eSewa.V2.VerifyPaymentUrl, ApiEndPoints.eSewa.V2.VerifyPaymentMethod),
                _ => throw new ArgumentException("Invalid gateway, version, or action", nameof(paymentMethod)),
            };
        }
    }
}
