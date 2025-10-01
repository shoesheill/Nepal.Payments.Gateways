using System.Net.Http;
using System.Threading.Tasks;
using Nepal.Payments.Gateways.Enum;

namespace Nepal.Payments.Gateways.Interfaces
{
    public interface IEndpointService
    {
        Task<(string apiUrl, HttpMethod httpMethod)> GetEndpoint(string gateway, PaymentVersion version, string action);
    }
}
