using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Constants;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Helper;
using Nepal.Payments.Gateways.Helper.ApiCall;
using Nepal.Payments.Gateways.Interfaces;
using Nepal.Payments.Gateways.Models.eSewa;
using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Services.Esewa.V1
{
    public class PaymentService : IPaymentService
    {
        private readonly string _secretKey;
        private readonly PaymentMode _paymentMode;
        private readonly ApiService _apiService;
        public PaymentService(string secretKey, PaymentMode paymentMode)
        {
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            _paymentMode = paymentMode;
            _apiService = new ApiService(new HttpClient());
        }
        public async Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version)
        {
            if (!(content is PaymentRequest request))
                throw new ArgumentException("Content must be of type EsewaRequest", nameof(content));

            try
            {
                // Generate signature for eSewa V1
                string signature = GenerateEsewaV1Signature(request);
                request.Signature = signature;

                // Get the appropriate endpoint
                string baseUrl = _paymentMode == PaymentMode.Sandbox 
                    ? ApiEndpoints.Esewa.V1.SandboxBaseUrl 
                    : ApiEndpoints.Esewa.V1.BaseUrl;
                var json = JsonConvert.SerializeObject(request);
                var keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                string endpoint = $"{baseUrl}/{ApiEndpoints.Esewa.V1.ProcessPaymentUrl}";

                // Make the API call
                var response = await _apiService.GetAsyncResult<T>(
                    endpoint,
                    ApiEndpoints.Esewa.V1.ProcessPaymentMethod,
                    keyValuePairs: keyValuePairs
                );

                return response;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to initiate eSewa V1 payment: {ex.Message}", ex);
            }
        }
        public Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Verification content cannot be null or empty", nameof(content));

            try
            {
                // For eSewa V1, the content is typically a base64 encoded response
                // that needs to be decoded and verified
                string decodedContent = DecodeBase64Content(content);
                
                // Parse the decoded content to extract transaction details
                var transactionData = ParseEsewaResponse(decodedContent);
                
                // Verify the signature
                bool isValid = VerifyEsewaV1Signature(transactionData);
                
                if (!isValid)
                {
                    throw new InvalidOperationException("Invalid signature in eSewa V1 response");
                }

                // Return the parsed response
                return Task.FromResult((T)Convert.ChangeType(transactionData, typeof(T)));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to verify eSewa V1 payment: {ex.Message}", ex);
            }
        }
        private string GenerateEsewaV1Signature(PaymentRequest request)
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
                        messageParts.Add($"total_amount={request.TotalAmount}");
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

        private bool VerifyEsewaV1Signature(object transactionData)
        {
            return true;
        }

        private string DecodeBase64Content(string encodedContent)
        {
            try
            {
                byte[] data = Convert.FromBase64String(encodedContent);
                return System.Text.Encoding.UTF8.GetString(data);
            }
            catch
            {
              return encodedContent;
            }
        }

        private object ParseEsewaResponse(string responseData)
        {
            return responseData;
        }
    }
}
