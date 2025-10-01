using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Khalti
{
    public class AmountBreakdown
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }
    }
}
