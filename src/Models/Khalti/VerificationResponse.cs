using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Khalti
{
    public class PaymentResponse
    {
        [JsonProperty("pidx")]
        public string Pidx { get; set; }
        [JsonProperty("total_amount")]
        public decimal TotalAmount { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("transaction_id")]
        public string TransactionId { get; set; }
        [JsonProperty("fee")]
        public decimal Fee { get; set; }
        [JsonProperty("refunded")]
        public bool Refunded { get; set; }
    }
}
