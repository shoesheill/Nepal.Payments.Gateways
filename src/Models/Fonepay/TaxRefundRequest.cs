using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Fonepay
{
    public class TaxRefundRequest
    {
        [JsonProperty("fonepayTraceId")]
        public long FonepayTraceId { get; set; }

        [JsonProperty("transactionAmount")]
        public string TransactionAmount { get; set; }

        [JsonProperty("merchantPRN")]
        public string MerchantPRN { get; set; }

        [JsonProperty("invoiceNumber")]
        public string InvoiceNumber { get; set; }

        [JsonProperty("invoiceDate")]
        public string InvoiceDate { get; set; }

        [JsonProperty("merchantCode")]
        public string MerchantCode { get; set; }

        [JsonProperty("dataValidation")]
        public string DataValidation { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
