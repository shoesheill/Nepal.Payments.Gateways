using System.Threading.Tasks;

namespace payment_gateway_nepal
{
    public interface IPaymentService
    {
        Task<T> InitiatePaymentAsync<T>(object content, PaymentVersion version);
        Task<T> VerifyPaymentAsync<T>(string content, PaymentVersion version);
    }
}
