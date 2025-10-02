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
using Nepal.Payments.Gateways.Models.Fonepay;
using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Services.Fonepay
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
            if (!(content is QrRequest request))
                throw new ArgumentException("Content must be of type QrRequest", nameof(content));

            try
            {
                // Generate HMAC signature for QR request
                string signature = GenerateQrSignature(request);
                request.DataValidation = signature;

                // Get the appropriate endpoint
                string baseUrl = _paymentMode == PaymentMode.Sandbox 
                    ? ApiEndpoints.Fonepay.SandboxBaseUrl 
                    : ApiEndpoints.Fonepay.BaseUrl;
                
                string endpoint = $"{baseUrl}{ApiEndpoints.Fonepay.QrGenerateUrl}";

                // Prepare headers
                Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                };

                // Make the API call
                var response = await _apiService.GetAsyncResult<QrResponse>(
                    endpoint,
                    ApiEndpoints.Fonepay.QrGenerateMethod,
                    headers,
                    null,
                    request
                );

                return ResponseConverter.ConvertTo<T>(new PaymentResult
                {
                    Data = response,
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                return ResponseConverter.ConvertTo<T>(new PaymentResult
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
                // Parse the verification data (PRN and MerchantCode)
                var verificationData = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                var prn = verificationData["prn"];
                var merchantCode = verificationData["merchantCode"];

                // Create QR status request
                var statusRequest = new QrStatusRequest
                {
                    Prn = prn,
                    MerchantCode = merchantCode,
                    DataValidation = GenerateQrStatusSignature(prn, merchantCode),
                    Username = verificationData["username"],
                    Password = verificationData["password"]
                };

                // Get the appropriate endpoint
                string baseUrl = _paymentMode == PaymentMode.Sandbox 
                    ? ApiEndpoints.Fonepay.SandboxBaseUrl 
                    : ApiEndpoints.Fonepay.BaseUrl;
                
                string endpoint = $"{baseUrl}{ApiEndpoints.Fonepay.QrStatusUrl}";

                // Prepare headers
                Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                };

                // Make the API call
                var response = await _apiService.GetAsyncResult<QrStatusResponse>(
                    endpoint,
                    ApiEndpoints.Fonepay.QrStatusMethod,
                    headers,
                    null,
                    statusRequest
                );

                return ResponseConverter.ConvertTo<T>(new PaymentResult
                {
                    Data = response,
                    Success = true,
                    Message = "QR status checked successfully"
                });
            }
            catch (Exception ex)
            {
                return ResponseConverter.ConvertTo<T>(new PaymentResult
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        public async Task<T> ProcessTaxRefundAsync<T>(TaxRefundRequest request)
        {
            try
            {
                // Generate HMAC signature for tax refund
                string signature = GenerateTaxRefundSignature(request);
                request.DataValidation = signature;

                // Get the appropriate endpoint
                string baseUrl = _paymentMode == PaymentMode.Sandbox 
                    ? ApiEndpoints.Fonepay.SandboxBaseUrl 
                    : ApiEndpoints.Fonepay.BaseUrl;
                
                string endpoint = $"{baseUrl}{ApiEndpoints.Fonepay.TaxRefundUrl}";

                // Prepare headers
                Dictionary<string, string> headers = new Dictionary<string, string>
                {
                    { "Content-Type", "application/json" }
                };

                // Make the API call
                var response = await _apiService.GetAsyncResult<TaxRefundResponse>(
                    endpoint,
                    ApiEndpoints.Fonepay.TaxRefundMethod,
                    headers,
                    null,
                    request
                );

                return ResponseConverter.ConvertTo<T>(new PaymentResult
                {
                    Data = response,
                    Success = response.Success,
                    Message = response.Message
                });
            }
            catch (Exception ex)
            {
                return ResponseConverter.ConvertTo<T>(new PaymentResult
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        private string GenerateQrSignature(QrRequest request)
        {
            // Create signature string based on whether tax refund is included
            string message;
            if (string.IsNullOrEmpty(request.TaxRefund))
            {
                // Without tax refund: AMOUNT,PRN,MERCHANT-CODE,REMARKS1,REMARKS2
                message = $"{request.Amount},{request.Prn},{request.MerchantCode},{request.Remarks1},{request.Remarks2}";
            }
            else
            {
                // With tax refund: AMOUNT,PRN,MERCHANT-CODE,REMARKS1,REMARKS2,TAXAMOUNT,TAXREFUND
                message = $"{request.Amount},{request.Prn},{request.MerchantCode},{request.Remarks1},{request.Remarks2},{request.TaxAmount},{request.TaxRefund}";
            }

            return HmacHelper.GenerateHmacSha512(message, _secretKey);
        }

        private string GenerateQrStatusSignature(string prn, string merchantCode)
        {
            // For QR status: HMAC-SHA512(PRN,MERCHANT-CODE)
            string message = $"{prn},{merchantCode}";
            return HmacHelper.GenerateHmacSha512(message, _secretKey);
        }

        private string GenerateTaxRefundSignature(TaxRefundRequest request)
        {
            // For tax refund: HMAC-SHA512(fonepayTraceId,merchantPRN,invoiceNumber,invoiceDate,transactionAmount,merchantCode)
            string message = $"{request.FonepayTraceId},{request.MerchantPRN},{request.InvoiceNumber},{request.InvoiceDate},{request.TransactionAmount},{request.MerchantCode}";
            return HmacHelper.GenerateHmacSha512(message, _secretKey);
        }
    }
}
