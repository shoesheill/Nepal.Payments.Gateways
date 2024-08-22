using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace payment_gateway_nepal.eSewa.V2
{
    public class eSewaPaymentService : IPaymentService
    {
        private readonly string _secretKey;
        private readonly PaymentMode _paymentMode;


        public eSewaPaymentService(string secretKey, PaymentMode paymentMode)
        {
            _secretKey = secretKey;
            _paymentMode = paymentMode;
        }

        public async Task<T> ProcessPayment<T>(object content, PaymentVersion version)
        {
            var json = JsonConvert.SerializeObject(content);
            eSewaRequest request = JsonConvert.DeserializeObject<eSewaRequest>(json) ?? throw new ArgumentException("Invalid content type", nameof(content));

            // Generate the signature
            string message = $"total_amount={request.TotalAmount},transaction_uuid={request.TransactionUuid},product_code={request.ProductCode}";
            request.Signature = HmacHelper.GenerateHmacSha256Signature(message, _secretKey);
            var (apiUrl, httpMethod) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.eSewa, version, PaymentAction.ProcessPayment, _paymentMode);
            Dictionary<string, string> headers = new Dictionary<string, string>();
            var formContent = new Dictionary<string, string>
            {
                { "amount", request.Amount.ToString() },
                { "tax_amount", request.TaxAmount.ToString() },
                { "total_amount", request.TotalAmount.ToString() },
                { "transaction_uuid", request.TransactionUuid },
                { "product_code", request.ProductCode },
                { "product_service_charge", request.ProductServiceCharge.ToString() },
                { "product_delivery_charge", request.ProductDeliveryCharge.ToString() },
                { "success_url", request.SuccessUrl },
                { "failure_url", request.FailureUrl },
                { "signed_field_names", request.SignedFieldNames },
                { "signature", request.Signature }
            };
            // Send the request
            var response = await new ApiService(new HttpClient()).GetAsyncResult<string>(apiUrl, httpMethod, headers, formContent);
            ApiResponse apiResponse = new ApiResponse { data = response };
            if (string.Equals(apiUrl, response))
            {
                apiResponse.status = HttpStatusCode.BadRequest;
                apiResponse.error_code = (int)HttpStatusCode.BadRequest;
                apiResponse.success = false;
            }

            return (T)Convert.ChangeType(apiResponse, typeof(T));
        }

        public async Task<T> VerifyPayment<T>(string encodedCode, PaymentVersion version)
        {
            // Decode the Base64 string to binary data
            byte[] binaryData = Convert.FromBase64String(encodedCode);
            // Convert the binary data to a string (using UTF-8 encoding)
            string decodedString = Encoding.UTF8.GetString(binaryData);
            try
            {
                eSewaResponse eSewaResponse = JsonConvert.DeserializeObject<eSewaResponse>(decodedString);
                if (!(eSewaResponse is eSewaResponse request))
                {
                    throw new ArgumentException("Invalid content type", nameof(eSewaResponse));
                }
                var (apiUrl, httpMethod) = PaymentEndpointFactory.GetEndpoint(PaymentMethod.eSewa, version, PaymentAction.VerifyPayment, _paymentMode);
                string url = apiUrl + $"?product_code={eSewaResponse.product_code}&total_amount={eSewaResponse.total_amount}&transaction_uuid={eSewaResponse.transaction_uuid}";
                eSewaResponse response = await new ApiService(new HttpClient()).GetAsyncResult<eSewaResponse>(url, httpMethod, null, null);
                eSewaResponse.status = response.status;
                return (T)Convert.ChangeType(eSewaResponse, typeof(T));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async Task<T> PaymentCheck<T>(object content, PaymentVersion version)
        //{
        //    if (content is not eSewaRequest request)
        //    {
        //        throw new ArgumentException("Invalid content type", nameof(content));
        //    }

        //    string url = $"https://epay.esewa.com.np/api/epay/transaction/status/?product_code={request.ProductCode}&total_amount={request.TotalAmount}&transaction_uuid={request.TransactionUuid}";
        //    var response = await _httpClient.GetAsync(url);
        //    response.EnsureSuccessStatusCode();

        //    var responseBody = await response.Content.ReadAsStringAsync();
        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseBody);
        //}
    }
}


