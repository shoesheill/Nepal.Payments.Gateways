using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.eSewa
{
    public class PaymentRequest
    {
        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("tax_amount")]
        public string TaxAmount { get; set; }

        [JsonProperty("total_amount")]
        public string TotalAmount { get; set; }

        [JsonProperty("transaction_uuid")]
        public string TransactionUuid { get; set; }

        [JsonProperty("product_code")]
        public string ProductCode { get; set; }

        [JsonProperty("product_service_charge")]
        public string ProductServiceCharge { get; set; }

        [JsonProperty("product_delivery_charge")]
        public string ProductDeliveryCharge { get; set; }

        [JsonProperty("success_url")]
        public string SuccessUrl { get; set; }

        [JsonProperty("failure_url")]
        public string FailureUrl { get; set; }

        [JsonProperty("signed_field_names")]
        public string SignedFieldNames { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}
