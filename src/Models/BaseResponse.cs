using System.Net;

namespace Nepal.Payments.Gateways.Models
{
    public abstract class BaseResponse
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; }
        public bool Success { get; set; } = true;
        public int ErrorCode { get; set; }
    }
}
