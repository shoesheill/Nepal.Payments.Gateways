using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Enum;
using Nepal.Payments.Gateways.Factories;
using Nepal.Payments.Gateways.Helper;
using Nepal.Payments.Gateways.Helper.ApiCall;
using Nepal.Payments.Gateways.Interfaces;
using Nepal.Payments.Gateways.Models;
using Nepal.Payments.Gateways.Models.eSewa;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nepal.Payments.Gateways.Services.Esewa.V2
{
    public class PaymentService : IPaymentService, IStatusCheckService
    {
        private readonly string _secretKey;
        private readonly PaymentMode _paymentMode;
        private readonly string _baseUrl;
        private readonly ApiService _apiService;

        public PaymentService(string secretKey, PaymentMode paymentMode) : this(secretKey, paymentMode, null) { }

        public PaymentService(string secretKey, PaymentMode paymentMode, string baseUrl)
        {
            _secretKey = secretKey ?? throw new ArgumentNullException(nameof(secretKey));
            _paymentMode = paymentMode;
            _baseUrl = baseUrl;
            _apiService = new ApiService(new HttpClient());
        }

        public async Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version)
        {
            if (!(content is PaymentRequest request))
                throw new ArgumentException("Content must be of type PaymentRequest", nameof(content));

            try
            {
                request.Signature = GenerateEsewaV2Signature(request);
                var (endpoint, method) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Esewa, PaymentVersion.V2, PaymentAction.ProcessPayment, _paymentMode, _baseUrl);
                var keyValuePairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonConvert.SerializeObject(request));
                var response = await _apiService.GetAsyncResult<string>(endpoint, method, keyValuePairs: keyValuePairs);

                return ResponseConverter.ConvertTo<T>(new PaymentResult
                {
                    Data = new RequestResponse { PaymentUrl = response ?? "" },
                    Success = true,
                    Message = "Payment initiated successfully"
                });
            }
            catch (Exception ex)
            {
                return ResponseConverter.ConvertTo<T>(new PaymentResult { Success = false, Message = ex.Message });
            }
        }

        /// <summary>Verifies the signed <c>data</c> eSewa appends to success_url (base64 JSON).</summary>
        public Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Verification content cannot be null or empty", nameof(content));

            try
            {
                var json = JObject.Parse(DecodeBase64Content(content));
                if (!HasValidSignature(json))
                    throw new InvalidOperationException("Invalid signature in eSewa V2 response");

                return Task.FromResult(ResponseConverter.ConvertTo<T>(new PaymentResult
                {
                    Data = json.ToObject<PaymentResponse>(),
                    Success = true,
                    Message = "Payment verified successfully"
                }));
            }
            catch (Exception ex)
            {
                return Task.FromResult(ResponseConverter.ConvertTo<T>(new PaymentResult { Success = false, Message = ex.Message }));
            }
        }

        /// <summary>Asks eSewa's status API what happened — the only server-authoritative answer.</summary>
        public async Task<T> CheckStatusAsync<T>(object content)
        {
            if (!(content is StatusRequest request))
                throw new ArgumentException("Content must be of type StatusRequest", nameof(content));

            try
            {
                var (endpoint, _) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.Esewa, PaymentVersion.V2, PaymentAction.CheckPayment, _paymentMode, _baseUrl);
                var url = BuildStatusUrl(endpoint, request);
                var response = await _apiService.GetAsyncResult<StatusResponse>(url, HttpMethod.Get);

                return ResponseConverter.ConvertTo<T>(new PaymentResult { Data = response, Success = true, Message = "Status retrieved successfully" });
            }
            catch (Exception ex)
            {
                return ResponseConverter.ConvertTo<T>(new PaymentResult { Success = false, Message = ex.Message });
            }
        }

        internal static string BuildStatusUrl(string statusEndpoint, StatusRequest request)
        {
            if (!statusEndpoint.Contains("/epay/transaction/status"))
                statusEndpoint = PaymentEndpointFactory.Combine(statusEndpoint, Constants.ApiEndpoints.Esewa.V2.VerifyPaymentUrl);

            return $"{statusEndpoint}?product_code={Uri.EscapeDataString(request.ProductCode ?? "")}"
                   + $"&total_amount={Uri.EscapeDataString(request.TotalAmount ?? "")}"
                   + $"&transaction_uuid={Uri.EscapeDataString(request.TransactionUuid ?? "")}";
        }

        private string GenerateEsewaV2Signature(PaymentRequest request)
        {
            var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["total_amount"] = request.TotalAmount,
                ["transaction_uuid"] = request.TransactionUuid,
                ["product_code"] = request.ProductCode,
            };
            var message = string.Join(",", request.SignedFieldNames.Split(',')
                .Select(f => f.Trim())
                .Where(values.ContainsKey)
                .Select(f => $"{f}={values[f]}"));
            return HmacHelper.GenerateHmacSha256Signature(message, _secretKey);
        }

        private bool HasValidSignature(JObject json)
        {
            var signature = (string)json["signature"];
            var signedFieldNames = (string)json["signed_field_names"];
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(signedFieldNames))
                return false;

            var parts = new List<string>();
            foreach (var field in signedFieldNames.Split(',').Select(f => f.Trim()))
            {
                var token = json[field];
                if (token == null)
                    return false;
                parts.Add($"{field}={(token.Type == JTokenType.String ? (string)token : token.ToString(Formatting.None))}");
            }

            return FixedTimeEquals(HmacHelper.GenerateHmacSha256Signature(string.Join(",", parts), _secretKey), signature);
        }

        private static bool FixedTimeEquals(string a, string b)
        {
            if (a.Length != b.Length)
                return false;
            var diff = 0;
            for (var i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }

        private static string DecodeBase64Content(string encodedContent)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(encodedContent));
            }
            catch (FormatException)
            {
                return encodedContent;
            }
        }
    }
}
