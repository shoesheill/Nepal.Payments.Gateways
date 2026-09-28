using System.Threading.Tasks;

namespace Nepal.Payments.Gateways.Interfaces
{
    internal interface IStatusCheckService
    {
        Task<T> CheckStatusAsync<T>(object content);
    }
}
