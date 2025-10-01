using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Fonepay
{
    public class TaxRefundResponse
    {
        [JsonProperty("fonepayTraceId")]
        public long FonepayTraceId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }
    }
}
