using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Khalti
{
    public class CustomerInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }
}
