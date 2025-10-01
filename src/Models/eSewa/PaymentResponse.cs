using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.eSewa
{
    public class PaymentResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("signature")]
        public string Signature { get; set; }
        [JsonProperty("transaction_code")]
        public string TransactionCode { get; set; }
        [JsonProperty("total_amount")]
        public decimal TotalAmount { get; set; }
        [JsonProperty("transaction_uuid")]
        public string TransactionUuid { get; set; }
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }
        [JsonProperty("signed_field_names")]
        public string SignedFieldNames { get; set; }
    }
}
