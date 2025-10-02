using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Fonepay
{
    public class QrStatusResponse
    {
        [JsonProperty("fonepayTraceId")]
        public long FonepayTraceId { get; set; }

        [JsonProperty("merchantCode")]
        public string MerchantCode { get; set; }

        [JsonProperty("paymentStatus")]
        public string PaymentStatus { get; set; }

        [JsonProperty("prn")]
        public string Prn { get; set; }
    }
}
