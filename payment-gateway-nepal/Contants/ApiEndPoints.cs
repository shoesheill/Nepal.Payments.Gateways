using System.Net.Http;

namespace payment_gateway_nepal
{
    public static class ApiEndPoints
    {
        public static class eSewa
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
            public const string BaseUrl = "https://api.khalti.com/";

            public static class V1
            {
                public const string ProcessPaymentUrl = "v1/payment/process";
                public const string VerifyPaymentUrl = "v1/payment/verify";
                public const string PaymentCheckUrl = "v1/payment/check";
                public static readonly HttpMethod ProcessPaymentMethod = HttpMethod.Post;
                public static readonly HttpMethod VerifyPaymentMethod = HttpMethod.Get;
                public static readonly HttpMethod PaymentCheckMethod = HttpMethod.Get;
            }

            public static class V2
            {
                public const string ProcessPaymentUrl = "v2/payment/process";
                public const string VerifyPaymentUrl = "v2/payment/verify";
                public const string PaymentCheckUrl = "v2/payment/check";
                public static readonly HttpMethod ProcessPaymentMethod = HttpMethod.Post;
                public static readonly HttpMethod VerifyPaymentMethod = HttpMethod.Get;
                public static readonly HttpMethod PaymentCheckMethod = HttpMethod.Get;
            }
        }
    }
}
