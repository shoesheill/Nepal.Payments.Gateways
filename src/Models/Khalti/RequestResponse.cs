using System;
using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Khalti
{
    public class RequestResponse
    {
        [JsonProperty("pidx")]
        public string Pidx { get; set; }

        [JsonProperty("payment_url")]
        public string PaymentUrl { get; set; }

        [JsonProperty("expires_at")]
        public DateTime ExpiresAt { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
