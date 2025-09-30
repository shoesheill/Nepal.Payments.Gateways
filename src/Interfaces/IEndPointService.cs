using System.Net.Http;
using System.Threading.Tasks;

namespace Nepal.Payments.Gateways
{
    /// <summary>
    /// Interface for endpoint service that provides API endpoints for different payment gateways.
    /// </summary>
    public interface IEndpointService
    {
        /// <summary>
        /// Gets the API endpoint URL and HTTP method for the specified gateway, version, and action.
        /// </summary>
        /// <param name="gateway">The payment gateway name.</param>
        /// <param name="version">The API version.</param>
        /// <param name="action">The payment action.</param>
        /// <returns>A tuple containing the API URL and HTTP method.</returns>
        Task<(string apiUrl, HttpMethod httpMethod)> GetEndpoint(string gateway, PaymentVersion version, string action);
    }
}
