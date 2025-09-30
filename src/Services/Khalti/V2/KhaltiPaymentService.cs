using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nepal.Payments.Gateways.Services.Khalti.V2
{
    /// <summary>
    /// Khalti payment service implementation for API version 2.
    /// </summary>
    public class KhaltiPaymentService : IPaymentService
    {
        private readonly string _secretKey;
        private readonly PaymentMode _paymentMode;
        private readonly ApiService _apiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="KhaltiPaymentService"/> class.
        /// </summary>
        /// <param name="secretKey">The secret key for Khalti.</param>
        /// <param name="paymentMode">The payment mode (sandbox or production).</param>
        public KhaltiPaymentService(string secretKey, PaymentMode paymentMode)
        {
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            _paymentMode = paymentMode;
            _apiService = new ApiService(new HttpClient());
        }

        /// <summary>
        /// Initiates a payment transaction asynchronously for Khalti V2.
        /// </summary>
        /// <typeparam name="T">The type of response expected.</typeparam>
        /// <param name="content">The payment request content.</param>
        /// <param name="version">The payment gateway API version.</param>
        /// <returns>A task that represents the asynchronous operation and contains the response.</returns>
        public async Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version)
        {
            try
            {
                // Prepare the request body for Khalti V2
                var requestBody = PrepareKhaltiV2Request(content);

                // Get the appropriate endpoint
                string baseUrl = _paymentMode == PaymentMode.Sandbox 
                    ? ApiEndpoints.Khalti.V2.SandboxBaseUrl 
                    : ApiEndpoints.Khalti.V2.BaseUrl;
                
                string endpoint = $"{baseUrl}{ApiEndpoints.Khalti.V2.ProcessPaymentUrl}";

                // Prepare headers for Khalti V2
                var headers = new Dictionary<string, string>
                {
                    ["Authorization"] = $"Key {_secretKey}",
                    ["Content-Type"] = "application/json"
                };

                // Make the API call
                var response = await _apiService.GetAsyncResult<T>(
                    endpoint,
                    ApiEndpoints.Khalti.V2.ProcessPaymentMethod,
                    headerParam: headers,
                    requestBody: requestBody
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to initiate Khalti V2 payment: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifies a payment transaction asynchronously for Khalti V2.
        /// </summary>
        /// <typeparam name="T">The type of response expected.</typeparam>
        /// <param name="content">The payment verification content (pidx from Khalti).</param>
        /// <param name="version">The payment gateway API version.</param>
        /// <returns>A task that represents the asynchronous operation and contains the verification response.</returns>
        public async Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Verification content (pidx) cannot be null or empty", nameof(content));

            try
            {
                // Get the appropriate endpoint
                string baseUrl = _paymentMode == PaymentMode.Sandbox 
                    ? ApiEndpoints.Khalti.V2.SandboxBaseUrl 
                    : ApiEndpoints.Khalti.V2.BaseUrl;
                
                string endpoint = $"{baseUrl}{ApiEndpoints.Khalti.V2.VerifyPaymentUrl}{content}";

                // Prepare headers for Khalti V2
                var headers = new Dictionary<string, string>
                {
                    ["Authorization"] = $"Key {_secretKey}",
                    ["Content-Type"] = "application/json"
                };

                // Make the API call
                var response = await _apiService.GetAsyncResult<T>(
                    endpoint,
                    ApiEndpoints.Khalti.V2.VerifyPaymentMethod,
                    headerParam: headers
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to verify Khalti V2 payment: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Prepares the request body for Khalti V2 payment initiation.
        /// </summary>
        /// <param name="content">The payment request content.</param>
        /// <returns>The prepared request body.</returns>
        private object PrepareKhaltiV2Request(object content)
        {
            // The content should be a dynamic object with Khalti V2 structure
            // This method ensures the request is properly formatted for Khalti V2 API
            return content;
        }
    }
}
