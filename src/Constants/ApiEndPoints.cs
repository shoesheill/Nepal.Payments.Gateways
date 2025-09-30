using System.Net.Http;

namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Contains API endpoints for different payment gateways and their versions.
    /// </summary>
    public static class ApiEndpoints
    {
        /// <summary>
        /// eSewa payment gateway API endpoints.
        /// </summary>
        public static class Esewa
        {
            /// <summary>
            /// eSewa API v1 endpoints.
            /// </summary>
            public static class V1
            {
                /// <summary>
                /// Production base URL for eSewa v1 API.
                /// </summary>
                public const string BaseUrl = "https://epay.esewa.com.np/api";
                
                /// <summary>
                /// Sandbox base URL for eSewa v1 API.
                /// </summary>
                public const string SandboxBaseUrl = "https://rc-epay.esewa.com.np/api";
                
                /// <summary>
                /// Payment processing endpoint for eSewa v1.
                /// </summary>
                public const string ProcessPaymentUrl = "v1/payment/process";
                
                /// <summary>
                /// Payment verification endpoint for eSewa v1.
                /// </summary>
                public const string VerifyPaymentUrl = "v1/payment/verify";
                
                /// <summary>
                /// Payment status check endpoint for eSewa v1.
                /// </summary>
                public const string PaymentCheckUrl = "v1/payment/check";
                
                /// <summary>
                /// HTTP method for payment processing.
                /// </summary>
                public static readonly HttpMethod ProcessPaymentMethod = HttpMethod.Post;
                
                /// <summary>
                /// HTTP method for payment verification.
                /// </summary>
                public static readonly HttpMethod VerifyPaymentMethod = HttpMethod.Get;
                
                /// <summary>
                /// HTTP method for payment status check.
                /// </summary>
                public static readonly HttpMethod PaymentCheckMethod = HttpMethod.Get;
            }

            /// <summary>
            /// eSewa API v2 endpoints.
            /// </summary>
            public static class V2
            {
                /// <summary>
                /// Production base URL for eSewa v2 API.
                /// </summary>
                public const string BaseUrl = "https://epay.esewa.com.np/api/epay/main/v2/form";
                
                /// <summary>
                /// Sandbox base URL for eSewa v2 API.
                /// </summary>
                public const string SandboxBaseUrl = "https://rc-epay.esewa.com.np/api";
                
                /// <summary>
                /// Payment processing endpoint for eSewa v2.
                /// </summary>
                public const string ProcessPaymentUrl = "/epay/main/v2/form";
                
                /// <summary>
                /// Payment verification endpoint for eSewa v2.
                /// </summary>
                public const string VerifyPaymentUrl = "/epay/transaction/status/";
                
                /// <summary>
                /// Payment status check endpoint for eSewa v2.
                /// </summary>
                public const string PaymentCheckUrl = "/epay/transaction/status/";
                
                /// <summary>
                /// HTTP method for payment processing.
                /// </summary>
                public static readonly HttpMethod ProcessPaymentMethod = HttpMethod.Post;
                
                /// <summary>
                /// HTTP method for payment verification.
                /// </summary>
                public static readonly HttpMethod VerifyPaymentMethod = HttpMethod.Get;
                
                /// <summary>
                /// HTTP method for payment status check.
                /// </summary>
                public static readonly HttpMethod PaymentCheckMethod = HttpMethod.Get;
            }
        }

        /// <summary>
        /// Khalti payment gateway API endpoints.
        /// </summary>
        public static class Khalti
        {
            /// <summary>
            /// Khalti API v1 endpoints.
            /// </summary>
            public static class V1
            {
                /// <summary>
                /// Production base URL for Khalti v1 API.
                /// </summary>
                public const string BaseUrl = "https://api.khalti.com/";
                
                /// <summary>
                /// Sandbox base URL for Khalti v1 API.
                /// </summary>
                public const string SandboxBaseUrl = "https://a.khalti.com/api/";
                
                /// <summary>
                /// Payment processing endpoint for Khalti v1.
                /// </summary>
                public const string ProcessPaymentUrl = "v1/payment/process";
                
                /// <summary>
                /// Payment verification endpoint for Khalti v1.
                /// </summary>
                public const string VerifyPaymentUrl = "v1/payment/verify";
                
                /// <summary>
                /// Payment status check endpoint for Khalti v1.
                /// </summary>
                public const string PaymentCheckUrl = "v1/payment/check";
                
                /// <summary>
                /// HTTP method for payment processing.
                /// </summary>
                public static readonly HttpMethod ProcessPaymentMethod = HttpMethod.Post;
                
                /// <summary>
                /// HTTP method for payment verification.
                /// </summary>
                public static readonly HttpMethod VerifyPaymentMethod = HttpMethod.Post;
                
                /// <summary>
                /// HTTP method for payment status check.
                /// </summary>
                public static readonly HttpMethod PaymentCheckMethod = HttpMethod.Get;
            }

            /// <summary>
            /// Khalti API v2 endpoints.
            /// </summary>
            public static class V2
            {
                /// <summary>
                /// Production base URL for Khalti v2 API.
                /// </summary>
                public const string BaseUrl = "https://api.khalti.com/";
                
                /// <summary>
                /// Sandbox base URL for Khalti v2 API.
                /// </summary>
                public const string SandboxBaseUrl = "https://a.khalti.com/api/";
                
                /// <summary>
                /// Payment processing endpoint for Khalti v2.
                /// </summary>
                public const string ProcessPaymentUrl = "epayment/initiate/";
                
                /// <summary>
                /// Payment verification endpoint for Khalti v2.
                /// </summary>
                public const string VerifyPaymentUrl = "epayment/lookup/";
                
                /// <summary>
                /// Payment status check endpoint for Khalti v2.
                /// </summary>
                public const string PaymentCheckUrl = "epayment/lookup/";
                
                /// <summary>
                /// HTTP method for payment processing.
                /// </summary>
                public static readonly HttpMethod ProcessPaymentMethod = HttpMethod.Post;
                
                /// <summary>
                /// HTTP method for payment verification.
                /// </summary>
                public static readonly HttpMethod VerifyPaymentMethod = HttpMethod.Post;
                
                /// <summary>
                /// HTTP method for payment status check.
                /// </summary>
                public static readonly HttpMethod PaymentCheckMethod = HttpMethod.Get;
            }
        }
    }
}
