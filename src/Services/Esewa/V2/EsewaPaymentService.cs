using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nepal.Payments.Gateways.Services.Esewa.V2
{
    /// <summary>
    /// eSewa payment service implementation for API version 2.
    /// </summary>
    public class EsewaPaymentService : IPaymentService
    {
        private readonly string _secretKey;
        private readonly PaymentMode _paymentMode;
        private readonly ApiService _apiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EsewaPaymentService"/> class.
        /// </summary>
        /// <param name="secretKey">The secret key for eSewa.</param>
        /// <param name="paymentMode">The payment mode (sandbox or production).</param>
        public EsewaPaymentService(string secretKey, PaymentMode paymentMode)
        {
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            _paymentMode = paymentMode;
            _apiService = new ApiService(new HttpClient());
        }

        /// <summary>
        /// Initiates a payment transaction asynchronously for eSewa V2.
        /// </summary>
        /// <typeparam name="T">The type of response expected.</typeparam>
        /// <param name="content">The payment request content.</param>
        /// <param name="version">The payment gateway API version.</param>
        /// <returns>A task that represents the asynchronous operation and contains the response.</returns>
        public async Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version)
        {
            if (!(content is EsewaRequest request))
                throw new ArgumentException("Content must be of type EsewaRequest", nameof(content));

            try
            {
                // Generate signature for eSewa V2
                string signature = GenerateEsewaV2Signature(request);
                request.Signature = signature;

                // Prepare form data for eSewa V2
                var formData = new Dictionary<string, string>
                {
                    ["amount"] = request.Amount.ToString("F2"),
                    ["tax_amount"] = request.TaxAmount.ToString("F2"),
                    ["total_amount"] = request.TotalAmount.ToString("F2"),
                    ["transaction_uuid"] = request.TransactionUuid,
                    ["product_code"] = request.ProductCode,
                    ["product_service_charge"] = request.ProductServiceCharge.ToString("F2"),
                    ["product_delivery_charge"] = request.ProductDeliveryCharge.ToString("F2"),
                    ["success_url"] = request.SuccessUrl,
                    ["failure_url"] = request.FailureUrl,
                    ["signed_field_names"] = request.SignedFieldNames,
                    ["signature"] = signature
                };

                // Get the appropriate endpoint
                string baseUrl = _paymentMode == PaymentMode.Sandbox 
                    ? ApiEndpoints.Esewa.V2.SandboxBaseUrl 
                    : ApiEndpoints.Esewa.V2.BaseUrl;
                
                string endpoint = $"{baseUrl}{ApiEndpoints.Esewa.V2.ProcessPaymentUrl}";

                // Make the API call
                var response = await _apiService.GetAsyncResult<T>(
                    endpoint,
                    ApiEndpoints.Esewa.V2.ProcessPaymentMethod,
                    keyValuePairs: formData
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to initiate eSewa V2 payment: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Verifies a payment transaction asynchronously for eSewa V2.
        /// </summary>
        /// <typeparam name="T">The type of response expected.</typeparam>
        /// <param name="content">The payment verification content (encoded response from eSewa).</param>
        /// <param name="version">The payment gateway API version.</param>
        /// <returns>A task that represents the asynchronous operation and contains the verification response.</returns>
        public Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Verification content cannot be null or empty", nameof(content));

            try
            {
                // For eSewa V2, the content is typically a base64 encoded response
                // that needs to be decoded and verified
                string decodedContent = DecodeBase64Content(content);
                
                // Parse the decoded content to extract transaction details
                var transactionData = ParseEsewaV2Response(decodedContent);
                
                // Verify the signature
                bool isValid = VerifyEsewaV2Signature(transactionData);
                
                if (!isValid)
                {
                    throw new InvalidOperationException("Invalid signature in eSewa V2 response");
                }

                // Return the parsed response
                return Task.FromResult((T)Convert.ChangeType(transactionData, typeof(T)));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to verify eSewa V2 payment: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Generates HMAC SHA256 signature for eSewa V2 request.
        /// </summary>
        /// <param name="request">The eSewa request object.</param>
        /// <returns>The generated signature.</returns>
        private string GenerateEsewaV2Signature(EsewaRequest request)
        {
            // Create the message to sign based on signed field names
            var signedFields = request.SignedFieldNames.Split(',');
            var messageParts = new List<string>();

            foreach (var field in signedFields)
            {
                var trimmedField = field.Trim();
                switch (trimmedField.ToLower())
                {
                    case "total_amount":
                        messageParts.Add($"total_amount={request.TotalAmount:F2}");
                        break;
                    case "transaction_uuid":
                        messageParts.Add($"transaction_uuid={request.TransactionUuid}");
                        break;
                    case "product_code":
                        messageParts.Add($"product_code={request.ProductCode}");
                        break;
                }
            }

            string message = string.Join(",", messageParts);
            return HmacHelper.GenerateHmacSha256Signature(message, _secretKey);
        }

        /// <summary>
        /// Verifies the signature in eSewa V2 response.
        /// </summary>
        /// <param name="transactionData">The transaction data to verify.</param>
        /// <returns>True if signature is valid, false otherwise.</returns>
        private bool VerifyEsewaV2Signature(object transactionData)
        {
            // Implementation depends on the actual eSewa V2 response format
            // This is a placeholder - actual implementation would parse the response
            // and verify the signature using the same algorithm
            return true; // Placeholder
        }

        /// <summary>
        /// Decodes base64 encoded content from eSewa.
        /// </summary>
        /// <param name="encodedContent">The base64 encoded content.</param>
        /// <returns>The decoded content.</returns>
        private string DecodeBase64Content(string encodedContent)
        {
            try
            {
                byte[] data = Convert.FromBase64String(encodedContent);
                return System.Text.Encoding.UTF8.GetString(data);
            }
            catch
            {
                // If not base64, return as is
                return encodedContent;
            }
        }

        /// <summary>
        /// Parses eSewa V2 response data.
        /// </summary>
        /// <param name="responseData">The response data to parse.</param>
        /// <returns>Parsed transaction data.</returns>
        private object ParseEsewaV2Response(string responseData)
        {
            // Implementation depends on the actual eSewa V2 response format
            // This is a placeholder - actual implementation would parse the response
            return responseData; // Placeholder
        }
    }
}
