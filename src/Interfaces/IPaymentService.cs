using System.Threading.Tasks;
using Nepal.Payments.Gateways.Enum;

namespace Nepal.Payments.Gateways.Interfaces
{
    public interface IPaymentService
    {
        Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version);
        Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version);
    }
}
