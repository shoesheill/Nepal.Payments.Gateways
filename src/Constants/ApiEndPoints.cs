using System.Net.Http;

namespace Nepal.Payments.Gateways.Constants
{
    public static class ApiEndpoints
    {
        public static class Esewa
        {
            public static class V1
            {
                public const string BaseUrl = "https://epay.esewa.com.np/api";
                public const string SandboxBaseUrl = "https://rc-epay.esewa.com.np/api";
                public const string ProcessPaymentUrl = "v1/payment/process";
                public const string VerifyPaymentUrl = "v1/payment/verify";
                public const string PaymentCheckUrl = "v1/payment/check";
                public static readonly HttpMethod ProcessPaymentMethod = HttpMethod.Post;
                public static readonly HttpMethod VerifyPaymentMethod = HttpMethod.Get;
                public static readonly HttpMethod PaymentCheckMethod = HttpMethod.Get;
            }
            public static class V2
            {
                public const string BaseUrl = "https://epay.esewa.com.np/api/epay/main/v2/form";
                public const string SandboxBaseUrl = "https://rc-epay.esewa.com.np/api";
                public const string ProcessPaymentUrl = "/epay/main/v2/form";
                public const string VerifyPaymentUrl = "/epay/transaction/status/";
                public const string PaymentCheckUrl = "/epay/transaction/status/";
                public static readonly HttpMethod ProcessPaymentMethod = HttpMethod.Post;
                public static readonly HttpMethod VerifyPaymentMethod = HttpMethod.Get;
                public static readonly HttpMethod PaymentCheckMethod = HttpMethod.Get;
            }
        }
        public static class Khalti
        {
            public static class V1
            {
                public const string BaseUrl = "https://api.khalti.com/";
                public const string SandboxBaseUrl = "https://a.khalti.com/api/";
                public const string ProcessPaymentUrl = "v1/payment/process";
                public const string VerifyPaymentUrl = "v1/payment/verify";
                public const string PaymentCheckUrl = "v1/payment/check";
                public static readonly HttpMethod ProcessPaymentMethod = HttpMethod.Post;
                public static readonly HttpMethod VerifyPaymentMethod = HttpMethod.Post;
                public static readonly HttpMethod PaymentCheckMethod = HttpMethod.Get;
            }

            public static class V2
            {
                public const string BaseUrl = "https://api.khalti.com/";
                public const string SandboxBaseUrl = "https://a.khalti.com/api/";
                public const string ProcessPaymentUrl = "epayment/initiate/";
                public const string VerifyPaymentUrl = "epayment/lookup/";
                public const string PaymentCheckUrl = "epayment/lookup/";
                public static readonly HttpMethod ProcessPaymentMethod = HttpMethod.Post;
                public static readonly HttpMethod VerifyPaymentMethod = HttpMethod.Post;
                public static readonly HttpMethod PaymentCheckMethod = HttpMethod.Get;
            }
        }
    }
}
