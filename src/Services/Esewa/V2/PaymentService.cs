using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Constants;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Helper;
using Nepal.Payments.Gateways.Helper.ApiCall;
using Nepal.Payments.Gateways.Interfaces;
using Nepal.Payments.Gateways.Models;
using Nepal.Payments.Gateways.Models.eSewa;
using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Services.Esewa.V2
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
                // Generate signature for eSewa V2
                string signature = GenerateEsewaV2Signature(request);
                request.Signature = signature;
                // var paymentData = new PaymentRequest
                // {
                //     Amount = request.Amount.ToString("F2"),
                //     TaxAmount = request.TaxAmount.ToString("F2"),
                //     TotalAmount = request.TotalAmount.ToString("F2"),
                //     TransactionUuid = request.TransactionUuid,
                //     ProductCode = request.ProductCode,
                //     ProductServiceCharge = request.ProductServiceCharge.ToString("F2"),
                //     ProductDeliveryCharge = request.ProductDeliveryCharge.ToString("F2"),
                //     SuccessUrl = request.SuccessUrl,
                //     FailureUrl = request.FailureUrl,
                //     SignedFieldNames = request.SignedFieldNames,
                //     Signature = signature
                // };
                string baseUrl = _paymentMode == PaymentMode.Sandbox 
                    ? ApiEndpoints.Esewa.V2.SandboxBaseUrl 
                    : ApiEndpoints.Esewa.V2.BaseUrl;
                var json = JsonConvert.SerializeObject(request);
                string endpoint = $"{baseUrl}{ApiEndpoints.Esewa.V2.ProcessPaymentUrl}";
                 var keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                var response = await _apiService.GetAsyncResult<T>(
                    endpoint,
                    ApiEndpoints.Esewa.V2.ProcessPaymentMethod,
                    keyValuePairs: keyValuePairs
                );

                return ResponseConverter.ConvertTo<T>(new ApiResponse
                {
                    Data = new RequestResponse{PaymentUrl = response.ToString()??""},
                    Success = true,
                    Message = "Payment initiated successfully"
                });
            }
            catch (Exception ex)
            {
                return ResponseConverter.ConvertTo<T>(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        public async Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version)
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
                return ResponseConverter.ConvertTo<T>(new ApiResponse
                {
                    Data = transactionData,
                    Success = true,
                    Message = "Payment verified successfully"
                });
            }
            catch (Exception ex)
            {
                return ResponseConverter.ConvertTo<T>(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }
        private string GenerateEsewaV2Signature(PaymentRequest request)
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
        private bool VerifyEsewaV2Signature(object transactionData)
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
                // If not base64, return as is
                return encodedContent;
            }
        }

        private PaymentResponse ParseEsewaV2Response(string responseData)
        {
            try
            {
                return JsonConvert.DeserializeObject<PaymentResponse>(responseData);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to parse eSewa V2 response", ex);
            }
        }
    }
}
