using System;
using System.Net.Http;
using Nepal.Payments.Gateways.Constants;
using Nepal.Payments.Gateways.Enum;

namespace Nepal.Payments.Gateways.Factories
{
    public static class PaymentEndpointFactory
    {
        public static (string apiUrl, HttpMethod httpMethod) GetEndpoint(PaymentMethod paymentMethod, PaymentVersion version, PaymentAction paymentAction, PaymentMode paymentMode)
        {
            return (paymentMethod, version, paymentMode, paymentAction) switch
            {
                // eSewa V2 endpoints
                (PaymentMethod.Esewa, PaymentVersion.V2, PaymentMode.Production, PaymentAction.ProcessPayment) => 
                    (ApiEndpoints.Esewa.V2.BaseUrl + ApiEndpoints.Esewa.V2.ProcessPaymentUrl, ApiEndpoints.Esewa.V2.ProcessPaymentMethod),
                
                (PaymentMethod.Esewa, PaymentVersion.V2, PaymentMode.Production, PaymentAction.VerifyPayment) => 
                    (ApiEndpoints.Esewa.V2.BaseUrl + ApiEndpoints.Esewa.V2.VerifyPaymentUrl, ApiEndpoints.Esewa.V2.VerifyPaymentMethod),
                
                (PaymentMethod.Esewa, PaymentVersion.V2, PaymentMode.Sandbox, PaymentAction.ProcessPayment) => 
                    (ApiEndpoints.Esewa.V2.SandboxBaseUrl + ApiEndpoints.Esewa.V2.ProcessPaymentUrl, ApiEndpoints.Esewa.V2.ProcessPaymentMethod),
                
                (PaymentMethod.Esewa, PaymentVersion.V2, PaymentMode.Sandbox, PaymentAction.VerifyPayment) => 
                    (ApiEndpoints.Esewa.V2.SandboxBaseUrl + ApiEndpoints.Esewa.V2.VerifyPaymentUrl, ApiEndpoints.Esewa.V2.VerifyPaymentMethod),

                // Khalti V2 endpoints
                (PaymentMethod.Khalti, PaymentVersion.V2, PaymentMode.Sandbox, PaymentAction.ProcessPayment) => 
                    (ApiEndpoints.Khalti.V2.SandboxBaseUrl + ApiEndpoints.Khalti.V2.ProcessPaymentUrl, ApiEndpoints.Khalti.V2.ProcessPaymentMethod),
                
                (PaymentMethod.Khalti, PaymentVersion.V2, PaymentMode.Sandbox, PaymentAction.VerifyPayment) => 
                    (ApiEndpoints.Khalti.V2.SandboxBaseUrl + ApiEndpoints.Khalti.V2.VerifyPaymentUrl, ApiEndpoints.Khalti.V2.VerifyPaymentMethod),
                
                (PaymentMethod.Khalti, PaymentVersion.V2, PaymentMode.Production, PaymentAction.ProcessPayment) => 
                    (ApiEndpoints.Khalti.V2.BaseUrl + ApiEndpoints.Khalti.V2.ProcessPaymentUrl, ApiEndpoints.Khalti.V2.ProcessPaymentMethod),
                
                (PaymentMethod.Khalti, PaymentVersion.V2, PaymentMode.Production, PaymentAction.VerifyPayment) => 
                    (ApiEndpoints.Khalti.V2.BaseUrl + ApiEndpoints.Khalti.V2.VerifyPaymentUrl, ApiEndpoints.Khalti.V2.VerifyPaymentMethod),

                // Unsupported combinations
                _ => throw new ArgumentException($"The combination of {paymentMethod}, {version}, {paymentMode}, and {paymentAction} is not supported.", nameof(paymentMethod)),
            };
        }
    }
}
