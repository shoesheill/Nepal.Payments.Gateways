using Newtonsoft.Json;

namespace Nepal.Payments.Gateways.Models.Fonepay
{
    public class QrResponse
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("qrMessage")]
        public string QrMessage { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("thirdpartyQrWebSocketUrl")]
        public string ThirdpartyQrWebSocketUrl { get; set; }

        [JsonProperty("documentation")]
        public string Documentation { get; set; }

        [JsonProperty("errorCode")]
        public int? ErrorCode { get; set; }
    }
}
