using System.Net.Http;
using System.Threading.Tasks;

namespace payment_gateway_nepal
{
    public interface IEndPointService
    {
        Task<(string apiUrl, HttpMethod httpMethod)> GetEndpoint(string gateway, PaymentVersion version, string action);
    }
}
