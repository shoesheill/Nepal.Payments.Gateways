using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Khalti
{
    namespace Nepal.Payments.Gateways.Models.Khalti
    {
        public class ProductDetail
        {
            [JsonProperty("identity")] 
            public string Identity { get; set; }

            [JsonProperty("name")] 
            public string Name { get; set; }

            [JsonProperty("total_price")] 
            public int TotalPrice { get; set; }

            [JsonProperty("quantity")] 
            public int Quantity { get; set; }

            [JsonProperty("unit_price")] 
            public int UnitPrice { get; set; }
        }
    }
}