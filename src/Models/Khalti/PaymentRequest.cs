using Nepal.Payments.Gateways.Models.Khalti.Nepal.Payments.Gateways.Models.Khalti;
using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Khalti
{
    public class PaymentRequest
    {
        [JsonProperty("return_url")]
        public string ReturnUrl { get; set; }

        [JsonProperty("website_url")]
        public string WebsiteUrl { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("purchase_order_id")]
        public string PurchaseOrderId { get; set; }

        [JsonProperty("purchase_order_name")]
        public string PurchaseOrderName { get; set; }

        [JsonProperty("customer_info")]
        public CustomerInfo CustomerInfo { get; set; }

        [JsonProperty("product_details")]
        public ProductDetail[] ProductDetails { get; set; }

        [JsonProperty("amount_breakdown")]
        public AmountBreakdown[] AmountBreakdown { get; set; }
    }
}