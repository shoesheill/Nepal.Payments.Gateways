using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.eSewa
{
    /// <summary>
    ///     eSewa transaction status. <see cref="Status" /> is one of COMPLETE, PENDING, AMBIGUOUS,
    ///     FULL_REFUND, PARTIAL_REFUND, CANCELED, NOT_FOUND.
    /// </summary>
    public class StatusResponse
    {
        [JsonProperty("product_code")]
        public string ProductCode { get; set; }
        [JsonProperty("transaction_uuid")]
        public string TransactionUuid { get; set; }
        [JsonProperty("total_amount")]
        public decimal? TotalAmount { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("ref_id")]
        public string RefId { get; set; }
    }
}
