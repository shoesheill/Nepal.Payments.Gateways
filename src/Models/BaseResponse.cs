using System.Net;

namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Base response class that provides common properties for all API responses.
    /// </summary>
    public abstract class BaseResponse
    {
        /// <summary>
        /// Gets or sets the HTTP status code of the response.
        /// </summary>
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

        /// <summary>
        /// Gets or sets a clear message that describes the response.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the result object to be transmitted.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool Success { get; set; } = true;

        /// <summary>
        /// Gets or sets the error code if the operation failed.
        /// </summary>
        public int ErrorCode { get; set; }
    }
}
