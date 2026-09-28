using System;
using System.Net.Http;
using Nepal.Payments.Gateways.Constants;
using Nepal.Payments.Gateways.Enum;

namespace Nepal.Payments.Gateways.Factories
{
    public static class PaymentEndpointFactory
    {
        public static (string apiUrl, HttpMethod httpMethod) GetEndpoint(PaymentMethod paymentMethod, PaymentVersion version, PaymentAction paymentAction, PaymentMode paymentMode)
            => GetEndpoint(paymentMethod, version, paymentAction, paymentMode, null);

        /// <param name="baseUrlOverride">Replaces the built-in host (e.g. a proxy, or a provider host change) when set.</param>
        public static (string apiUrl, HttpMethod httpMethod) GetEndpoint(PaymentMethod paymentMethod, PaymentVersion version, PaymentAction paymentAction, PaymentMode paymentMode, string baseUrlOverride)
        {
            var sandbox = paymentMode == PaymentMode.Sandbox;
            var (baseUrl, path, method) = (paymentMethod, version, paymentAction) switch
            {
                (PaymentMethod.Esewa, PaymentVersion.V2, PaymentAction.ProcessPayment) =>
                    (sandbox ? ApiEndpoints.Esewa.V2.SandboxBaseUrl : ApiEndpoints.Esewa.V2.BaseUrl, ApiEndpoints.Esewa.V2.ProcessPaymentUrl, ApiEndpoints.Esewa.V2.ProcessPaymentMethod),
                (PaymentMethod.Esewa, PaymentVersion.V2, _) =>
                    (sandbox ? ApiEndpoints.Esewa.V2.SandboxBaseUrl : ApiEndpoints.Esewa.V2.BaseUrl, ApiEndpoints.Esewa.V2.VerifyPaymentUrl, ApiEndpoints.Esewa.V2.VerifyPaymentMethod),

                (PaymentMethod.Khalti, PaymentVersion.V2, PaymentAction.ProcessPayment) =>
                    (sandbox ? ApiEndpoints.Khalti.V2.SandboxBaseUrl : ApiEndpoints.Khalti.V2.BaseUrl, ApiEndpoints.Khalti.V2.ProcessPaymentUrl, ApiEndpoints.Khalti.V2.ProcessPaymentMethod),
                (PaymentMethod.Khalti, PaymentVersion.V2, _) =>
                    (sandbox ? ApiEndpoints.Khalti.V2.SandboxBaseUrl : ApiEndpoints.Khalti.V2.BaseUrl, ApiEndpoints.Khalti.V2.VerifyPaymentUrl, ApiEndpoints.Khalti.V2.VerifyPaymentMethod),

                (PaymentMethod.FonePay, _, PaymentAction.ProcessPayment) =>
                    (sandbox ? ApiEndpoints.Fonepay.SandboxBaseUrl : ApiEndpoints.Fonepay.BaseUrl, ApiEndpoints.Fonepay.QrGenerateUrl, ApiEndpoints.Fonepay.QrGenerateMethod),
                (PaymentMethod.FonePay, _, PaymentAction.VerifyPayment) =>
                    (sandbox ? ApiEndpoints.Fonepay.SandboxBaseUrl : ApiEndpoints.Fonepay.BaseUrl, ApiEndpoints.Fonepay.QrStatusUrl, ApiEndpoints.Fonepay.QrStatusMethod),

                _ => throw new ArgumentException($"The combination of {paymentMethod}, {version}, {paymentMode}, and {paymentAction} is not supported.", nameof(paymentMethod)),
            };

            return (Combine(string.IsNullOrWhiteSpace(baseUrlOverride) ? baseUrl : baseUrlOverride, path), method);
        }

        internal static string Combine(string baseUrl, string path) => baseUrl.TrimEnd('/') + "/" + path.TrimStart('/');
    }
}
