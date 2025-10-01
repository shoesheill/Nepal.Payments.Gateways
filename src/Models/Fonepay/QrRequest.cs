using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Fonepay
{
    public class QrRequest
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("remarks1")]
        public string Remarks1 { get; set; }

        [JsonProperty("remarks2")]
        public string Remarks2 { get; set; }

        [JsonProperty("prn")]
        public string Prn { get; set; }

        [JsonProperty("merchantCode")]
        public string MerchantCode { get; set; }

        [JsonProperty("dataValidation")]
        public string DataValidation { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("taxAmount")]
        public string TaxAmount { get; set; }

        [JsonProperty("taxRefund")]
        public string TaxRefund { get; set; }
    }
}
