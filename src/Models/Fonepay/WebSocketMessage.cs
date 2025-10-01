using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Fonepay
{
    public class WebSocketMessage
    {
        [JsonProperty("merchantId")]
        public int MerchantId { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("transactionStatus")]
        public string TransactionStatus { get; set; }
    }

    public class TransactionStatus
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("qrVerified")]
        public bool? QrVerified { get; set; }

        [JsonProperty("paymentSuccess")]
        public bool? PaymentSuccess { get; set; }

        [JsonProperty("remarks1")]
        public string Remarks1 { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }
    }
}
